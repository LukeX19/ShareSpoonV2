export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  birthday: string;
  pictureURL: string;
  role: number;
}

export interface IUserWithInteractions {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  birthday: string;
  age: number;
  pictureURL: string;
  role: number;
  postedRecipesCounter: number;
  receivedLikesCounter: number;
}