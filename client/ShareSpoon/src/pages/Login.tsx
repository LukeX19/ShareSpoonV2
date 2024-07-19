import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Container, Typography, TextField, Button, Link, InputAdornment, IconButton } from "@mui/material";
import { Helmet } from 'react-helmet-async';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { ILogin } from '../interfaces/LoginInterface';
import { login } from '../services/LoginService';
import VisibilityRoundedIcon from '@mui/icons-material/VisibilityRounded';
import VisibilityOffRoundedIcon from '@mui/icons-material/VisibilityOffRounded';
import { toast } from 'react-toastify';

const Login: React.FC = () => {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  const handleMouseUpPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  const formik = useFormik<ILogin>({
    initialValues: {
      email: "",
      password: ""
    },

    validationSchema: Yup.object({
      email: Yup.string()
        .email("Invalid email address")
        .required("Email is required"),
      password: Yup.string()
        .min(10, "Password must have at least 10 characters")
        .required("Password is required")
    }),

    onSubmit: async (values) => {
      try {
        const response = await login(values);
        if(response && response.status === 200) {
          if(response.data.role === 0) {
            navigate("/admin");
          }
          else {
            navigate("/");
          }
        }
      } catch (error: any) {
        console.log(error);
        if (error.response.status === 401) {
          toast.error("The provided credentials are invalid.");
        }
        else {
          toast.error("An error has occured! Please try again later.");
        }
      }
    }
  });

  return (
    <>
      <Helmet>
        <title>Login</title>
      </Helmet>
      <form onSubmit={formik.handleSubmit}>
        <Container
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: '100vh',
            width: '500px'
          }}
        >
          <Box
            sx={{
              width: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              padding: 4,
              borderRadius: 2,
            }}
          >
            <Typography variant="h4" component="h1" gutterBottom mb={3}>
              Login
            </Typography>
            <TextField
              label="Email"
              variant="outlined"
              fullWidth
              margin="normal"
              name="email"
              value={formik.values.email}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.email && Boolean(formik.errors.email)}
            />
            {formik.touched.email && formik.errors.email && (
              <Typography
                variant="body2"
                sx={{ color: 'red', mt: 1, textAlign: 'center' }}
              >
                {formik.errors.email}
              </Typography>
            )}
            <TextField
              label="Password"
              variant="outlined"
              type={showPassword ? "text" : "password"}
              fullWidth
              margin="normal"
              name="password"
              value={formik.values.password}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.password && Boolean(formik.errors.password)}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPassword}
                      onMouseDown={handleMouseDownPassword}
                      onMouseUp={handleMouseUpPassword}
                    >
                      {showPassword ? <VisibilityOffRoundedIcon /> : <VisibilityRoundedIcon />}
                    </IconButton>
                  </InputAdornment>
                )
              }}
            />
            {formik.touched.password && formik.errors.password && (
              <Typography
                variant="body2"
                sx={{ color: 'red', mt: 1, textAlign: 'center' }}
              >
                {formik.errors.password}
              </Typography>
            )}
            <Button
              variant="contained"
              color="primary"
              sx={{ mt: 2, width: "100%"}}
              type="submit"
            >
              Login
            </Button>
            <Typography variant="body1" sx={{ mt: 5}}>
              Don't have an account?{' '}
              <Link
                component="button"
                variant="body1"
                onClick={() => navigate('/register')}
                sx={{ textDecoration: 'none' }}
              >
                Sign up now
              </Link>
            </Typography>
          </Box>
        </Container>
      </form>
    </>
  );
}

export default Login;