import { Navigate, Outlet } from 'react-router-dom';
import { useUser } from '../hooks/useUser';

interface ProtectedRouteProps {
  requiredRoles: number[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ requiredRoles }) => {
  const { user } = useUser();

  if (user && !requiredRoles.includes(user.role)) {
    return <Navigate to="/forbidden" />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
