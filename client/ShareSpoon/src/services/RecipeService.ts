import authAxios from '../config/authConfig';
import { IRecipeRequest } from '../interfaces/RecipeInterface';

export const createRecipe = async (formData: IRecipeRequest) => {
  const response = await authAxios.post('/api/recipes', formData);
  return response;
};

export const getRecipes = async (pageIndex: number, pageSize: number = 10) => {
  const response = await authAxios.get('/api/recipes', {
    params: {
      PageIndex: pageIndex,
      PageSize: pageSize
    }
  });
  return response.data;
};

export const getUserRecipes = async (userId: string, pageIndex: number, pageSize: number = 10) => {
  const response = await authAxios.get(`/api/recipes/user/${userId}`, {
    params: {
      PageIndex: pageIndex,
      PageSize: pageSize
    }
  });
  return response.data;
};

export const getUserLikedRecipes = async (userId: string, pageIndex: number, pageSize: number = 10) => {
  const response = await authAxios.get(`/api/recipes/user/${userId}/liked`, {
    params: {
      PageIndex: pageIndex,
      PageSize: pageSize
    }
  });
  return response.data;
};

export const getRecipeById = async (recipeId: number) => {
  const response = await authAxios.get(`/api/recipes/${recipeId}`);
  return response.data;
};

export const searchRecipes = async (input: string | null, promotedUsers: boolean | null, difficulties: number[], tagIds: number[], pageIndex: number, pageSize: number = 10) => {
  const params = new URLSearchParams();
  if (input) params.append('input', input);
  if (difficulties.length > 0) {
    difficulties.forEach(difficulty => params.append('difficulties', difficulty.toString()));
  }
  if (tagIds.length > 0) {
    tagIds.forEach(tagId => params.append('tagIds', tagId.toString()));
  }
  if (promotedUsers !== null) {
    params.append('promotedUsers', promotedUsers.toString());
  }
  params.append('PageIndex', pageIndex.toString());
  params.append('PageSize', pageSize.toString());

  const response = await authAxios.get('/api/recipes/search', { params });
  return response.data;
};

export const updateRecipe = async (recipeId: number, formData: IRecipeRequest) => {
  const response = await authAxios.put(`/api/recipes/${recipeId}`, formData);
  return response;
};

export const deleteRecipe = async (recipeId: number) => {
  const response = await authAxios.delete(`/api/recipes/${recipeId}`);
  return response;
};
