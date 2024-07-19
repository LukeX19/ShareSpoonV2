import authAxios from "../config/authConfig";

export const createIngredient = async (name: string) => {
  const response = await authAxios.post('/api/ingredients', {
    name: name
  });
  return response;
};