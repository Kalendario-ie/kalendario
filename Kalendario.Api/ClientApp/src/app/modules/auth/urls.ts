

export enum AUTH_ROUTES {
    ROOT = '/auth',
    LOGIN = '/auth/login',
    REGISTER = '/auth/register',
    RESET_PASSWORD = '/auth/reset-password',
    RESET_PASSWORD_CONFIRM = '/auth/password-reset/confirm/:uid/:token',
    LOGOUT = '/auth/logout',
}
