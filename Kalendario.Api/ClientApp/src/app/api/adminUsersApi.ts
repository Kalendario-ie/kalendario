import {ApplicationUserAdminResourceModel, UsersClient} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new UsersClient('', baseApiAxios);

export const adminUsersClient: BaseModelRequest<ApplicationUserAdminResourceModel, UpsertUserRequest, BaseQueryParams> = {
    get(params) {
        return client.usersGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post: body => Promise.resolve(userParser(null)),
    put: id => Promise.resolve(userParser(null)),
    delete: id => Promise.resolve(),
}

export function userParser(data: any): ApplicationUserAdminResourceModel {
    return {
        id: '',
        userName: ''
    }
}

export interface UpsertUserRequest {
    firstName: string;
    lastName: string;
    email: string;
    employee: number | '';
    groups: number[];
}

export const UpsertUserRequestValidation = yup.object().shape({
    firstName: yup.string().required(),
    lastName: yup.string().required(),
    email: yup.string().required().email(),
});


export interface ChangeUserPasswordRequest {
    userPassword: string;
    password1: string;
    password2: string;
}

export const ChangeUserPasswordValidation = yup.object().shape({
    password1: yup.string().required(),
    password2: yup.string().required()
        .oneOf([yup.ref('password1'), null], 'Passwords must match'),
    userPassword: yup.string().required()
});


export function upsertUserRequestParser(user: ApplicationUserAdminResourceModel | null): UpsertUserRequest {
    return {
        firstName: '',
        lastName: '',
        email: '',
        employee: '',
        groups: []
    }
}

export function changeUserPasswordRequestParser(): ChangeUserPasswordRequest {
    return {
        password1: '', password2: '', userPassword: ''
    }
}
