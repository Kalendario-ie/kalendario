import {AuthUser} from 'src/app/api/auth/models';
import {PermissionModel, PermissionType} from 'src/app/api/auth/permissions';

export function hasPermission(user: AuthUser, type: PermissionType, model: PermissionModel) {
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
