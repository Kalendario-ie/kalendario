import {CancelToken} from 'axios';
import {ServiceAdminResourceModel, ServicesClient, UpsertServiceCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {durationValidation} from 'src/app/common';
import * as yup from 'yup';

const client = new ServicesClient('', baseApiAxios);

export const adminServiceClient: BaseModelRequest<ServiceAdminResourceModel, UpsertServiceCommand, BaseQueryParams> = {
    get(params) {
        return client.servicesGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertServiceCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.servicesPost(body, cancelToken);
    },
    put(id: string, command: UpsertServiceCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.servicesPut(id, command, cancelToken);
    }
}

export function upsertServiceCommandParser(service: ServiceAdminResourceModel | null | undefined): UpsertServiceCommand {
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
