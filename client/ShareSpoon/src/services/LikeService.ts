import authAxios from "../config/authConfig";
import { ILikeRequest } from "../interfaces/LikeInterface";

export const createLike = async (formData: ILikeRequest) => {
  const response = await authAxios.post('/api/likes', formData);
  return response;
};

export const deleteLike = async (recipeId: number) => {
  const response = await authAxios.delete(`/api/likes/${recipeId}`);
  return response;
};