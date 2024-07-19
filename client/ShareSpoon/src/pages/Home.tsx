import React, { useEffect, useRef, useState } from "react";
import { Box, Button, Typography, IconButton, useTheme} from "@mui/material";
import Masonry from '@mui/lab/Masonry';
import { styled } from '@mui/material/styles';
import FavoriteBorderRoundedIcon from '@mui/icons-material/FavoriteBorderRounded';
import ChatBubbleOutlineRoundedIcon from '@mui/icons-material/ChatBubbleOutlineRounded';
import VerifiedRoundedIcon from '@mui/icons-material/VerifiedRounded';
import FavoriteIcon from '@mui/icons-material/Favorite';
import { Helmet } from "react-helmet-async";
import { getRecipes, searchRecipes } from "../services/RecipeService";
import { createLike, deleteLike } from "../services/LikeService";
import { IRecipe } from "../interfaces/RecipeInterface";
import { Link, useNavigate } from "react-router-dom";
import StaticSearchbar from "../components/StaticSearchbar";
import FilterListIcon from '@mui/icons-material/FilterList';
import FilterDialog, { FilterDialogHandle } from "../components/FilterDialog";
import { useUser } from "../hooks/useUser";

const ImageContainer = styled(Box)(({ theme }) => ({
  position: 'relative',
  overflow: 'hidden',
  borderRadius: 4,
  '&:hover .overlay': {
    bottom: 0,
  }
}));

const Overlay = styled(Box)(({ theme }) => ({
  position: 'absolute',
  left: 0,
  right: 0,
  bottom: '-100%',
  backgroundColor: 'rgba(255, 255, 255, 0.9)',
  color: 'black',
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  justifyContent: 'center',
  padding: theme.spacing(2),
  borderRadius: 4,
  transition: 'bottom 0.3s ease',
  textAlign: 'center',
}));

const Home: React.FC = () => {
  const theme = useTheme();
  const { user } = useUser();
  const [recipes, setRecipes] = useState<IRecipe[]>([]);
  const [pageIndex, setPageIndex] = useState(1);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState<string | null>("");
  const [isSearching, setIsSearching] = useState(false);
  const [totalResults, setTotalResults] = useState(0);
  const [filterDialogOpen, setFilterDialogOpen] = useState(false);
  const [filterParams, setFilterParams] = useState<{ promotedUsers: boolean, difficulties: number[], tagIds: number[] }>({ promotedUsers: false, difficulties: [], tagIds: [] });
  const navigate = useNavigate();
  const searchbarRef = useRef<HTMLInputElement>(null);
  const filterDialogRef = useRef<FilterDialogHandle>(null);

  useEffect(() => {
    retrieveRecipes();
  }, []);

  const retrieveRecipes = async () => {
    try {
      setLoading(true);
      const data = await getRecipes(1, 10);
      setRecipes(data.elements);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  const loadMoreRecipes = async () => {
    try {
      setLoading(true);
      const nextPageIndex = pageIndex + 1;
      const data = await getRecipes(nextPageIndex, 10);
      setRecipes((prevRecipes) => [...prevRecipes, ...data.elements]);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  const loadMoreSearchResults = async () => {
    try {
      setLoading(true);
      const nextPageIndex = pageIndex + 1;
      const data = await searchRecipes(searchQuery, filterParams.promotedUsers, filterParams.difficulties, filterParams.tagIds, nextPageIndex, 10);
      setRecipes((prevRecipes) => [...prevRecipes, ...data.elements]);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  const handleLikeToggle = async (recipeId: number, liked: boolean) => {
    try {
      if (liked) {
        const response = await deleteLike(recipeId);
        if (response && response.status === 204) {
          setRecipes((prevRecipes) =>
            prevRecipes.map((recipe) =>
              recipe.id === recipeId
                ? {
                    ...recipe,
                    likesCounter: recipe.likesCounter - 1,
                    currentUserLiked: false,
                  }
                : recipe
            )
          );
        }
      } else {
        const response = await createLike({ recipeId });
        if (response && response.status === 201) {
          setRecipes((prevRecipes) =>
            prevRecipes.map((recipe) =>
              recipe.id === recipeId
                ? {
                    ...recipe,
                    likesCounter: recipe.likesCounter + 1,
                    currentUserLiked: true,
                  }
                : recipe
            )
          );
        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handleSearch = async (query: string) => {
    try {
      setSearchQuery(query);
      setLoading(true);
      setIsSearching(true);
      const data = await searchRecipes(query, filterParams.promotedUsers, filterParams.difficulties, filterParams.tagIds, 1, 10);
      setRecipes(data.elements);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
      setTotalResults(data.resultsCount);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  const handleSearchKeyDown = async (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter' && event.currentTarget.value) {
      handleSearch(event.currentTarget.value);
    }
  };

  const handleSearchClick = () => {
    if (searchbarRef.current?.value) {
      handleSearch(searchbarRef.current.value);
    }
  };

  const handleClearSearch = () => {
    setSearchQuery("");
    setIsSearching(false);
    setTotalResults(0);
    if (searchbarRef.current) {
      searchbarRef.current.value = "";
    }
    retrieveRecipes();
  };

  const handleFilterClick = () => {
    setFilterDialogOpen(true);
  };

  const handleFilterDialogClose = (filters: { promotedUsers: boolean, difficulties: number[], tagIds: number[] }) => {
    setFilterDialogOpen(false);
    setFilterParams(filters);
    handleSearchWithFilters(filters);
  };

  const handleSearchWithFilters = async (filters: { promotedUsers: boolean, difficulties: number[], tagIds: number[] }) => {
    try {
      setLoading(true);
      const data = await searchRecipes(searchQuery, filters.promotedUsers, filters.difficulties, filters.tagIds, 1, 10);
      setRecipes(data.elements);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
      setTotalResults(data.resultsCount);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  const handleClearFilter = () => {
    setFilterParams({ promotedUsers: false, difficulties: [], tagIds: [] });
    if (filterDialogRef.current) {
      filterDialogRef.current.clearFilters();
    }
    handleClearSearch();
  };

  if (loading && recipes.length === 0) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <Helmet>
        <title>Home</title>
      </Helmet>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'flex-start',
          minHeight: '100vh',
          width: '100%',
        }}
      >
        <Box
          sx={{
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            mb: 4,
            mt: 14,
            px: 2
          }}
        >
          <StaticSearchbar onKeyDown={handleSearchKeyDown} onSearchClick={handleSearchClick} ref={searchbarRef} />
          <Button variant="contained" color="primary" startIcon={<FilterListIcon />} sx={{ ml: 3 }} onClick={handleFilterClick}>
            Filter
          </Button>
        </Box>
        {(isSearching || filterParams.promotedUsers || filterParams.difficulties.length > 0 || filterParams.tagIds.length > 0) && (
          <Box sx={{ mb: 2, textAlign: 'center' }}>
            <Typography variant="body1" sx={{ mb: 1 }}>
              Found {totalResults} results.
            </Typography>
            <Button onClick={handleClearFilter} sx={{color: 'red'}}>Clear</Button>
          </Box>
        )}
        <Box sx={{ width: '100%', maxWidth: 1000, minHeight: 829, pl: 2}}>
          <Masonry columns={{ xs: 1, sm: 2, md: 3 }} spacing={2}>
            {recipes.map((recipe) => {
              return (
                <ImageContainer key={recipe.id} sx={{position: 'relative'}}>
                  <Link to={`/recipe/${recipe.id}`} style={{ textDecoration: 'none' }}>
                    <img
                      srcSet={`${recipe.pictureURL}?w=162&auto=format&dpr=2 2x`}
                      src={`${recipe.pictureURL}?w=162&auto=format`}
                      alt={recipe.name}
                      loading="lazy"
                      style={{
                        display: 'block',
                        width: '100%',
                        borderRadius: 7
                      }}
                    />
                  </Link>
                  {recipe.user.role === 2 &&
                    <VerifiedRoundedIcon 
                      sx={{
                        color: theme.palette.primary.main,
                        position: 'absolute',
                        top: 8,
                        right: 8,
                        backgroundColor: "black",
                        borderRadius: '50%',
                        fontSize: '1.8rem'
                      }}
                    />
                  }
                  <Overlay className="overlay">
                    <Typography variant="h6">{recipe.name}</Typography>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <Box sx={{ display: 'flex', alignItems: 'center', marginRight: 2 }}>
                        <IconButton
                          aria-label="like"
                          size="small"
                          onClick={() => user?.role !== 0 && handleLikeToggle(recipe.id, recipe.currentUserLiked)}
                          color={recipe.currentUserLiked ? 'error' : 'default'}
                        >
                          {recipe.currentUserLiked ? <FavoriteIcon /> : <FavoriteBorderRoundedIcon />}
                        </IconButton>
                        <Typography variant="body2" color="text.secondary">
                          {recipe.likesCounter}
                        </Typography>
                      </Box>
                      <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <IconButton
                          aria-label="comment"
                          size="small"
                          onClick={() => navigate(`/recipe/${recipe.id}#comments`)}
                        >
                          <ChatBubbleOutlineRoundedIcon />
                        </IconButton>
                        <Typography variant="body2" color="text.secondary">
                          {recipe.commentsCounter}
                        </Typography>
                      </Box>
                    </Box>
                  </Overlay>
                </ImageContainer>
              );
            })}
          </Masonry>
        </Box>
        {hasNextPage && (
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'center',
              width: '100%',
              maxWidth: 1000,
              mt: 2,
              mb: 4,
              pl: 2,
              pr: 2
            }}
          >
            <Button
              onClick={isSearching || filterParams.promotedUsers || filterParams.difficulties.length > 0 || filterParams.tagIds.length > 0 ? loadMoreSearchResults : loadMoreRecipes}
              variant="outlined"
              sx={{
                width: '100%',
                borderColor: 'rgba(0, 0, 0, 0.23)',
                color: 'rgba(0, 0, 0, 0.87)',
              }}
              disabled={loading}
            >
              {loading ? 'Loading...' : 'Load More'}
            </Button>
          </Box>
        )}
      </Box>
      <FilterDialog ref={filterDialogRef} open={filterDialogOpen} onClose={handleFilterDialogClose} onClear={handleClearFilter} />
    </>
  );
};

export default Home;
