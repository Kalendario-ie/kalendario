import {Profile} from 'oidc-client';
import {PermissionModel, PermissionType} from 'src/app/api/auth/permissions';

export function hasPermission(user: Profile, type: PermissionType, model: PermissionModel) {
    const app = getAppLabel(model);
    return true;
    // return user.permissions.includes(`${app}.${type}_${model}`); // fix here later.
}

function getAppLabel(model: PermissionModel): string {
    switch (model) {
        case PermissionModel.user:
        case PermissionModel.groupprofile:
            return 'core';
        default:
            return 'scheduling';
    }
}
