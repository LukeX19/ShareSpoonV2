import React, { createContext, useState, useEffect, ReactNode, useCallback } from 'react';
import { IUserWithInteractions } from '../interfaces/UserInterface';
import authAxios from '../config/authConfig';
import { Navigate, useLocation } from 'react-router-dom';

interface UserContextType {
  user: IUserWithInteractions | undefined;
  setUser: React.Dispatch<React.SetStateAction<IUserWithInteractions | undefined>>;
}

export const UserContext = createContext<UserContextType | undefined>(undefined);

const UserProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<IUserWithInteractions | undefined>(undefined);
  const isToken = localStorage.getItem('token') ?? false;
  const isLoggedIn = user ?? isToken;
  //const isAdmin = user?.role === 0 ?? false;
  const location = useLocation().pathname;

  const fetchUser = useCallback(async () => {
    try {
      const response = await authAxios.get('/api/users/current');
      setUser(response.data);
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    if (location === '/login' || location === '/register')
      return;
    fetchUser();
  }, []);

  return (isLoggedIn ?
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
    : <Navigate to="/login" />
  );
};

export default UserProvider;
