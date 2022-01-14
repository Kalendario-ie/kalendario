import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {PermissionGroup} from 'src/app/api/permissions/models';
import {permissionGroupParser} from 'src/app/api/permissions/parsers';
import {UpsertPermissionGroupRequest} from 'src/app/api/permissions/requests';

const baseUrl = 'core/groups/';

export const adminPermissionGroupClient : BaseModelRequest<PermissionGroup, UpsertPermissionGroupRequest, BaseQueryParams> = {
    get: search => Promise.resolve({entities: []}),
    put: (id, command) => Promise.resolve(permissionGroupParser(null)),
    post: body => Promise.resolve(permissionGroupParser(null))
}

