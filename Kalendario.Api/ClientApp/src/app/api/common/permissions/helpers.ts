import {ApplicationUserAdminResourceModel} from 'src/app/api/api';
import {ModelPermissions, PermissionModels} from './index';


export function userPermissions(user: ApplicationUserAdminResourceModel, model: PermissionModels): ModelPermissions {
    // return {
    //   view: user.permissions.includes(`${getApp(model)}.${PERMISSION_VIEW}_${model}`),
    //   add: user.permissions.includes(`${getApp(model)}.${PERMISSION_ADD}_${model}`),
    //   change: user.permissions.includes(`${getApp(model)}.${PERMISSION_CHANGE}_${model}`),
    //   delete: user.permissions.includes(`${getApp(model)}.${PERMISSION_DELETE}_${model}`),
    // };
    return {view: true, add: true, change: true, delete: true}; //TODO FIX HERE.
}

//
// export function appointmentPermissions(user: IUser): AppointmentPermissions {
//   const model = Appointment.modelType;
//   return {
//     ...userPermissions(user, model),
//     overlap: user.permissionGroups.includes(`${getApp(model)}.overlap_${model}`),
//   };
// }
