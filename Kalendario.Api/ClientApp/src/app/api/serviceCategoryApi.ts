import {CancelToken} from 'axios';
import {
    ServiceCategoriesClient,
    ServiceCategoryAdminResourceModel,
    UpsertServiceCategoryCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';


const serviceCategoryClient = new ServiceCategoriesClient('', baseApiAxios);

export const adminServiceCategoryClient: BaseModelRequest<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand> = {
    post(body: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return serviceCategoryClient.serviceCategoriesPost(body, cancelToken);
    },
    get(search: string | undefined, start: number | undefined, length: number | undefined) {
        return serviceCategoryClient.serviceCategoriesGet(search, start, length);
    },
    put(id: string, command: UpsertServiceCategoryCommand | undefined, cancelToken?: CancelToken | undefined) {
        return serviceCategoryClient.serviceCategoriesPut(id, command, cancelToken);
    }
}
