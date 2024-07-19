import React, { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import './CreateRecipe.css';
import { Box, Button, CardMedia, Dialog, DialogActions, DialogContent, DialogTitle, Grid, IconButton, MenuItem, Paper, Select, Slider, TextField, Typography } from '@mui/material';
import { Helmet } from 'react-helmet-async';
import DynamicSearchbar from '../../components/DynamicSearchbar';
import _ from 'lodash';
import authAxios from '../../config/authConfig';
import IngredientsList from '../../components/IngredientsList';
import TagsList from '../../components/TagsList';
import DeleteIcon from '@mui/icons-material/Delete';
import { IRecipeRequest } from '../../interfaces/RecipeInterface';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { createRecipe } from '../../services/RecipeService';
import { uploadFile } from '../../services/FileService';
import { toast } from 'react-toastify';
import { createIngredient } from '../../services/IngredientService';
import { createTag } from '../../services/TagService';

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

const tagCategories = [
  { value: 1, label: 'Course' },
  { value: 2, label: 'Cuisine' },
  { value: 3, label: 'Cooking Method' },
  { value: 4, label: 'Dietary Preference' },
];

const CreateRecipe: React.FC = () => {
  const navigate = useNavigate();
  const [hours, setHours] = useState<string>('00');
  const [minutes, setMinutes] = useState<string>('00');
  const [difficulty, setDifficulty] = useState<number>(1);
  const [instructions, setInstructions] = useState<string>('');
  const [image, setImage] = useState<string | null>(null);
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [searchIngredients, setSearchIngredients] = useState([]);
  const [searchTags, setSearchTags] = useState([]);
  const [ingredientSearchTouched, setIngredientSearchTouched] = useState<boolean>(false);
  const [tagSearchTouched, setTagSearchTouched] = useState<boolean>(false);
  const [selectedIngredients, setSelectedIngredients] = useState<{ id: number, name: string, quantity: number, quantityType: number }[]>([]);
  const [selectedTags, setSelectedTags] = useState<{ id: number, name: string }[]>([]);
  const [openIngredientDialog, setOpenIngredientDialog] = useState<boolean>(false);
  const [openTagDialog, setOpenTagDialog] = useState<boolean>(false);
  const [newIngredientName, setNewIngredientName] = useState<string>('');
  const [newTagName, setNewTagName] = useState<string>('');
  const [newTagCategory, setNewTagCategory] = useState<number>(1);

  const handleHoursChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    if (/^\d*$/.test(value) && Number(value) >= 0) {
      setHours(value.padStart(2, '0'));
      formik.setFieldValue('estimatedTime', `${value.padStart(2, '0')}:${minutes}:00`);
    }
  };

  const handleMinutesChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    if (/^\d*$/.test(value) && Number(value) >= 0 && Number(value) < 60) {
      setMinutes(value.padStart(2, '0'));
      formik.setFieldValue('estimatedTime', `${hours}:${value.padStart(2, '0')}:00`);
    }
  };

  const handleDifficultyChange = (event: Event, newValue: number | number[]) => {
    setDifficulty(newValue as number);
    formik.setFieldValue('difficulty', newValue);
  };

  const handleInstructionsChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInstructions(event.target.value);
    formik.setFieldValue('description', event.target.value);
  };

  const handleImageChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    try {
      if (event.target.files && event.target.files[0]) {
        const file = event.target.files[0];
        setImageFile(file);
        const reader = new FileReader();
        reader.onloadend = async () => {
          setImage(reader.result as string);
          const fileData = { file: file };
          const uploadResponse = await uploadFile(fileData);
          if (uploadResponse!.status === 201) {
            formik.setFieldValue('pictureURL', uploadResponse!.data.uri);
          }
        };
        reader.readAsDataURL(file);
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handleDeleteImage = () => {
    setImage(null);
    setImageFile(null);
    formik.setFieldValue('pictureURL', '');
  };

  const handleIngredientSearchChange = useCallback(
    _.debounce(async (query: string) => {
      setIngredientSearchTouched(true);
      setNewIngredientName(query);
      if (query) {
        try {
          const response = await authAxios.get(`/api/ingredients/search?name=${query}`);
          setSearchIngredients(response.data);
        } catch (error) {
          console.error('Error fetching ingredients:', error);
        }
      } else {
        setSearchIngredients([]);
      }
    }, 300),
    []
  );

  const handleTagSearchChange = useCallback(
    _.debounce(async (query: string) => {
      setTagSearchTouched(true);
      setNewTagName(query);
      if (query) {
        try {
          const response = await authAxios.get(`/api/tags/search?name=${query}`);
          setSearchTags(response.data);
        } catch (error) {
          console.error('Error fetching tags:', error);
        }
      } else {
        setSearchTags([]);
      }
    }, 300),
    []
  );

  const handleSaveIngredient = (id: number, name: string, quantity: number, quantityType: number) => {
    const updatedIngredients = [...selectedIngredients, { id, name, quantity, quantityType }];
    setSelectedIngredients(updatedIngredients);
    formik.setFieldValue('ingredients', updatedIngredients.map(ingredient => ({
      id: ingredient.id,
      quantity: ingredient.quantity,
      quantityType: ingredient.quantityType,
    })));
  };

  const handleDeleteIngredient = (index: number) => {
    const updatedIngredients = selectedIngredients.filter((_, i) => i !== index);
    setSelectedIngredients(updatedIngredients);
    formik.setFieldValue('ingredients', updatedIngredients.map(ingredient => ({
      id: ingredient.id,
      quantity: ingredient.quantity,
      quantityType: ingredient.quantityType,
    })));
  };

  const handleSaveTag = (id: number, name: string) => {
    const updatedTags = [...selectedTags, { id, name }];
    setSelectedTags(updatedTags);
    formik.setFieldValue('tags', updatedTags.map(tag => ({ id: tag.id })));
  };

  const handleDeleteTag = (index: number) => {
    const updatedTags = selectedTags.filter((_, i) => i !== index);
    setSelectedTags(updatedTags);
    formik.setFieldValue('tags', updatedTags.map(tag => ({ id: tag.id })));
  };

  const difficultyLevels = [
    { value: 1, label: 'Easy' },
    { value: 2, label: 'Medium' },
    { value: 3, label: 'Hard' },
  ];

  const getThumbColor = (value: number) => {
    switch (value) {
      case 1:
        return 'green';
      case 2:
        return 'yellow';
      case 3:
        return 'red';
      default:
        return 'green';
    }
  };

  const getTrackGradient = (value: number) => {
    switch (value) {
      case 1:
        return 'linear-gradient(to right, green, green 100%)';
      case 2:
        return 'linear-gradient(to right, green, yellow 50%, yellow 100%)';
      case 3:
        return 'linear-gradient(to right, green, yellow 50%, red 100%)';
      default:
        return 'linear-gradient(to right, green, green 100%)';
    }
  };

  const formik = useFormik<IRecipeRequest>({
    initialValues: {
      name: "",
      estimatedTime: "00:00:00",
      difficulty: 1,
      ingredients: [],
      description: "",
      tags: [],
      pictureURL: ""
    },

    validationSchema: Yup.object({
      name: Yup.string()
        .min(3, "Name must have at least 3 characters")
        .max(50, "Name must have maximum 50 characters")
        .required("Name is required"),
      estimatedTime: Yup.string()
        .test('is-not-zero', 'Estimated time must be greater than zero', function (value) {
          if (!value) return false;
          const [hours, minutes] = value.split(':').map(Number);
          return !(hours === 0 && minutes === 0);
        })
        .required("Estimated time is required"),
      difficulty: Yup.number()
        .min(1, "Invalid Difficulty Level")
        .max(3, "Invalid Difficulty Level")
        .required("Difficulty Level is required"),
      ingredients: Yup.array()
        .min(1, "At least one ingredient is required"),
      description: Yup.string()
        .min(3, "Recipe's description must have at least 3 characters")
        .max(3000, "Recipe's description must have maximum 3000 characters")
        .required("Recipe's description is required"),
      tags: Yup.array()
        .min(1, "At least one tag is required"),
      pictureURL: Yup.string()
        .required("Picture is required"),
    }),

    onSubmit: async (values) => {
      try {
        const estimatedTime = `${hours}:${minutes}:00`;
        const formData = { ...values, estimatedTime };
        const response = await createRecipe(formData);
        if (response!.status === 201) {
          toast.success("Recipe created successfully!");
          navigate("/");
        }
      } catch (error) {
        console.log(error);
        toast.error("An error has occured! Please try again later.");
      }
    }
  });

  const handleAddIngredient = async () => {
    if (newIngredientName.length >= 3 && newIngredientName.length <= 50) {
      try {
        const response = await createIngredient(newIngredientName);
        if (response && response.status === 201) {
          try {
            const response = await authAxios.get(`/api/ingredients/search?name=${newIngredientName}`);
            setSearchIngredients(response.data);
          } catch (error) {
            console.error('Error fetching ingredients:', error);
          }
          toast.success("Ingredient added successfully!");
          setOpenIngredientDialog(false);
        }
      } catch (error: any) {
        if (error.response.status === 409) {
          toast.error("There is already an ingredient with a similar name!");
        }
        else {
          toast.error("An error has occured! Please try again later.");
        }
      }
    }
  };

  const handleAddTag = async () => {
    if (newTagName.length >= 3 && newTagName.length <= 50) {
      try {
        const response = await createTag(newTagName, newTagCategory);
        if (response && response.status === 201) {
          try {
            const response = await authAxios.get(`/api/tags/search?name=${newTagName}`);
            setSearchTags(response.data);
          } catch (error) {
            console.error('Error fetching tags:', error);
          }
          toast.success("Tag added successfully!");
          setOpenTagDialog(false);
        }
      } catch (error: any) {
        if (error.response.status === 409) {
          toast.error("There is already a tag with a similar name!");
        }
        else {
          toast.error("An error has occured! Please try again later.");
        }
      }
    }
  };

  return (
    <>
      <Helmet>
        <title>New Recipe</title>
      </Helmet>
      <form onSubmit={formik.handleSubmit}>
        <Grid container className="container-create-recipe" sx={{ mt: 10, justifyContent: 'center' }}>
          <Paper elevation={0} className="paper">
            <Typography sx={{ mt: 4 }}>THUMBNAIL</Typography>
            <Box
              sx={{
                height: 400,
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                border: image ? 'none' : '1px dashed grey',
                mb: 2,
                mt: 2,
                overflow: 'hidden',
                position: 'relative',
                '&:hover .overlay': {
                  opacity: 0.5,
                },
                '&:hover .delete-button': {
                  opacity: 1,
                },
              }}
            >
              {image ? (
                <>
                  <CardMedia
                    component="img"
                    sx={{ height: '100%', width: '100%', objectFit: 'contain' }}
                    image={image}
                    title="recipeImage"
                  />
                  <Box
                    className="overlay"
                    sx={{
                      position: 'absolute',
                      top: 0,
                      left: 0,
                      width: '100%',
                      height: '100%',
                      backgroundColor: 'rgba(255, 255, 255)',
                      opacity: 0,
                      transition: 'opacity 0.3s',
                    }}
                  />
                  <Button
                    className="delete-button"
                    variant="contained"
                    color="error"
                    sx={{
                      position: 'absolute',
                      top: '50%',
                      left: '50%',
                      transform: 'translate(-50%, -50%)',
                      opacity: 0,
                      transition: 'opacity 0.3s',
                    }}
                    onClick={handleDeleteImage}
                  >
                    Delete Picture
                  </Button>
                </>
              ) : (
                <Button variant="contained" component="label">
                  Choose Recipe Image
                  <input type="file" hidden accept="image/*" onChange={handleImageChange} />
                </Button>
              )}
            </Box>
            {formik.touched.pictureURL && formik.errors.pictureURL && (
              <Typography
                variant="body2"
                sx={{ color: 'red', mt: 1, textAlign: 'center' }}
              >
                {formik.errors.pictureURL}
              </Typography>
            )}
            <Grid container my={2}>
              <Grid item xs={12} py={2}>
                <Typography>NAME</Typography>
                <Grid container py={3}>
                  <TextField
                    variant="standard"
                    placeholder="Type your eye-catching title here..."
                    fullWidth
                    name="name"
                    value={formik.values.name}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    sx={{ pb: 3 }}
                    error={formik.touched.name && Boolean(formik.errors.name)}
                  />
                  <Typography color="grey">
                    <i>Note: We recommend you to brainstorm, in order to find a delightful and captivating name for your recipe. Example: "Heavenly Herb Chicken" or "Sizzling Summer Salad".</i>
                  </Typography>
                  {formik.touched.name && formik.errors.name && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                    >
                      {formik.errors.name}
                    </Typography>
                  )}
                </Grid>
                <Typography sx={{ mt: 4 }}>GENERAL INFORMATION</Typography>
                <Grid container py={3} spacing={2}>
                  <Grid item xs={12} sm={5}>
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                      <TextField
                        label="Hours"
                        variant="standard"
                        type="number"
                        name="hours"
                        value={hours}
                        onChange={handleHoursChange}
                        sx={{ mr: 2, width: '40%' }}
                      />
                      <TextField
                        name="minutes"
                        label="Minutes"
                        type="number"
                        variant="standard"
                        value={minutes}
                        onChange={handleMinutesChange}
                        sx={{ width: '40%' }}
                      />
                    </Box>
                    {formik.touched.estimatedTime && formik.errors.estimatedTime && (
                      <Typography
                        variant="body2"
                        sx={{ color: 'red', mt: 1 }}
                      >
                        {formik.errors.estimatedTime}
                      </Typography>
                    )}
                  </Grid>
                  <Grid item xs={12} sm={7}>
                    <Typography gutterBottom>Difficulty</Typography>
                    <Slider
                      className="gradient-slider"
                      value={difficulty}
                      onChange={handleDifficultyChange}
                      step={1}
                      marks={difficultyLevels}
                      min={1}
                      max={3}
                      valueLabelDisplay="auto"
                      valueLabelFormat={(value) => difficultyLevels.find(mark => mark.value === value)?.label}
                      sx={{
                        width: '100%',
                        '& .MuiSlider-thumb': {
                          backgroundColor: getThumbColor(difficulty),
                          border: `2px solid ${getThumbColor(difficulty)}`,
                        },
                        '& .MuiSlider-track': {
                          background: getTrackGradient(difficulty),
                          border: 'none'
                        }
                      }}
                    />
                    {formik.touched.difficulty && formik.errors.difficulty && (
                      <Typography
                        variant="body2"
                        sx={{ color: 'red', mt: 1, ml: -7, textAlign: 'center' }}
                      >
                        {formik.errors.difficulty}
                      </Typography>
                    )}
                  </Grid>
                </Grid>
                <Typography sx={{ mt: 4 }}>SELECT INGREDIENTS</Typography>
                <Box py={3}>
                  <DynamicSearchbar onSearchChange={handleIngredientSearchChange} />
                  {(ingredientSearchTouched && newIngredientName !== '') &&
                    <IngredientsList ingredients={searchIngredients} onSaveIngredient={handleSaveIngredient} onNotFound={() => setOpenIngredientDialog(true)} selectedIngredients={selectedIngredients} />
                  }
                </Box>
                <Typography sx={{ mt: 4 }}>SELECTED INGREDIENTS</Typography>
                <Grid container direction="column" spacing={1} py={3}>
                  {selectedIngredients.length === 0 && <Typography sx={{ mt: 1, ml: 4 }}>No ingredient was selected</Typography>}
                  {selectedIngredients.length !== 0 && selectedIngredients.map((ingredient, index) => (
                    <Grid item key={index}>
                      <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <IconButton onClick={() => handleDeleteIngredient(index)} sx={{ mr: 1, color: 'red' }}>
                          <DeleteIcon />
                        </IconButton>
                        <Typography>
                          {ingredient.quantity} x {ingredient.quantityType !== 1 && `${quantityTypes.find(type => type.value === ingredient.quantityType)?.label}`} {ingredient.name}
                        </Typography>
                      </Box>
                    </Grid>
                  ))}
                  {formik.touched.ingredients && formik.errors.ingredients && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 2, ml: 4 }}
                    >
                      At least one ingredient is required
                    </Typography>
                  )}
                </Grid>
                <Typography sx={{ mt: 4 }}>INSTRUCTIONS</Typography>
                <Grid container py={3}>
                  <TextField
                    name="instructions"
                    multiline
                    rows={10}
                    variant="outlined"
                    fullWidth
                    value={instructions}
                    onChange={handleInstructionsChange}
                    inputProps={{ maxLength: 3000 }}
                    placeholder="Describe the preparation process of your recipe here..."
                  />
                  {formik.touched.description && formik.errors.description && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                    >
                      {formik.errors.description}
                    </Typography>
                  )}
                </Grid>
                <Typography sx={{ mt: 4 }}>SELECT TAGS</Typography>
                <Box py={3}>
                  <DynamicSearchbar onSearchChange={handleTagSearchChange} />
                  {(tagSearchTouched && newTagName !== '') &&
                    <TagsList tags={searchTags} onTagClick={(id, name) => handleSaveTag(id, name)} onNotFound={() => setOpenTagDialog(true)} selectedTags={selectedTags} />
                  }
                </Box>
                <Typography sx={{ mt: 4 }}>SELECTED TAGS</Typography>
                <Grid container direction="column" spacing={1} py={3}>
                  {selectedTags.length === 0 && <Typography sx={{ mt: 1, ml: 4 }}>No tag was selected</Typography>}
                  {selectedTags.length !== 0 && selectedTags.map((tag, index) => (
                    <Grid item key={index}>
                      <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <IconButton onClick={() => handleDeleteTag(index)} sx={{ mr: 1, color: 'red' }}>
                          <DeleteIcon />
                        </IconButton>
                        <Typography>{tag.name}</Typography>
                      </Box>
                    </Grid>
                  ))}
                  {formik.touched.tags && formik.errors.tags && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 2, ml: 4 }}
                    >
                      At least one tag is required
                    </Typography>
                  )}
                </Grid>
                <Grid container mt={2} justifyContent="space-between">
                  <Button
                    onClick={() => navigate('/')}
                    variant="outlined"
                    sx={{
                      color: 'black',
                      borderColor: 'black',
                      '&:hover': {
                        borderColor: 'black',
                        backgroundColor: 'rgba(0, 0, 0, 0.04)'
                      },
                    }}
                  >
                    Cancel
                  </Button>
                  <Button variant="contained" color="primary" type="submit">
                    Submit
                  </Button>
                </Grid>
              </Grid>
            </Grid>
          </Paper>
        </Grid>
      </form>
      <Dialog open={openIngredientDialog} onClose={() => setOpenIngredientDialog(false)} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">Add Ingredient</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            id="ingredientName"
            label="Ingredient Name"
            type="text"
            fullWidth
            value={newIngredientName}
            onChange={(e) => setNewIngredientName(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => setOpenIngredientDialog(false)}
            variant="outlined"
            sx={{
              color: 'black',
              borderColor: 'black',
              '&:hover': {
                borderColor: 'black',
                backgroundColor: 'rgba(0, 0, 0, 0.04)'
              },
            }}
          >
            Cancel
          </Button>
          <Button onClick={handleAddIngredient} variant="contained" color="primary" sx={{ mr: 2 }} disabled={newIngredientName.length < 3 || newIngredientName.length > 50}>
            Add
          </Button>
        </DialogActions>
      </Dialog>
      <Dialog open={openTagDialog} onClose={() => setOpenTagDialog(false)} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">Add Tag</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            id="tagName"
            label="Tag Name"
            type="text"
            fullWidth
            value={newTagName}
            onChange={(e) => setNewTagName(e.target.value)}
            sx={{ mb: 2 }}
          />
          <Select
            labelId="tag-category-label"
            id="tag-category"
            value={newTagCategory}
            onChange={(e) => setNewTagCategory(Number(e.target.value))}
            fullWidth
          >
            {tagCategories.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => setOpenTagDialog(false)}
            variant="outlined"
            sx={{
              color: 'black',
              borderColor: 'black',
              '&:hover': {
                borderColor: 'black',
                backgroundColor: 'rgba(0, 0, 0, 0.04)'
              },
            }}
          >
            Cancel
          </Button>
          <Button onClick={handleAddTag} variant="contained" color="primary" sx={{ mr: 2 }} disabled={newTagName.length < 3 || newTagName.length > 50}>
            Add
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default CreateRecipe;
