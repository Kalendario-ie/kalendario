import {CancelToken} from 'axios';
import * as moment from 'moment';
import {
    RoleAdminResourceModel,
    RoleGroupAdminResourceModel,
    RoleGroupsClient,
    UpsertApplicationRoleGroupCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

export interface AdminPermissionGroupClient extends BaseModelRequest<RoleGroupAdminResourceModel, UpsertApplicationRoleGroupCommand, BaseQueryParams> {
    roles: (cancelToken?: CancelToken | undefined) => Promise<RoleAdminResourceModel[]>;
}

const client = new RoleGroupsClient('', baseApiAxios);


export const adminPermissionGroupClient: AdminPermissionGroupClient = {
    get(params) {
        return client.roleGroupsGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertApplicationRoleGroupCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.roleGroupsCreate(body, cancelToken);
    },
    put(id: string, command: UpsertApplicationRoleGroupCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.roleGroupsUpdate(id, command, cancelToken);
    },
    delete(id: string, cancelToken?: CancelToken | undefined) {
        return Promise.resolve();
    },
    roles(cancelToken?: CancelToken | undefined) {
        return client.roleGroupsGetAllRoles({}, cancelToken);
    }
}

export const UpsertPermissionRequestValidation = yup.object().shape({
    name: yup.string().required(),
    roles: yup.array(yup.string()).required().min(1),
});

export function upsertPermissionGroupRequestParser(roleGroup: RoleGroupAdminResourceModel | null): UpsertApplicationRoleGroupCommand {
    return roleGroup == null ? {
        name: '',
        roles: [],
    } : {
        name: roleGroup.name,
        roles: roleGroup.roles || []
    }
}
