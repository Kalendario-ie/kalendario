import {Profile} from 'oidc-client';
import {PermissionModel, PermissionType} from 'src/app/api/auth/permissions';

export function hasPermission(user: Profile, type: PermissionType, model: PermissionModel) {
    return user.UserRoles.includes(`${PermissionType}_${model}`) || user.UserRoles.includes('master');
}

