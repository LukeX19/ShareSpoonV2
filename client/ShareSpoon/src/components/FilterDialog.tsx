import React, { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Checkbox, FormControlLabel, Typography, Box, FormGroup } from "@mui/material";
import { getFilterTags } from '../services/TagService';
import { ITag } from '../interfaces/TagInterface';

export interface FilterDialogHandle {
  clearFilters: () => void;
  resetCheckboxes: () => void;
}

interface FilterDialogProps {
  open: boolean;
  onClose: (filters: { promotedUsers: boolean, difficulties: number[], tagIds: number[] }) => void;
  onClear: () => void;
}

const FilterDialog = forwardRef<FilterDialogHandle, FilterDialogProps>(({ open, onClose, onClear }, ref) => {
  const [tags, setTags] = useState<ITag[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [selectedDifficulties, setSelectedDifficulties] = useState<number[]>([]);
  const [selectedTagIds, setSelectedTagIds] = useState<number[]>([]);
  const [promotedUsers, setPromotedUsers] = useState<boolean>(false);

  useEffect(() => {
    const fetchTags = async () => {
      setLoading(true);
      try {
        const response = await getFilterTags();
        if (response && response.status === 200) {
          setTags(response.data);
        }
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    };

    if (open) {
      fetchTags();
    }
  }, [open]);

  useImperativeHandle(ref, () => ({
    clearFilters: () => {
      setSelectedDifficulties([]);
      setSelectedTagIds([]);
      setPromotedUsers(false);
    },
    resetCheckboxes: () => {
      setSelectedDifficulties([]);
      setSelectedTagIds([]);
      setPromotedUsers(false);
    }
  }));

  const handleDifficultyChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = parseInt(event.target.value, 10);
    setSelectedDifficulties((prev) =>
      event.target.checked ? [...prev, value] : prev.filter((d) => d !== value)
    );
  };

  const handleTagChange = (event: React.ChangeEvent<HTMLInputElement>, tagId: number) => {
    setSelectedTagIds((prev) =>
      event.target.checked ? [...prev, tagId] : prev.filter((id) => id !== tagId)
    );
  };

  const handlePromotedUsersChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPromotedUsers(event.target.checked);
  };

  const renderTags = (type: number) => {
    return (
      <FormGroup>
        {tags.filter(tag => tag.type === type).map(tag => (
          <FormControlLabel
            key={tag.id}
            control={<Checkbox checked={selectedTagIds.includes(tag.id)} onChange={(e) => handleTagChange(e, tag.id)} />}
            label={tag.name} />
        ))}
      </FormGroup>
    );
  };

  const handleFilter = () => {
    onClose({ promotedUsers, difficulties: selectedDifficulties, tagIds: selectedTagIds });
  };

  const handleReset = () => {
    setSelectedDifficulties([]);
    setSelectedTagIds([]);
    setPromotedUsers(false);
    if (ref && 'current' in ref && ref.current) {
      ref.current.resetCheckboxes();
    }
  };

  return (
    <Dialog
      open={open}
      onClose={() => onClose({ promotedUsers: false, difficulties: [], tagIds: [] })}
      maxWidth="xs"
      fullWidth
      PaperProps={{ style: { maxHeight: '80vh', width: '50vh' } }}
    >
      <DialogTitle style={{ textAlign: 'center' }}>Filter Recipes</DialogTitle>
      <DialogContent dividers>
        <Box mb={2}>
          <Typography variant="h6">Curated Content</Typography>
          <FormGroup>
            <FormControlLabel control={<Checkbox checked={promotedUsers} onChange={handlePromotedUsersChange} />} label="Recipes by Promoted Users" />
          </FormGroup>
        </Box>
        <Box mb={2}>
          <Typography variant="h6">Difficulty</Typography>
          <FormGroup>
            <FormControlLabel control={<Checkbox value={1} checked={selectedDifficulties.includes(1)} onChange={handleDifficultyChange} />} label="Easy" />
            <FormControlLabel control={<Checkbox value={2} checked={selectedDifficulties.includes(2)} onChange={handleDifficultyChange} />} label="Medium" />
            <FormControlLabel control={<Checkbox value={3} checked={selectedDifficulties.includes(3)} onChange={handleDifficultyChange} />} label="Hard" />
          </FormGroup>
        </Box>
        <Box mb={2}>
          <Typography variant="h6">Course</Typography>
          <Box>
            {loading ? <Typography>Loading...</Typography> : renderTags(1)}
          </Box>
        </Box>
        <Box mb={2}>
          <Typography variant="h6">Dietary Preference</Typography>
          <Box>
            {loading ? <Typography>Loading...</Typography> : renderTags(4)}
          </Box>
        </Box>
      </DialogContent>
      <DialogActions style={{ justifyContent: 'space-between', marginTop: '10px', marginBottom: '10px', marginLeft: '15px', marginRight: '15px' }}>
        <Button
          onClick={handleReset}
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
          Reset
        </Button>
        <Button onClick={handleFilter} variant="contained" color="primary">
          Filter
        </Button>
      </DialogActions>
    </Dialog>
  );
});

export default FilterDialog;
