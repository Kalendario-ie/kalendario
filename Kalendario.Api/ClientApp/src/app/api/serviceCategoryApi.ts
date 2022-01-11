import {CancelToken} from 'axios';
import {
    ServiceCategoriesClient,
    ServiceCategoryAdminResourceModel,
    UpsertServiceCategoryCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';


const client = new ServiceCategoriesClient('', baseApiAxios);

export const adminServiceCategoryClient: BaseModelRequest<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand> = {
    post(body: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.serviceCategoriesPost(body, cancelToken);
    },
    get(search: string | undefined, start: number | undefined, length: number | undefined) {
        return client.serviceCategoriesGet(search, start, length);
    },
    put(id: string, command: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.serviceCategoriesPut(id, command, cancelToken);
    }
}

export function createUpsertServiceCategoryRequest(category: ServiceCategoryAdminResourceModel | null | undefined): UpsertServiceCategoryCommand {
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