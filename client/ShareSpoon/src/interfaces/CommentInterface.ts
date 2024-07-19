import { IUser } from "./UserInterface";

export interface IComment {
  id: number;
  userId: string;
  user: IUser;
  recipeId: number;
  text: string;
  createdAt: string;
}

export interface ICommentRequest {
  recipeId: number;
  text: string;
}

export interface ICommentRequestUpdate {
  id: number;
  text: string;
}