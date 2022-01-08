import {
    ServiceAdminResourceModel,
    ServiceCategoriesClient,
    ServiceCategoryAdminResourceModel,
    ServicesClient,
    UpsertServiceCategoryCommand,
    UpsertServiceCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';

const baseUrl = 'admin/services/';
const baseServiceCategoryUrl = 'admin/serviceCategories/';
const client = new ServicesClient(baseUrl, baseApiAxios);
const serviceCategoryClient = new ServiceCategoriesClient(baseServiceCategoryUrl, baseApiAxios);

export const adminServiceClient: BaseModelRequest<ServiceAdminResourceModel, UpsertServiceCommand> = {
    post: client.servicesPost,
    get: client.servicesGet,
    put: client.servicesPut,
}

export const adminServiceCategoryClient: BaseModelRequest<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand> = {
    post: serviceCategoryClient.serviceCategoriesPost,
    get: serviceCategoryClient.serviceCategoriesGet,
    put: serviceCategoryClient.serviceCategoriesPut,
}
