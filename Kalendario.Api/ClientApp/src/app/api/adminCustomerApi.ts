import {CancelToken} from 'axios';
import {CustomerAdminResourceModel, CustomersClient, UpsertCustomerCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new CustomersClient('', baseApiAxios);

export const adminCustomerClient: BaseModelRequest<CustomerAdminResourceModel, UpsertCustomerCommand, BaseQueryParams> = {
    get(params) {
        return client.customersGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertCustomerCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.customersCreate(body, cancelToken);
    },
    put(id: string, command: UpsertCustomerCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.customersUpdate(id, command, cancelToken);
    },
    delete(id: string , cancelToken?: CancelToken | undefined) {
        return client.customersDelete(id, cancelToken)
    }
}

export function upsertCustomerCommandParser(customer: CustomerAdminResourceModel | null): UpsertCustomerCommand {
    return customer ? {
        name: customer.name,
        phoneNumber: customer.phoneNumber,
        email: customer.email,
        warning: customer.warning
    } : {
        email: '', name: '', phoneNumber: '', warning: ''
    }
}

export const upsertCustomerCommandValidation = yup.object().shape({
    name: yup.string().required(),
    phoneNumber: yup.string().required(),
    email: yup.string().required().email(),
});
