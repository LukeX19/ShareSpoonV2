import axios from 'axios';
import { ILogin } from '../interfaces/LoginInterface';
import { BASE_URL } from '../config/apiConfig';

export const login = async (formData: ILogin) => {
  const response = await axios.post(`${BASE_URL}/api/auth/login`, formData);
  localStorage.setItem("token", response.data.token);
  return response;
};