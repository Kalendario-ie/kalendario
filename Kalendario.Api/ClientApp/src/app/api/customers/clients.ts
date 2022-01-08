import {CustomerAdminResourceModel, CustomersClient, UpsertCustomerCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';

const baseUrl = 'admin/customers/';
const client = new CustomersClient(baseUrl, baseApiAxios);

export const adminCustomerClient: BaseModelRequest<CustomerAdminResourceModel, UpsertCustomerCommand> = {
    post: client.customersPost,
    get: client.customersGet,
    put: client.customersPut,
}
