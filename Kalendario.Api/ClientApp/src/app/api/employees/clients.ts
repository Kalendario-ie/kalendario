import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import {Employee} from 'src/app/api/employees/models';
import {employeeParser} from 'src/app/api/employees/parsers';

const baseUrl = 'admin/employees/';

export const adminEmployeeClient: BaseModelRequest<Employee, Employee> = {
    get: (search, start, length) => Promise.resolve({entities: []}),
    put: (id, command) => Promise.resolve(employeeParser(null)),
    post: body => Promise.resolve(employeeParser(null))
    // ...baseModelRequest(baseUrl, employeeParser),
    //
    // uploadProfilePicture(id: number, file: File): Promise<{ url: string }> {
    //     const formData = new FormData();
    //     formData.append('image', file);
    //     return baseApiAxios.post<{url: string}>(baseUrl + id + '/photo/', formData)
    //         .then(result => result.data);
    // }
}
