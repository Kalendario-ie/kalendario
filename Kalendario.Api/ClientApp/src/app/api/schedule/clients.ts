import {ScheduleAdminResourceModel, SchedulesClient, UpsertScheduleCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';

const baseUrl = 'admin/schedules/';
const client = new SchedulesClient(baseUrl, baseApiAxios);

export const adminScheduleClient: BaseModelRequest<ScheduleAdminResourceModel, UpsertScheduleCommand> = {
    get: client.schedulesGet,
    post: client.schedulesPost,
    put: client.schedulesPut
}
