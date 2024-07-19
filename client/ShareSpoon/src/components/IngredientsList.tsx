import React, { useState } from 'react';
import { List, ListItemButton, ListItemText, Typography, Box, Button } from '@mui/material';
import IngredientPopup from './IngredientPopup';
import { IIngredient } from '../interfaces/IngredientInterface';
import { toast } from 'react-toastify';

interface IngredientsListProps {
  ingredients: IIngredient[];
  onSaveIngredient: (id: number, name: string, quantity: number, quantityType: number) => void;
  onNotFound: () => void;
  selectedIngredients: { id: number; name: string; quantity: number; quantityType: number }[];
}

const IngredientsList: React.FC<IngredientsListProps> = ({ ingredients, onSaveIngredient, onNotFound, selectedIngredients }) => {
  const [selectedIngredient, setSelectedIngredient] = useState<IIngredient | null>(null);
  const [popupOpen, setPopupOpen] = useState<boolean>(false);

  const handleIngredientClick = (ingredient: IIngredient) => {
    if (selectedIngredients.some(selected => selected.id === ingredient.id)) {
      toast.error("This ingredient is already selected!");
      return;
    }
    setSelectedIngredient(ingredient);
    setPopupOpen(true);
  };

  const handlePopupClose = () => {
    setPopupOpen(false);
    setSelectedIngredient(null);
  };

  const handleSave = (quantity: number, quantityType: number) => {
    if (selectedIngredient) {
      onSaveIngredient(selectedIngredient.id, selectedIngredient.name, quantity, quantityType);
    }
  };

  return (
    <Box sx={{ width: '100%', maxHeight: 250, overflow: 'auto', mt: 2, pb: 2}}>
      <Typography variant="body1">Results:</Typography>
      {ingredients.length === 0 ? (
        <Typography sx={{ mt: 2 }}>No results found!</Typography>
      ) : (
        <List>
          {ingredients.map((ingredient) => (
            <ListItemButton key={ingredient.id} onClick={() => handleIngredientClick(ingredient)}>
              <ListItemText primary={ingredient.name} />
            </ListItemButton>
          ))}
        </List>
      )}
      {selectedIngredient && (
        <IngredientPopup open={popupOpen} onClose={handlePopupClose} onSave={handleSave} ingredientName={selectedIngredient.name} />
      )}
      <Button color="primary" variant="contained" onClick={onNotFound} sx={{ mt: 1 }}>Add new ingredient</Button>
    </Box>
  );
};

export default IngredientsList;
