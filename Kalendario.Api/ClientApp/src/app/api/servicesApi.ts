import {CancelToken} from 'axios';
import {ServiceAdminResourceModel, ServicesClient, UpsertServiceCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';

const client = new ServicesClient('', baseApiAxios);

export const adminServiceClient: BaseModelRequest<ServiceAdminResourceModel, UpsertServiceCommand> = {
    post(body: UpsertServiceCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.servicesPost(body, cancelToken);
    },
    get(search: string | undefined, start: number | undefined, length: number | undefined) {
        return client.servicesGet(search, start, length);
    },
    put(id: string, command: UpsertServiceCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.servicesPut(id, command, cancelToken);
    }
}

