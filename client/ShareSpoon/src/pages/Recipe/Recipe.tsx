import React, { useEffect, useState } from 'react';
import './Recipe.css';
import { Box, Button, CardMedia, Checkbox, FormControlLabel, Grid, IconButton, List, ListItem, Paper, TextField, Typography, useTheme, Divider } from '@mui/material';
import AvTimerRoundedIcon from '@mui/icons-material/AvTimerRounded';
import SoupKitchenRoundedIcon from '@mui/icons-material/SoupKitchenRounded';
import { Helmet } from 'react-helmet-async';
import { useParams } from 'react-router-dom';
import { getRecipeById } from '../../services/RecipeService';
import { IRecipe } from '../../interfaces/RecipeInterface';
import BadRequest from '../BadRequest/BadRequest';
import CustomAvatar from '../../components/CustomAvatar';
import FavoriteBorderRoundedIcon from '@mui/icons-material/FavoriteBorderRounded';
import FavoriteIcon from '@mui/icons-material/Favorite';
import { createLike, deleteLike } from '../../services/LikeService';
import { createComment, deleteComment, getCommentsForRecipe } from '../../services/CommentService';
import { IComment, ICommentRequest } from '../../interfaces/CommentInterface';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import RecipePdfDocument from '../../components/RecipePdfDocument';
import { PDFDownloadLink } from '@react-pdf/renderer';
import { useUser } from '../../hooks/useUser';
import { toast } from 'react-toastify';
import ConfirmationDialog from '../../components/ConfirmationDialog';
import PrintRoundedIcon from '@mui/icons-material/PrintRounded';
import TagChip from '../../components/TagChip';

const difficultyLevels = [
  { value: 1, label: 'Easy' },
  { value: 2, label: 'Medium' },
  { value: 3, label: 'Hard' },
];

const quantityTypes = [
  { value: 1, label: 'None' },
  { value: 2, label: 'Grams' },
  { value: 3, label: 'Kilograms' },
  { value: 4, label: 'Milliliters' },
  { value: 5, label: 'Liters' },
  { value: 6, label: 'Pieces' },
  { value: 7, label: 'Slices' },
  { value: 8, label: 'Cups' },
  { value: 9, label: 'Teaspoons' },
  { value: 10, label: 'Tablespoons' },
];

const getRoleText = (role: number): string => {
  switch (role) {
    case 1:
      return "";
    case 2:
      return "Promoted User";
    default:
      return "Unknown Role";
  }
};

const getQuantityTypeText = (quantityType: number): string => {
  const type = quantityTypes.find((qt) => qt.value === quantityType);
  return type ? type.label : '';
};

const Recipe: React.FC = () => {
  const { user } = useUser();
  const { recipeId } = useParams<{ recipeId: string }>();
  const theme = useTheme();
  const [recipe, setRecipe] = useState<IRecipe | null>(null);
  const [loading, setLoading] = useState(true);
  const [comments, setComments] = useState<IComment[]>([]);
  const [checkedIngredients, setCheckedIngredients] = useState<Set<number>>(new Set());
  const [pageIndex, setPageIndex] = useState(1);
  const [pageSize] = useState(5);
  const [totalPages, setTotalPages] = useState(0);
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [selectedCommentId, setSelectedCommentId] = useState<number | null>(null);

  useEffect(() => {
    const fetchRecipe = async () => {
      if (recipeId) {
        try {
          const response = await getRecipeById(Number(recipeId));
          setRecipe(response);
          setLoading(false);
        } catch (error) {
          console.log(error);
          setLoading(false);
        }
      }
    };

    fetchRecipe();
    fetchComments();
  }, [recipeId, pageIndex, pageSize]);

  const fetchComments = async (resetPage = false) => {
    if (recipeId) {
      try {
        const response = await getCommentsForRecipe(Number(recipeId), resetPage ? 1 : pageIndex, pageSize);
        setComments(response.elements);
        setTotalPages(response.totalPages);
        if (resetPage) {
          setPageIndex(1);
        }
      } catch (error) {
        console.log(error);
      }
    }
  };

  useEffect(() => {
    if (window.location.hash === "#comments") {
      const intervalId = setInterval(() => {
        const element = document.getElementById("comments");
        if (element) {
          element.scrollIntoView({ behavior: "smooth" });
          clearInterval(intervalId);
        }
      }, 100);
      return () => clearInterval(intervalId);
    }
  }, []);

  const handleLikeToggle = async (recipeId: number, liked: boolean) => {
    try {
      if (liked) {
        const response = await deleteLike(recipeId);
        if (response && response.status === 204) {
          setRecipe((prevRecipe) =>
            prevRecipe ? {
              ...prevRecipe,
              likesCounter: prevRecipe.likesCounter - 1,
              currentUserLiked: false,
            } : prevRecipe
          );
        }
      } else {
        const response = await createLike({ recipeId });
        if (response && response.status === 201) {
          setRecipe((prevRecipe) =>
            prevRecipe ? {
              ...prevRecipe,
              likesCounter: prevRecipe.likesCounter + 1,
              currentUserLiked: true,
            } : prevRecipe
          );
        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handleNextPage = () => {
    if (pageIndex < totalPages) {
      setPageIndex((prevPageIndex) => prevPageIndex + 1);
    }
  };

  const handlePreviousPage = () => {
    if (pageIndex > 1) {
      setPageIndex((prevPageIndex) => prevPageIndex - 1);
    }
  };

  const handleOpenDialog = (commentId: number) => {
    setSelectedCommentId(commentId);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedCommentId(null);
  };

  const handleConfirmDelete = async () => {
    if (selectedCommentId !== null) {
      try {
        const response = await deleteComment(selectedCommentId);
        if (response && response.status === 204) {
          toast.success("Comment deleted successfully!");
          handleCloseDialog();
          await new Promise(resolve => setTimeout(resolve, 300));
          if (comments.length === 1 && pageIndex > 1) {
            setPageIndex((prevPageIndex) => prevPageIndex - 1);
          }
          fetchComments();
        }
      } catch (error) {
        console.log(error);
        toast.error("An error has occured! Please try again later.");
        handleCloseDialog();
      }
    }
  };

  const formik = useFormik<ICommentRequest>({
    initialValues: {
      recipeId: Number(recipeId),
      text: ""
    },

    validationSchema: Yup.object({
      text: Yup.string()
        .max(1000, "Comment must have a maximum of 1000 characters")
        .required("Comment is required")
    }),

    onSubmit: async (values, { resetForm }) => {
      try {
        const response = await createComment(values);
        if (response && response.status === 201) {
          toast.success("Comment added successfully!");
          await new Promise(resolve => setTimeout(resolve, 300));
          fetchComments(true);
          resetForm();
        }
      } catch (error) {
        console.log(error);
        toast.error("An error has occured! Please try again later.");
      }
    }
  });

  const handleCheckboxChange = (index: number) => {
    setCheckedIngredients(prevCheckedIngredients => {
      const newCheckedIngredients = new Set(prevCheckedIngredients);
      if (newCheckedIngredients.has(index)) {
        newCheckedIngredients.delete(index);
      } else {
        newCheckedIngredients.add(index);
      }
      return newCheckedIngredients;
    });
  };

  const isValidCategory = (category: number): category is 1 | 2 | 3 | 4 => {
    return [1, 2, 3, 4].includes(category);
  };
  

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!recipe) {
    return <BadRequest />;
  }

  return (
    <>
      <Helmet>
        <title>{recipe.name}</title>
      </Helmet>
      <Grid container className="container-recipe-page" sx={{ mt: 10 }}>
        <Paper elevation={0} className="paper">
          <Box display={'flex'} justifyContent={'flex-end'} mt={1} mb={3}>
            <PDFDownloadLink
              document={<RecipePdfDocument recipe={recipe} />}
              fileName={`${recipe.name}.pdf`}
            >
              {({ loading }) => (
                <Button variant="contained" disabled={loading} sx={{
                  mt: 2,
                  backgroundColor: theme.palette.primary.main,
                  '&:hover': {
                    backgroundColor: theme.palette.primary.main
                  },
                }}>
                  {loading ? 'Loading document...' : <PrintRoundedIcon sx={{ color: 'black' }} />}
                </Button>
              )}
            </PDFDownloadLink>
          </Box>
          <Box sx={{ width: '100%', textAlign: 'center', mb: 4 }}>
            <Typography gutterBottom variant="h4" component="div">
              {recipe.name}
            </Typography>
          </Box>
          <Box
            sx={{
              height: 400,
              display: 'flex',
              justifyContent: 'center',
              alignItems: 'center',
              border: recipe.pictureURL ? 'none' : '1px dashed grey',
              mb: 2,
              mt: 2,
              overflow: 'hidden',
              position: 'relative'
            }}
          >
            {recipe.pictureURL ? (
              <CardMedia
                component="img"
                sx={{ height: '100%', width: '100%', objectFit: 'contain' }}
                image={recipe.pictureURL}
                title="recipeImage"
              />
            ) : (
              <Typography variant="body2" color="text.secondary">
                No image available
              </Typography>
            )}
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', mb: 2}}>
            <Box sx={{ flexGrow: 1, height: '1px', backgroundColor: 'lightgray'}} />
            <Box display="flex" flexDirection="column" alignItems="center" mx={2}>
              {user?.role !== 0 && (
                <IconButton
                  aria-label="like"
                  size="small"
                  onClick={() => handleLikeToggle(recipe.id, recipe.currentUserLiked)}
                  color={recipe.currentUserLiked ? 'error' : 'default'}
                >
                  {recipe.currentUserLiked ? <FavoriteIcon /> : <FavoriteBorderRoundedIcon />}
                </IconButton>
              )}
              <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                {recipe.likesCounter} {recipe.likesCounter === 1 ? 'person likes this recipe' : 'people like this recipe'}
              </Typography>
            </Box>
            <Box sx={{ flexGrow: 1, height: '1px', backgroundColor: 'lightgray'}} />
          </Box>
          <Grid container my={2}>
            <Grid item xs={12} md={8} py={2}>
              <Grid container>
                <Grid item xs={12} py={1}>
                  <Typography variant="body2" color="text.secondary">
                    Posted by
                  </Typography>
                </Grid>
                <Grid item py={1}>
                  <CustomAvatar
                    firstName={recipe.user.firstName}
                    lastName={recipe.user.lastName}
                    pictureURL={recipe.user.pictureURL}
                  />
                </Grid>
                <Grid item px={2} py={1}>
                  {recipe.user.role === 1 &&
                    <Typography variant="body2">
                      {recipe.user.firstName} {recipe.user.lastName}
                    </Typography>
                  }
                  {recipe.user.role === 2 &&
                    <Typography variant="body2">
                      {recipe.user.firstName} {recipe.user.lastName} | <span style={{backgroundColor: '#BDF2D5', paddingLeft: '5px', paddingRight: '5px'}}>{getRoleText(recipe.user.role)}</span>
                    </Typography>
                  }
                  <Typography variant="body2" color="text.secondary">
                    {new Date(recipe.createdAt).toLocaleString('ro-RO').slice(0, -3)}
                  </Typography>
                </Grid>
              </Grid>
            </Grid>
            <Grid item py={4} xs={12} md={4}>
              <Grid container>
                <Grid item xs={12} py={1} display="flex" alignItems="center">
                  <AvTimerRoundedIcon sx={{ mr: 1 }} />
                  <Typography variant="body2" color="text.secondary">
                    Estimated Time: {recipe.estimatedTime}
                  </Typography>
                </Grid>
                <Grid item xs={12} display="flex" alignItems="center">
                  <SoupKitchenRoundedIcon sx={{ mr: 1 }} />
                  <Typography variant="body2" color="text.secondary">
                    Difficulty Level: {difficultyLevels.find((level) => level.value === recipe.difficulty)?.label}
                  </Typography>
                </Grid>
              </Grid>
            </Grid>
          </Grid>
          <Grid item my={3}>
            <Typography gutterBottom variant="h5" component="div">
              Ingredients
            </Typography>
            <List sx={{ listStyleType: 'none', pl: 0 }}>
              {recipe.recipeIngredients.map((ingredient, index) => (
                <ListItem key={index + 1} sx={{ py: 0.5 }}>
                  <FormControlLabel
                    control={
                      <Checkbox
                        checked={checkedIngredients.has(index)}
                        onChange={() => handleCheckboxChange(index)}
                      />
                    }
                    label={
                      <Typography sx={{
                        textDecoration: checkedIngredients.has(index) ? 'line-through' : 'none'
                      }}>
                        {ingredient.quantity} x {ingredient.quantityType !== 1 && `${getQuantityTypeText(ingredient.quantityType)} `}{ingredient.name}
                      </Typography>
                    }
                  />
                </ListItem>
              ))}
            </List>
          </Grid>
          <Grid item my={3}>
            <Typography gutterBottom variant="h5" component="div">
              Description
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {recipe.description}
            </Typography>
          </Grid>
          <Grid item my={3}>
            <Typography gutterBottom variant="h5" component="div">
              Keywords
            </Typography>
            <Box>
              {recipe.recipeTags.map((tag, index) => (
                isValidCategory(tag.type) && (
                  <TagChip key={index + 1} tagName={tag.name} category={tag.type}/>
                )
              ))}
            </Box>
          </Grid>
          <Grid item my={3} id="comments">
            <Typography gutterBottom variant="h5" component="div">
              Comments
            </Typography>
            {comments.length > 0 ? (
              <List sx={{ listStyleType: 'none', pl: 0 }}>
                {comments.map((comment, index) => (
                  <React.Fragment key={index}>
                    <Divider/>
                    <ListItem key={index} sx={{ display: 'flex', alignItems: 'flex-start', mb: 2}}>
                      <CustomAvatar firstName={comment.user.firstName} lastName={comment.user.lastName} pictureURL={comment.user.pictureURL} />
                      <Box sx={{ ml: 2, flexGrow: 1 }}>
                        <Typography variant="body2" color="text.primary">
                          {comment.user.firstName} {comment.user.lastName}
                          {comment.user.role === 2 && (
                            <>
                              {" | "}
                              <span style={{backgroundColor: '#BDF2D5', paddingLeft: '5px', paddingRight: '5px'}}>{getRoleText(comment.user.role)}</span>
                            </>
                          )}
                        </Typography>
                        <Typography variant="body2" color="text.primary">
                          {new Date(comment.createdAt).toLocaleString('ro-RO').slice(0, -3)}
                        </Typography>
                        <Typography variant="body2" color="text.secondary" sx={{ mt: 2 }}>
                          "{comment.text}"
                        </Typography>
                        {user && comment.user.id === user.id && (
                          <Typography
                            variant="body2"
                            color="red"
                            sx={{ cursor: 'pointer', mt: 1 }}
                            onClick={() => handleOpenDialog(comment.id)}
                          >
                            Delete comment
                          </Typography>
                        )}
                      </Box>
                    </ListItem>
                  </React.Fragment>
                ))}
              </List>
            ) : (
              <Typography variant="body2" color="text.secondary">
                No comments posted yet. Be the first one to add a comment!
              </Typography>
            )}
            <Grid container spacing={2} justifyContent="center">
              <Grid item>
                {(pageIndex !== 1) && 
                  <Button
                    variant="contained"
                    onClick={handlePreviousPage}
                  >
                    Previous
                  </Button>
                }
              </Grid>
              <Grid item>
                {(pageIndex < totalPages) &&
                  <Button
                    variant="contained"
                    onClick={handleNextPage}
                  >
                    Next
                  </Button>
                }
              </Grid>
            </Grid>
            {user?.role !== 0 && (
              <form onSubmit={formik.handleSubmit}>
                <TextField
                  fullWidth
                  multiline
                  rows={3}
                  variant="outlined"
                  placeholder="Write a comment..."
                  name="text"
                  value={formik.values.text}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  error={formik.touched.text && Boolean(formik.errors.text)}
                  helperText={formik.touched.text && formik.errors.text}
                  sx={{ mt: 2 }}
                />
                <Button type="submit" variant="contained" sx={{ mt: 2 }}>
                  Submit
                </Button>
              </form>
            )}
          </Grid>
        </Paper>
      </Grid>
      <ConfirmationDialog
        open={openDialog}
        title="Delete Confirmation"
        contentText="Are you sure you want to delete this comment? Warning: this action is permanent and cannot be undone."
        onClose={handleCloseDialog}
        onConfirm={handleConfirmDelete}
      />
    </>
  );
};

export default Recipe;
