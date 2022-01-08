import axios, {AxiosResponse, CancelToken} from 'axios';
import {UpsertCustomerCommand} from 'src/app/api/api';
import {ApiListResult} from '../api-results';
import {convertMoment} from '../helpers';
import baseApiAxios from './base-api';

export interface BaseModelRequest<TResourceModel, TUpsertCommand> {
    get: (search: string | undefined, start: number | undefined, length: number | undefined) => Promise<ApiListResult<TResourceModel>>;
    // detail: (id: number, params?: {}) => Promise<TResourceModel>;
    post: (body: TUpsertCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    put: (id: string, command: TUpsertCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    // delete: (id: string) => Promise<AxiosResponse>;
}

// function baseModelRequest<TEntity, TFilter = object>(baseUrl: string, adapter: (model: any) => TEntity): BaseModelRequest<TEntity, TFilter> {
//     const token = axios.CancelToken;
//     let source = token.source();
//     return {
//         get(filter: any): Promise<ApiListResult<TEntity>> {
//             const params = convertMoment(filter);
//             source.cancel('New request was made, canceling old request.');
//             source = token.source();
//             return baseApiAxios.get<ApiListResult<TEntity>>(baseUrl, {params, cancelToken: source.token})
//                 .then(project => {
//                         project.data.results = project.data.results.map(r => adapter(r));
//                         return project.data;
//                     }
//                 )
//         },
//
//         post(model: any): Promise<TEntity> {
//             return baseApiAxios.post(baseUrl, model)
//                 .then(result => adapter(result.data));
//         },
//
//         detail(id: number, params = {}): Promise<TEntity> {
//             return baseApiAxios.get<TEntity>(baseUrl + id + '/', {params: {...params}})
//                 .then(result => adapter(result.data));
//         },
//
//         patch(id: number, model: any): Promise<TEntity> {
//             return baseApiAxios.patch<TEntity>(baseUrl + id + '/', model)
//                 .then(result => adapter(result.data));
//         },
//
//         put(id: number, model: any): Promise<TEntity> {
//             return baseApiAxios.put<TEntity>(baseUrl + id + '/', model)
//                 .then(result => adapter(result.data));
//         },
//
//         delete(id: number): Promise<AxiosResponse> {
//             return baseApiAxios.delete<void>(baseUrl + id + '/');
//         }
//     }
//
// }

// export default baseModelRequest;
