import {CancelToken} from 'axios';
import {EmployeeAdminResourceModel, EmployeesClient, UpsertEmployeeCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new EmployeesClient('', baseApiAxios);

export interface AdminEmployeesApi extends BaseModelRequest<EmployeeAdminResourceModel, UpsertEmployeeCommand, BaseQueryParams> {
    employeesUploadFile(id: string, body: Blob | undefined , cancelToken?: CancelToken | undefined): Promise<EmployeeAdminResourceModel>;
}

export const adminEmployeeClient: AdminEmployeesApi = {
    get(params) {
        return client.employeesGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertEmployeeCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.employeesCreate(body, cancelToken);
    },
    put(id: string, command: UpsertEmployeeCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.employeesUpdate(id, command, cancelToken);
    },
    delete(id: string, cancelToken?: CancelToken | undefined) {
        return client.employeesDelete(id, cancelToken);
    },
    employeesUploadFile(id: string, data: Blob | undefined, cancelToken?: CancelToken | undefined) {
        return client.employeesUploadFile(id, {fileName: '', data}, cancelToken);
    }
}

export function upsertEmployeeCommandParser(employee: EmployeeAdminResourceModel | null | undefined): UpsertEmployeeCommand {
    return employee ? {
        name: employee.name,
        email: employee.email,
        phoneNumber: employee.phoneNumber,
        scheduleId: employee.scheduleId,
        services: employee.services
    } : {
        name: '',
        email: '',
        phoneNumber: '',
        scheduleId: undefined,
        services: []
    }
}

export const upsertEmployeeCommandValidation = yup.object().shape({
    name: yup.string().required(),
    email: yup.string().required().email(),
    phoneNumber: yup.string().required(),
    scheduleId: yup.string().nullable(),
    services: yup.array(yup.string()),
});
