import { IIngredient } from "./IngredientInterface";
import { IRecipeIngredient } from "./RecipeIngredientInterface";
import { ITag } from "./TagInterface";
import { IUser } from "./UserInterface";

export interface IRecipeForm {
  name: string;
  estimatedHours: number;
  estimatedMinutes: number;
  difficulty: number;
  ingredients: IIngredient[];
  description: string;
  tags: ITag[];
  pictureURL: string;
}

export interface IRecipeRequest {
  name: string;
  estimatedTime: string;
  difficulty: number;
  ingredients: IIngredient[];
  description: string;
  tags: ITag[];
  pictureURL: string;
}

export interface IRecipe {
  id: number;
  userId: string;
  user: IUser;
  name: string;
  description: string;
  estimatedTime: string;
  difficulty: number;
  recipeIngredients: IRecipeIngredient[];
  recipeTags: ITag[];
  createdAt: string;
  pictureURL: string;
  likesCounter: number;
  currentUserLiked: boolean;
  commentsCounter: number;
}

export interface IRecipeListResponse {
  elements: IRecipe[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}