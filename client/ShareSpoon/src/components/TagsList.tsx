import React from 'react';
import { List, ListItemButton, ListItemText, Typography, Box, Button } from '@mui/material';
import { ITag } from '../interfaces/TagInterface';
import { toast } from 'react-toastify';

interface TagsListProps {
  tags: ITag[];
  onTagClick: (id: number, name: string) => void;
  onNotFound: () => void;
  selectedTags: { id: number; name: string }[];
}

const TagsList: React.FC<TagsListProps> = ({ tags, onTagClick, onNotFound, selectedTags }) => {
  const handleTagClick = (id: number, name: string) => {
    if (selectedTags.some(tag => tag.id === id)) {
      toast.error("This tag is already selected!");
      return;
    }
    onTagClick(id, name);
  };

  return (
    <Box sx={{ width: '100%', maxHeight: 250, overflow: 'auto', mt: 2, pb: 2 }}>
      <Typography variant="body1">Results:</Typography>
      {tags.length === 0 ? (
        <Typography sx={{ mt: 2 }}>No results found!</Typography>
      ) : (
        <List>
          {tags.map((tag) => (
            <ListItemButton key={tag.id} onClick={() => handleTagClick(tag.id, tag.name)}>
              <ListItemText primary={tag.name} />
            </ListItemButton>
          ))}
        </List>
      )}
      <Button color="primary" variant="contained" onClick={onNotFound} sx={{ mt: 1 }}>Add new tag</Button>
    </Box>
  );
};

export default TagsList;
