export interface IRegisterStepOne {
    firstName: string,
    lastName: string,
    birthday: string,
    role: number
}

export interface IRegisterStepTwo {
    email: string,
    password: string,
    confirmPassword: string,
    pictureURL: string
}

export interface IRegisterComplete {
    firstName: string,
    lastName: string,
    birthday: string,
    pictureURL: string,
    email: string,
    password: string,
    role: number
}