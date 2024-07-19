import authAxios from "../config/authConfig";

export const createTag= async (name: string, type: number) => {
  const response = await authAxios.post('/api/tags', {
    name: name,
    type: type
  });
  return response;
};

export const getFilterTags = async () => {
  const response = await authAxios.get('/api/tags/filter');
  return response;
};