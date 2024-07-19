import { HelmetProvider } from "react-helmet-async";
import { Routes, Route, Outlet } from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Recipe from "./pages/Recipe/Recipe";
import BadRequest from "./pages/BadRequest/BadRequest";
import Register from "./pages/Register";
import CreateRecipe from "./pages/CreateRecipe/CreateRecipe";
import UserProvider from "./context/UserProvider";
import AdminPanel from "./pages/AdminPanel";
import Profile from "./pages/Profile";
import UpdateRecipe from "./pages/UpdateRecipe/UpdateRecipe";
import ProtectedRoute from "./components/ProtectedRoute";
import Forbidden from "./pages/Forbidden/Forbidden";
import { ToastContainer, Zoom } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { ThemeProvider, createTheme } from "@mui/material";
import NavbarLayout from "./components/NavbarLayout";

const theme = createTheme({
  palette: {
    primary: {
      main: '#FFD44D'
    },
    secondary: {
      // Navbar
      main: '#023255',
    },
  },
  typography: {
    fontFamily: 'Poppins, sans-serif',
  }
});

const UserProviderWrapper: React.FC = () => (
  <UserProvider>
    <Outlet />
  </UserProvider>
);

function App() {
  return (
    <ThemeProvider theme={theme}>
      <HelmetProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route element={<UserProviderWrapper />}>
            <Route element={<NavbarLayout />}>
              <Route path="/" element={<Home />} />
              <Route path="/recipe/:recipeId" element={<Recipe />} />
              <Route element={<ProtectedRoute requiredRoles={[1, 2]} />}>
                <Route path="/recipe/create" element={<CreateRecipe />} />
                <Route path="/profile" element={<Profile />} />
                <Route path="/recipe/:recipeId/update" element={<UpdateRecipe />} />
              </Route>
              <Route element={<ProtectedRoute requiredRoles={[0]} />}>
                <Route path="/admin" element={<AdminPanel />} />
              </Route>
            </Route>
            <Route path="/forbidden" element={<Forbidden />} />
            <Route path="*" element={<BadRequest />} />
          </Route>
        </Routes>
        <ToastContainer
          position="top-right"
          autoClose={3000}
          hideProgressBar={false}
          newestOnTop={true}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
          transition={Zoom}
        />
      </HelmetProvider>
    </ThemeProvider>
  );
};

export default App;
