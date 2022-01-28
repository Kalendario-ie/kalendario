import {CancelToken} from 'axios';
import {ApplicationUserAdminResourceModel, UpsertUserCommand, UsersClient} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new UsersClient('', baseApiAxios);

export const adminUsersClient: BaseModelRequest<ApplicationUserAdminResourceModel, UpsertUserCommand, BaseQueryParams> = {
    get(params) {
        return client.usersGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertUserCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.usersCreate(body, cancelToken);
    },
    put(id: string, command: UpsertUserCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.usersUpdate(id, command, cancelToken);
    },
    delete(id: string, cancelToken?: CancelToken | undefined) {
        return Promise.resolve();
    }
}

export const UpsertUserRequestValidation = yup.object().shape({
    userName: yup.string().required(),
    email: yup.string().required().email(),
    roleGroupId: yup.string().required(),
    employeeId: yup.string(),
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


export function upsertUserRequestParser(user: ApplicationUserAdminResourceModel | null): UpsertUserCommand {
    return user == null ? {
        userName: '',
        email: '',
        roleGroupId: '',
        employeeId: ''
    } : {
        userName: user.userName,
        email: user.email,
        roleGroupId: user.roleGroupId,
        employeeId: user.employeeId
    }
}

export function changeUserPasswordRequestParser(): ChangeUserPasswordRequest {
    return {
        password1: '', password2: '', userPassword: ''
    }
}
