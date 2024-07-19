import React, { forwardRef } from 'react';
import { InputBase, alpha, styled, Button, Box } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';

const SearchContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  width: '100%',
  [theme.breakpoints.up('sm')]: {
    width: '56.5%',
  },
}));

const Search = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  backgroundColor: alpha(theme.palette.common.black, 0.05),
  '&:hover': {
    backgroundColor: alpha(theme.palette.common.black, 0.07),
  },
  flexGrow: 1,
  borderTopLeftRadius: theme.shape.borderRadius * 5,
  borderBottomLeftRadius: theme.shape.borderRadius * 5,
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  color: 'black',
  width: '100%',
  '& .MuiInputBase-input': {
    padding: theme.spacing(1, 1, 1, 0),
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create('width'),
    width: '100%',
  },
  borderTopLeftRadius: theme.shape.borderRadius * 5,
  borderBottomLeftRadius: theme.shape.borderRadius * 5,
  borderTopRightRadius: 0,
  borderBottomRightRadius: 0,
}));

const SearchButton = styled(Button)(({ theme }) => ({
  borderTopLeftRadius: 0,
  borderBottomLeftRadius: 0,
  borderTopRightRadius: theme.shape.borderRadius * 5,
  borderBottomRightRadius: theme.shape.borderRadius * 5,
  height: '41px',
  width: '75px',
  whiteSpace: 'nowrap',
  boxShadow: 'none'
}));

interface StaticSearchbarProps {
  onKeyDown: (event: React.KeyboardEvent<HTMLInputElement>) => void;
  onSearchClick: () => void;
}

const StaticSearchbar = forwardRef<HTMLInputElement, StaticSearchbarProps>(({ onKeyDown, onSearchClick }, ref) => {
  return (
    <SearchContainer>
      <Search sx={{border: '1px solid #B9B4C7'}}>
        <StyledInputBase
          placeholder="Search recipes…"
          inputProps={{ 'aria-label': 'search' }}
          onKeyDown={onKeyDown}
          inputRef={ref}
        />
      </Search>
      <SearchButton variant="contained" sx={{backgroundColor: '#DBDBDB', border: '1px solid #B9B4C7', ":hover": {backgroundColor: '#C9C9C9', boxShadow: 'none'}}} onClick={onSearchClick}>
        <SearchIcon />
      </SearchButton>
    </SearchContainer>
  );
});

export default StaticSearchbar;
