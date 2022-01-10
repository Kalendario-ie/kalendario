import {CancelToken} from 'axios';
import {ServiceAdminResourceModel, ServicesClient, UpsertServiceCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import {durationValidation} from 'src/app/common';
import * as yup from 'yup';

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

export function createUpsertServiceCommand(service: ServiceAdminResourceModel | null | undefined): UpsertServiceCommand {
    return service ? {
        name: service.name,
        description: service.description,
        price: service.price,
        duration: service.duration,
        serviceCategoryId: service.serviceCategoryId,
    } : {
        name: '',
        description: '',
        price: 0,
        duration: '00:00:00',
        serviceCategoryId: '',
    }
}

export const upsertServiceCommandValidation = yup.object().shape({
    name: yup.string().required().max(255),
    description: yup.string().required().max(255),
    price: yup.number(),
    duration: durationValidation,
    serviceCategoryId: yup.string(),
});
