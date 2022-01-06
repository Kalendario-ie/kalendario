export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password1: string;
    password2: string;
}

export interface ChangePasswordRequest {
    oldPassword: string;
    newPassword1: string;
    newPassword2: string;
}

export interface ResetPasswordRequest {
    uid: string;
    token: string;
    newPassword1: string;
    newPassword2: string;
}

export interface ForgotPasswordRequest {
    email: string;
}
