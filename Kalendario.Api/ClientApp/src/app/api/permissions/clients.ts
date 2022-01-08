import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import {PermissionGroup} from 'src/app/api/permissions/models';
import {permissionGroupParser} from 'src/app/api/permissions/parsers';

const baseUrl = 'core/groups/';

export const adminPermissionGroupClient : BaseModelRequest<PermissionGroup, PermissionGroup> = {
    get: search => Promise.resolve({entities: []}),
    put: (id, command) => Promise.resolve(permissionGroupParser(null)),
    post: body => Promise.resolve(permissionGroupParser(null))
}

