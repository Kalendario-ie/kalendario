import {CancelToken} from 'axios';
import {
    ServiceCategoriesClient,
    ServiceCategoryAdminResourceModel,
    UpsertServiceCategoryCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';


const client = new ServiceCategoriesClient('', baseApiAxios);

export const adminServiceCategoryClient: BaseModelRequest<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand, BaseQueryParams> = {
    get(params) {
        return client.serviceCategoriesGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.serviceCategoriesCreate(body, cancelToken);
    },
    put(id: string, command: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.serviceCategoriesUpdate(id, command, cancelToken);
    }
}

export function upsertServiceCategoryCommandParser(category: ServiceCategoryAdminResourceModel | null | undefined): UpsertServiceCategoryCommand {
    return category ? {
        name: category.name,
        colour: category.colour.code || '#FFFFFF'
    } : {
        name: '',
        colour: '#FFFFFF',
    }
}

export const UpsertServiceCategoryRequestValidation = yup.object().shape({
    name: yup.string().required().max(255),
    colour: yup.string().required(),
});
