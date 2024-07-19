import React, { useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { Box, Container, Typography, Button, Avatar, Grid, TextField, LinearProgress, Stepper, Step, StepLabel, useTheme, InputAdornment, IconButton } from "@mui/material";
import { Helmet } from "react-helmet-async";
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs, { Dayjs } from "dayjs";
import AddIcon from "@mui/icons-material/Add";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityRoundedIcon from '@mui/icons-material/VisibilityRounded';
import VisibilityOffRoundedIcon from '@mui/icons-material/VisibilityOffRounded';
import { IRegisterStepOne, IRegisterStepTwo } from "../interfaces/RegisterInterface";
import { uploadFile } from "../services/FileService";
import { register } from "../services/RegisterService";
import { toast } from "react-toastify";

const steps = ['Personal Information', 'Account Information'];

const Register: React.FC = () => {
  const navigate = useNavigate();
  const theme = useTheme();

  const [birthday, setBirthday] = useState<Dayjs | null>(null);
  const [password, setPassword] = useState("");
  const [stepOneData, setStepOneData] = useState<IRegisterStepOne | null>(null);

  const [selectedImage, setSelectedImage] = useState<string | ArrayBuffer | null>(null);
  const [imageFile, setImageFile] = useState<File | null>(null);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleClickShowConfirmPassword = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  const handleMouseUpPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
  };

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      setImageFile(file);
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        setSelectedImage(e.target?.result as string);
      };
      reader.readAsDataURL(file);
      if (fileInputRef.current) {
        fileInputRef.current.value = '';
      }
    }
  };

  const handleImageDelete = () => {
    setSelectedImage(null);
    setImageFile(null);
  };

  const handleDateChange = (newValue: Dayjs | null) => {
    setBirthday(newValue);
    formikStepOne.setFieldValue("birthday", newValue?.format("YYYY-MM-DD") || "");
  };

  const disableFutureDates = (date: Dayjs) => {
    return date.isAfter(dayjs());
  };

  const minLength = 12;
  const getHue = (length: number) => Math.min(length * 10, 120);
  const getStrengthText = (length: number) => {
    if (length < 3) return "Very weak";
    if (length >= 3 && length < 6) return "Weak";
    if (length >= 6 && length < 10) return "Strong";
    if (length >= 10) return "Very strong";
  };

  const formikStepOne = useFormik<IRegisterStepOne>({
    initialValues: {
      firstName: "",
      lastName: "",
      birthday: "",
      role: 1
    },

    validationSchema: Yup.object({
      firstName: Yup.string()
        .min(3, "First name must have at least 3 characters")
        .max(50, "First name must have a maximum of 50 characters")
        .matches(/^[a-z ,.'-]+$/i, "First name must contain only english alphabet letters, spaces, and - , . ' characters")
        .required("First name is required"),
      lastName: Yup.string()
        .min(3, "Last name must have at least 3 characters")
        .max(50, "Last name must have a maximum of 50 characters")
        .matches(/^[a-z ,.'-]+$/i, "Last name must contain only english alphabet letters, spaces, and - , . ' characters")
        .required("Last name is required"),
      birthday: Yup.string()
        .required("Birthday is required")
        .test("valid-age", "You must be at least 14 years old", function (value) {
          return dayjs().diff(dayjs(value), 'year') >= 14;
        })
    }),

    onSubmit: async (values) => {
      try {
        setStepOneData(values);
        handleNext();
      } catch (error) {
        console.log(error);
      }
    }
  });

  const formikStepTwo = useFormik<IRegisterStepTwo>({
    initialValues: {
      email: "",
      password: "",
      confirmPassword: "",
      pictureURL: "default"
    },

    validationSchema: Yup.object({
      email: Yup.string()
        .email("Invalid email address")
        .required("Email is required"),
      password: Yup.string()
        .min(10, "Password must have at least 10 characters")
        .required("Password is required"),
      confirmPassword: Yup.string()
        .oneOf([Yup.ref("password"), undefined], "Passwords must match")
        .required("Confirm Password is required")
    }),

    onSubmit: async (values) => {
      try {
        if (stepOneData) {
          const { confirmPassword, ...stepTwoData } = values;
          const completeData = { ...stepOneData, ...stepTwoData };

          if (imageFile) {
            const fileData = { file: imageFile };
            const uploadResponse = await uploadFile(fileData);
            if (uploadResponse!.status === 201) {
              completeData.pictureURL = uploadResponse!.data.uri;
            }
          }
          const response = await register(completeData);
          if (response && response.status === 200) {
            handleNext();
          }
        }
      } catch (error: any) {
        console.log(error);
        if (error.response.status === 409) {
          toast.error("The provided email is already associated with an account!");
        }
        else {
          toast.error("An error has occured! Please try again later.");
        }
      }
    }
  });

  const [activeStep, setActiveStep] = useState(0);

  const handleNext = () => {
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  return (
    <>
      <Helmet>
        <title>Register</title>
      </Helmet>
      <Container
        maxWidth={"sm"}
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          minHeight: "100vh"
        }}
      >
        <Box
          sx={{
            width: "100%",
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            padding: 4,
            borderRadius: 2
          }}
        >
          <Typography variant="h4" component="h1" gutterBottom mb={5}>
            Register
          </Typography>
          <Box sx={{ width: '90%', mb: 4 }}>
            <Stepper activeStep={activeStep} alternativeLabel>
              {steps.map((label) => (
                <Step key={label}>
                  <StepLabel>{label}</StepLabel>
                </Step>
              ))}
            </Stepper>
          </Box>
          {activeStep === 0 && (
            <form onSubmit={formikStepOne.handleSubmit}>
              <Grid container spacing={2} px={7}>
                <Grid item xs={12}>
                  <TextField
                    label="First Name"
                    variant="outlined"
                    fullWidth
                    name="firstName"
                    value={formikStepOne.values.firstName}
                    onChange={formikStepOne.handleChange}
                    onBlur={formikStepOne.handleBlur}
                    error={formikStepOne.touched.firstName && Boolean(formikStepOne.errors.firstName)}
                  />
                  {formikStepOne.touched.firstName && formikStepOne.errors.firstName && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                    >
                      {formikStepOne.errors.firstName}
                    </Typography>
                  )}
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    label="Last Name"
                    variant="outlined"
                    fullWidth
                    name="lastName"
                    value={formikStepOne.values.lastName}
                    onChange={formikStepOne.handleChange}
                    onBlur={formikStepOne.handleBlur}
                    error={formikStepOne.touched.lastName && Boolean(formikStepOne.errors.lastName)}
                  />
                  {formikStepOne.touched.lastName && formikStepOne.errors.lastName && (
                    <Typography
                      variant="body2"
                      sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                    >
                      {formikStepOne.errors.lastName}
                    </Typography>
                  )}
                </Grid>
                <Grid item xs={12}>
                  <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <DatePicker
                      format="DD-MM-YYYY"
                      label="Birthday"
                      value={birthday}
                      onChange={handleDateChange}
                      shouldDisableDate={disableFutureDates}
                      disableFuture
                      slotProps={{
                        textField: {
                          variant: "outlined",
                          error: formikStepOne.touched.birthday && Boolean(formikStepOne.errors.birthday)
                        }
                      }}
                      sx={{ width: '100%' }}
                    />
                    {formikStepOne.touched.birthday && formikStepOne.errors.birthday && (
                      <Typography
                        variant="body2"
                        sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                      >
                        {formikStepOne.errors.birthday}
                      </Typography>
                    )}
                  </LocalizationProvider>
                </Grid>
              </Grid>
              <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2, px: 7 }}>
                <Button
                  variant="contained"
                  color="primary"
                  type="submit"
                  sx={{ mt: 2 }}
                >
                  Next
                </Button>
              </Box>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                <Typography>Already a member? </Typography>{' '}
                <Typography onClick={() => navigate('/login')} sx={{ pl: 0.5, cursor: 'pointer', color: theme.palette.primary.main }}> Log in</Typography>
              </Grid>
            </form>
          )}
          {activeStep === 1 && (
            <Box mt={2}>
              <form onSubmit={formikStepTwo.handleSubmit}>
                <Grid container spacing={6}>
                  <Grid item xs={12} sm={4} sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                    <Box position="relative">
                      <Avatar
                        src={selectedImage as string}
                        alt="Selected"
                        sx={{
                          width: 150,
                          height: 150,
                          borderRadius: '50%',
                          boxShadow: 1,
                          backgroundColor: selectedImage ? 'transparent' : '#B6BBC4',
                        }}
                      >
                        {!selectedImage &&
                          <AccountCircleIcon sx={{ fontSize: 150, color: 'white' }} />
                        }
                      </Avatar>
                      <input
                        accept="image/*"
                        type="file"
                        onChange={handleImageChange}
                        style={{ display: 'none' }}
                        ref={fileInputRef}
                        id="upload-photo"
                      />
                      <label htmlFor="upload-photo">
                        <Box
                          sx={{
                            position: 'absolute',
                            bottom: -10,
                            right: -10,
                            backgroundColor: 'white',
                            borderRadius: '50%',
                            width: 40,
                            height: 40,
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            boxShadow: 3,
                            cursor: 'pointer',
                          }}
                        >
                          <AddIcon color="primary" />
                        </Box>
                      </label>
                      {selectedImage && (
                        <Box
                          onClick={handleImageDelete}
                          sx={{
                            position: 'absolute',
                            bottom: -10,
                            left: -10,
                            backgroundColor: 'white',
                            borderRadius: '50%',
                            width: 40,
                            height: 40,
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            boxShadow: 3,
                            cursor: 'pointer',
                          }}
                        >
                          <DeleteIcon color="error" />
                        </Box>
                      )}
                    </Box>
                  </Grid>
                  <Grid item xs={12} sm={8}>
                    <Grid container spacing={2}>
                      <Grid item xs={12}>
                        <TextField
                          label="Email"
                          variant="outlined"
                          fullWidth
                          name="email"
                          value={formikStepTwo.values.email}
                          onChange={formikStepTwo.handleChange}
                          onBlur={formikStepTwo.handleBlur}
                          error={formikStepTwo.touched.email && Boolean(formikStepTwo.errors.email)}
                        />
                        {formikStepTwo.touched.email && formikStepTwo.errors.email && (
                          <Typography
                            variant="body2"
                            sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                          >
                            {formikStepTwo.errors.email}
                          </Typography>
                        )}
                      </Grid>
                      <Grid item xs={12}>
                        <Box sx={{ "--hue": getHue(password.length) }}>
                          <TextField
                            label="Password"
                            type={showPassword ? "text" : "password"}
                            variant="outlined"
                            fullWidth
                            value={formikStepTwo.values.password}
                            onChange={(event) => {
                              setPassword(event.target.value);
                              formikStepTwo.setFieldValue("password", event.target.value);
                            }}
                            onBlur={formikStepTwo.handleBlur}
                            error={formikStepTwo.touched.password && Boolean(formikStepTwo.errors.password)}
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
                          {password !== '' &&
                            <>
                              <LinearProgress
                                variant="determinate"
                                value={Math.min((password.length * 100) / minLength, 100)}
                                sx={{
                                  bgcolor: "background.level3",
                                  color: "hsl(var(--hue) 80% 40%)",
                                  "& .MuiLinearProgress-bar": {
                                    bgcolor: "hsl(var(--hue) 80% 40%)",
                                  },
                                  mt: 1,
                                }}
                              />
                              <Typography
                                variant="body2"
                                sx={{
                                  alignSelf: "flex-end",
                                  color: "hsl(var(--hue) 80% 30%)",
                                  mt: 1,
                                }}
                              >
                                {getStrengthText(password.length)}
                              </Typography>
                            </>

                          }
                          {formikStepTwo.touched.password && formikStepTwo.errors.password && (
                            <Typography
                              variant="body2"
                              sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                            >
                              {formikStepTwo.errors.password}
                            </Typography>
                          )}
                        </Box>
                      </Grid>
                      <Grid item xs={12}>
                        <TextField
                          label="Confirm Password"
                          variant="outlined"
                          type={showConfirmPassword ? "text" : "password"}
                          fullWidth
                          name="confirmPassword"
                          value={formikStepTwo.values.confirmPassword}
                          onChange={formikStepTwo.handleChange}
                          onBlur={formikStepTwo.handleBlur}
                          error={formikStepTwo.touched.confirmPassword && Boolean(formikStepTwo.errors.confirmPassword)}
                          InputProps={{
                            endAdornment: (
                              <InputAdornment position="end">
                                <IconButton
                                  aria-label="toggle password visibility"
                                  onClick={handleClickShowConfirmPassword}
                                  onMouseDown={handleMouseDownPassword}
                                  onMouseUp={handleMouseUpPassword}
                                >
                                  {showConfirmPassword ? <VisibilityOffRoundedIcon /> : <VisibilityRoundedIcon />}
                                </IconButton>
                              </InputAdornment>
                            )
                          }}
                        />
                        {formikStepTwo.touched.confirmPassword && formikStepTwo.errors.confirmPassword && (
                          <Typography
                            variant="body2"
                            sx={{ color: 'red', mt: 1, textAlign: 'center' }}
                          >
                            {formikStepTwo.errors.confirmPassword}
                          </Typography>
                        )}
                      </Grid>
                    </Grid>
                  </Grid>
                </Grid>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
                  <Button
                    color="primary"
                    onClick={handleBack}
                    sx={{ mt: 2 }}
                  >
                    Back
                  </Button>
                  <Button
                    variant="contained"
                    color="primary"
                    type="submit"
                    sx={{ mt: 2 }}
                  >
                    Finish
                  </Button>
                </Box>
              </form>
            </Box>
          )}
          {activeStep === steps.length && (
            <React.Fragment>
              <Typography sx={{ mt: 2, mb: 1, fontSize: '22px'}}>
                Welcome to ShareSpoon!
              </Typography>
              <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
                <Box sx={{ flex: '1 1 auto' }} />
                <Button
                  variant="contained"
                  color="primary"
                  onClick={() => navigate("/")}
                >
                  Begin Now
                </Button>
              </Box>
            </React.Fragment>
          )}
        </Box>
      </Container>
    </>
  );
};

export default Register;
