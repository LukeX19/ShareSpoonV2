import authAxios from "../config/authConfig";
import { ICommentRequest } from "../interfaces/CommentInterface";

export const getCommentsForRecipe = async (recipeId: number, pageIndex: number, pageSize: number = 10) => {
  const response = await authAxios.get(`/api/comments/${recipeId}`, {
    params: {
      PageIndex: pageIndex,
      PageSize: pageSize
    }
  });
  return response.data;
};

export const createComment = async (formData: ICommentRequest) => {
  const response = await authAxios.post('/api/comments', formData);
  return response;
};

export const deleteComment = async (commentId: number,) => {
  const response = await authAxios.delete(`/api/comments/${commentId}`);
  return response;
};