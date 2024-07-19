import React from 'react';
import { Chip } from '@mui/material';
import { styled } from '@mui/material/styles';

interface TagChipProps {
  tagName: string;
  category: 1 | 2 | 3 | 4;
}

const categoryColors = {
  //Red
  1: {
    color: '#ff4081',
    borderColor: '#ff4081',
  },
  //Purple
  2: {
    color: '#AA14F0',
    borderColor: '#AA14F0',
  },
  //Orage
  3: {
    color: '#ff9800',
    borderColor: '#ff9800',
  },
  //Blue
  4: {
    color: '#2196f3',
    borderColor: '#2196f3',
  },
};

const StyledChip = styled(Chip, {
  shouldForwardProp: (prop) => prop !== 'category',
})<{ category: keyof typeof categoryColors }>(({ category }) => ({
  color: categoryColors[category].color,
  borderColor: categoryColors[category].borderColor,
}));

const TagChip: React.FC<TagChipProps> = ({ tagName, category }) => {
  return <StyledChip label={`# ${tagName}`} variant="outlined" category={category} sx={{mr: 1}}/>;
};

export default TagChip;
