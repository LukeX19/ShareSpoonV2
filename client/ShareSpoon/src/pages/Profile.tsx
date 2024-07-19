import React, { useEffect, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Button, IconButton, Typography, useTheme, Grid } from '@mui/material';
import ArrowBackIosNewRoundedIcon from '@mui/icons-material/ArrowBackIosNewRounded';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import TurnedInIcon from '@mui/icons-material/TurnedIn';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { IUserWithInteractions } from '../interfaces/UserInterface';
import { getCurrentUser } from '../services/UserService';
import { IRecipe } from '../interfaces/RecipeInterface';
import { deleteRecipe, getUserLikedRecipes, getUserRecipes } from '../services/RecipeService';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import ConfirmationDialog from '../components/ConfirmationDialog';
import CustomAvatar from '../components/CustomAvatar';

const Profile: React.FC = () => {
  const navigate = useNavigate();
  const theme = useTheme();
  const [user, setUser] = useState<IUserWithInteractions | null>(null);
  const [recipes, setRecipes] = useState<IRecipe[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [hasNextPage, setHasNextPage] = useState<boolean>(false);
  const [view, setView] = useState<'myRecipes' | 'likedRecipes'>('myRecipes');
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [selectedRecipeId, setSelectedRecipeId] = useState<number | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await getCurrentUser();
        if (response && response.status === 200) {
          setUser(response.data);
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchUser();
  }, []);

  useEffect(() => {
    const fetchRecipes = async () => {
      if (user) {
        try {
          setLoading(true);
          const data = await getUserRecipes(user.id, 1, 10);
          setRecipes(data.elements);
          setPageIndex(data.pageIndex);
          setHasNextPage(data.hasNextPage);
        } catch (error) {
          console.error(error);
        } finally {
          setLoading(false);
        }
      }
    };

    fetchRecipes();
  }, [user]);

  const loadMoreRecipes = async () => {
    try {
      setLoading(true);
      const nextPageIndex = pageIndex + 1;
      const data = await getUserRecipes(user!.id, nextPageIndex, 10);
      setRecipes((prevRecipes) => [...prevRecipes, ...data.elements]);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const loadMoreLikedRecipes = async () => {
    try {
      setLoading(true);
      const nextPageIndex = pageIndex + 1;
      const data = await getUserLikedRecipes(user!.id, nextPageIndex, 10);
      setRecipes((prevRecipes) => [...prevRecipes, ...data.elements]);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const loadLikedRecipes = async () => {
    if (!user) return;
    try {
      setLoading(true);
      const data = await getUserLikedRecipes(user.id, 1, 10);
      setRecipes(data.elements);
      setPageIndex(data.pageIndex);
      setHasNextPage(data.hasNextPage);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleSwitchToLikedRecipes = () => {
    setView('likedRecipes');
    loadLikedRecipes();
  };

  const handleSwitchToMyRecipes = () => {
    setView('myRecipes');
    const fetchRecipes = async () => {
      if (user) {
        try {
          setLoading(true);
          const data = await getUserRecipes(user.id, 1, 10);
          setRecipes(data.elements);
          setPageIndex(data.pageIndex);
          setHasNextPage(data.hasNextPage);
        } catch (error) {
          console.error(error);
        } finally {
          setLoading(false);
        }
      }
    };

    fetchRecipes();
  };

  const handleRecipeClick = (recipeId: number) => {
    navigate(`/recipe/${recipeId}`);
  };

  const handleIconClick = (event: React.MouseEvent, action: () => void) => {
    event.stopPropagation();
    action();
  };

  const handleOpenDialog = (recipeId: number) => {
    setSelectedRecipeId(recipeId);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedRecipeId(null);
  };

  const handleConfirmDelete = async () => {
    try {
      if (selectedRecipeId !== null) {
        const response = await deleteRecipe(selectedRecipeId);
        if (response && response.status === 204) {
          toast.success("Recipe deleted successfully!");
          const deletedRecipe = recipes.find(recipe => recipe.id === selectedRecipeId);
          setRecipes((prevRecipes) => prevRecipes.filter(recipe => recipe.id !== selectedRecipeId));
          if (user && deletedRecipe) {
            setUser({
              ...user,
              postedRecipesCounter: user.postedRecipesCounter - 1,
              receivedLikesCounter: user.receivedLikesCounter - deletedRecipe.likesCounter,
            });
          }
          handleCloseDialog();
        }
      }
    } catch (error) {
      console.log(error);
      toast.error("An error has occured! Please try again later.");
    }
  };

  return (
    <>
      <Helmet>
        <title>My Profile</title>
      </Helmet>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'flex-start',
          minHeight: '100vh',
          width: '100%',
          backgroundColor: '#f5f5f5',
          paddingTop: 8,
        }}
      >
        <Box
          sx={{
            width: '100%',
            maxWidth: 1000,
            backgroundColor: '#ffffff',
            borderRadius: 2,
            boxShadow: 3,
            padding: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            minHeight: '100vh'
          }}
        >
          <CustomAvatar
            firstName={user?.firstName || "First"}
            lastName={user?.lastName || "Last"}
            pictureURL={user?.pictureURL || ""}
            sx={{ width: 120, height: 120, mb: 2, mt: 6, fontSize: "60px" }}
          />
          <Typography variant="h4">{`${user?.firstName} ${user?.lastName}`}</Typography>
          {user?.role === 2 &&
            <span style={{ backgroundColor: '#BDF2D5', paddingLeft: '5px', paddingRight: '5px' }}>Promoted User</span>
          }
          <Box sx={{ display: 'flex', justifyContent: 'space-around', width: '100%', mb: 4, mt: 3 }}>
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="h6">{user?.postedRecipesCounter}</Typography>
              <Typography variant="body1">Posted Recipes</Typography>
            </Box>
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="h6">{user?.receivedLikesCounter}</Typography>
              <Typography variant="body1">Received Likes</Typography>
            </Box>
          </Box>
          <Grid container sx={{display: 'flex', mb: 2, height: '100px'}}>
            <Grid item xs={4} sx={{display: 'flex', alignItems: 'center', justifyContent: 'flex-end'}}>
              {view === 'likedRecipes' && (
                <IconButton onClick={handleSwitchToMyRecipes}>
                  <ArrowBackIosNewRoundedIcon />
                </IconButton>
              )}
            </Grid>
            <Grid item xs={4} sx={{display: 'flex', justifyContent: 'center', alignItems: 'center'}}>
              <Typography variant="h5" sx={{ mx: 2, my: 2}}>
                {view === 'myRecipes' ? 'My Recipes' : 'Liked Recipes'}
              </Typography>
            </Grid>
            <Grid item xs={4} sx={{display: 'flex', alignItems: 'center', justifyContent: 'flex-start'}}>
              {view === 'myRecipes' && (
                <IconButton onClick={handleSwitchToLikedRecipes}>
                  <ArrowForwardIosIcon />
                </IconButton>
              )}
            </Grid>
          </Grid>
          {recipes.length === 0 && (
            <Typography variant="body1">
              {view === 'myRecipes' ? 'No recipes posted yet.' : 'No liked recipes yet.'}
            </Typography>
          )}
          {recipes.map((recipe) => (
            <Box
              key={recipe.id}
              sx={{
                position: 'relative',
                width: '100%',
                backgroundColor: '#F9F9F9',
                borderRadius: 2,
                px: 2,
                py: 3,
                mb: 2,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                cursor: 'pointer',
              }}
              onClick={() => handleRecipeClick(recipe.id)}
            >
              {view === 'myRecipes' && (
                <Box
                  sx={{
                    display: 'flex',
                    height: '100%',
                    width: '100px'
                  }}
                >
                  <TurnedInIcon sx={{ fontSize: '2rem', color: theme.palette.secondary.main}} />
                </Box>
              )}
              <Box sx={{ flex: 1, textAlign: 'center' }}>
                <Typography variant="body1">
                  <Typography component="span" color="text.primary">{recipe.name}</Typography>
                  {view === 'myRecipes' && (
                    <Typography component="span" color="text.secondary">{` [${new Date(recipe.createdAt).toLocaleString('ro-RO').slice(0, -3)}]`}</Typography>
                  )}
                </Typography>
                <Typography variant="body2">
                  Likes: {recipe.likesCounter} | Comments: {recipe.commentsCounter}
                </Typography>
                {view === 'likedRecipes' && recipe.user && (
                  <Typography variant="body2">
                    Posted by: {recipe.user.firstName} {recipe.user.lastName}
                  </Typography>
                )}
              </Box>
              {view === 'myRecipes' && (
                <Box sx={{ display: 'flex', width: '100px', gap: 2, mr: 0, justifyContent: 'space-between' }}>
                  <IconButton
                    onClick={(event) => handleIconClick(event, () => navigate(`/recipe/${recipe.id}/update`))}
                    sx={{
                      backgroundColor: '#EBCC12',
                      color: 'white',
                      borderRadius: '50%',
                      width: 40,
                      height: 40,
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      boxShadow: 3,
                      cursor: 'pointer',
                      '&:hover': {
                        backgroundColor: '#7D6C0A',
                      },
                    }}
                  >
                    <EditIcon />
                  </IconButton>
                  <IconButton
                    onClick={(event) => handleIconClick(event, () => handleOpenDialog(recipe.id))}
                    sx={{
                      backgroundColor: 'red',
                      color: 'white',
                      borderRadius: '50%',
                      width: 40,
                      height: 40,
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      boxShadow: 3,
                      cursor: 'pointer',
                      '&:hover': {
                        backgroundColor: 'darkred',
                      },
                    }}
                  >
                    <DeleteIcon />
                  </IconButton>
                </Box>
              )}
            </Box>
          ))}
          {loading && <Typography variant="body1">Loading...</Typography>}
          {hasNextPage && !loading && (
            <Button onClick={view === 'myRecipes' ? loadMoreRecipes : loadMoreLikedRecipes} variant="contained" color="primary">
              Load More
            </Button>
          )}
        </Box>
      </Box>
      <ConfirmationDialog
        open={openDialog}
        title="Delete Confirmation"
        contentText="Are you sure you want to delete this recipe? Warning: this action is permanent and cannot be undone."
        onClose={handleCloseDialog}
        onConfirm={handleConfirmDelete}
      />
    </>
  );
};

export default Profile;