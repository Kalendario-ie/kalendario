import {CustomerAdminResourceModel, UpsertCustomerCommand} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import {personParser} from 'src/app/api/common/parsers';
import {Customer} from 'src/app/api/customers/models';
import {SaveCustomerRequest} from '.';


export function customerParser(data?: any): Customer {
    return {
        name: '',
        ...personParser(data),
        warning: data.warning
    }

}

export function saveCustomerRequestParser(customer: CustomerAdminResourceModel | null): UpsertCustomerCommand {
    return customer ? {
        name: customer.name,
        phoneNumber: customer.phoneNumber,
        email: customer.email,
        warning: customer.warning
    } : {
        email: '', name: '', phoneNumber: '', warning: ''
    }
}
