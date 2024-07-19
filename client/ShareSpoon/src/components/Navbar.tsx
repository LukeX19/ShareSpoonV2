import React, { useState } from 'react';
import { AppBar, Box, Button, Container, IconButton, ListItemIcon, Menu, MenuItem, Toolbar, Tooltip, Typography, useTheme } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { useUser } from '../hooks/useUser';
import { useNavigate } from 'react-router-dom';
import CustomAvatar from './CustomAvatar';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import LogoutIcon from '@mui/icons-material/Logout';

const Navbar: React.FC = () => {
  const { user } = useUser();
  const navigate = useNavigate();
  const theme = useTheme();
  const [anchorElNav, setAnchorElNav] = useState<null | HTMLElement>(null);
  const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = (setting?: string) => {
    setAnchorElUser(null);
    if (setting === "Profile") {
      navigate("/profile");
    }
    else if (setting === "Admin Panel") {

      navigate("/admin");

    }
    else if (setting === "Logout") {
      localStorage.removeItem("token");
      navigate("/login");
    }
  };

  const handlePageNavigation = (page: { label: string; path: string }) => {
    handleCloseNavMenu();
    navigate(page.path);
  };

  const pages = [
    { label: 'Discover Recipes', path: '/' },
    ...(user?.role !== 0 ? [{ label: 'Create & Share', path: '/recipe/create' }] : [])
  ];

  const settings = [
    ...(user?.role !== 0 ? ["Profile"] : []),
    ...(user?.role === 0 ? ["Admin Panel"] : []),
    "Logout"
  ];

  return (
    <AppBar position="fixed" sx={{ backgroundColor: theme.palette.secondary.main }}>
      <Container maxWidth={false}>
        <Toolbar disableGutters sx={{ height: 80 }}>
          <Typography
            noWrap
            sx={{
              ml: 5,
              mr: 1,
              display: { xs: 'none', md: 'flex' },
              letterSpacing: '.1rem',
              color: 'white',
              textDecoration: 'none',
              fontFamily: '"M PLUS 2", sans-serif',
              fontSize: '24px',
            }}
          >
            ShareSpoon
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon sx={{ color: 'white' }} />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {pages.map((page) => (
                <MenuItem key={page.label} onClick={() => handlePageNavigation(page)}>
                  <Typography textAlign="center">{page.label}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
          <Typography
            noWrap
            sx={{
              mr: 2.5,
              display: { xs: 'flex', md: 'none' },
              flexGrow: 1,
              letterSpacing: '.1rem',
              color: 'white',
              textDecoration: 'none',
              justifyContent: 'center',
              textAlign: 'center',
              fontFamily: '"M PLUS 2", sans-serif',
              fontSize: '24px'
            }}
          >
            ShareSpoon
          </Typography>
          <Box sx={{ flexGrow: 1 }} />
          <Box sx={{ display: { xs: 'none', md: 'flex' }, alignItems: 'center' }}>
            {pages.map((page) => (
              <Button
                key={page.label}
                onClick={() => handlePageNavigation(page)}
                sx={{ mx: 1, my: 1, color: 'white', display: 'block' }}
              >
                {page.label}
              </Button>
            ))}
          </Box>
          <Box sx={{ flexGrow: 0, ml: 2, mr: { md: 5 } }}>
            <Tooltip title="Open settings">
              <IconButton onClick={handleOpenUserMenu}>
                <CustomAvatar
                  firstName={user?.firstName || "First"}
                  lastName={user?.lastName || "Last"}
                  pictureURL={user?.pictureURL || ""}
                />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={() => handleCloseUserMenu()}
            >
              {settings.map((setting) => (
                <MenuItem key={setting} onClick={() => handleCloseUserMenu(setting)}>
                  <ListItemIcon>
                    {setting === 'Profile' ? <AccountCircleIcon /> : setting === 'Admin Panel' ? <AdminPanelSettingsIcon /> : <LogoutIcon />}
                  </ListItemIcon>
                  {setting}
                </MenuItem>
              ))}
            </Menu>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default Navbar;
