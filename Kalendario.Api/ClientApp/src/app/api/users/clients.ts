import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {AdminUser} from 'src/app/api/users/models';
import {userParser} from 'src/app/api/users/parsers';
import {ChangeUserPasswordRequest, UpsertUserRequest} from 'src/app/api/users/requests';

const baseUrl = 'core/users/';

const client: BaseModelRequest<AdminUser, UpsertUserRequest, BaseQueryParams> = {
    get: search => Promise.resolve({entities: []}),
    post: body => Promise.resolve(userParser(null)),
    put: id => Promise.resolve(userParser(null)),
    delete: id => Promise.resolve(),
}

export const adminUserClient = {
    ...client,
    changePassword: (id: string, model: ChangeUserPasswordRequest): Promise<AdminUser> =>
        baseApiAxios
            .patch<AdminUser>(baseUrl + id + '/changePassword/', model)
            .then(data => userParser(data.data))
}
