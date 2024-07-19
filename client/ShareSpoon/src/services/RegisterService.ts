import axios from 'axios';
import { IRegisterComplete } from '../interfaces/RegisterInterface';
import { BASE_URL } from '../config/apiConfig';

export const register = async (formData: IRegisterComplete) => {
  const response = await axios.post(`${BASE_URL}/api/auth/register`, formData);
  localStorage.setItem("token", response.data.token);
  return response;
};