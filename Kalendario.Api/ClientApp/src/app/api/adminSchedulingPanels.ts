import {CancelToken} from 'axios';
import {SchedulingPanelAdminResourceModel, SchedulingPanelsClient, UpsertSchedulingPanelCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';

const client = new SchedulingPanelsClient('', baseApiAxios);

export const adminSchedulingPanelsClient: BaseModelRequest<SchedulingPanelAdminResourceModel, UpsertSchedulingPanelCommand, BaseQueryParams> = {
    get(params) {
        return client.schedulingPanelsGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertSchedulingPanelCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.schedulingPanelsPost(body, cancelToken);
    },
    put(id: string, command: UpsertSchedulingPanelCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.schedulingPanelsPut(id, command, cancelToken);
    }
}

export function upsertSchedulingPanelCommandParser(panel: SchedulingPanelAdminResourceModel | null): UpsertSchedulingPanelCommand {
    return panel ? {name: panel.name, employeeIds: panel.employeeIds} : {employeeIds: [], name: ''}
}
