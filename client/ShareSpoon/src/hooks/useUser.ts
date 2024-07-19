import { useContext } from 'react';
import { UserContext } from '../context/UserProvider';

export const useUser = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUser hook must be used within a UserProvider');
  }
  return context;
};