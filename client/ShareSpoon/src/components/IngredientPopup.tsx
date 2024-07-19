import React, { useState } from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, Select, TextField } from '@mui/material';

interface IngredientPopupProps {
  open: boolean;
  onClose: () => void;
  onSave: (quantity: number, quantityType: number) => void;
  ingredientName: string;
}

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

const IngredientPopup: React.FC<IngredientPopupProps> = ({ open, onClose, onSave, ingredientName  }) => {
  const [quantity, setQuantity] = useState<number>(1);
  const [quantityType, setQuantityType] = useState<number>(1);

  const handleSave = () => {
    onSave(quantity, quantityType);
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">{ingredientName} Details</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          id="quantity"
          label="Quantity"
          type="number"
          fullWidth
          value={quantity}
          onChange={(e) => setQuantity(Number(e.target.value))}
          sx={{ mb: 2 }}
        />
        <Select
          labelId="quantity-type-label"
          id="quantity-type"
          value={quantityType}
          onChange={(e) => setQuantityType(Number(e.target.value))}
          fullWidth
        >
          {quantityTypes.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </Select>
      </DialogContent>
      <DialogActions>
        <Button
          onClick={onClose}
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
        <Button onClick={handleSave} variant="contained" color="primary" sx={{ mr: 2 }}>
          Save
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default IngredientPopup;
