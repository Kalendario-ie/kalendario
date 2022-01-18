import axios, {AxiosResponse, CancelToken} from 'axios';
import {UpsertCustomerCommand} from 'src/app/api/api';
import {ApiListResult} from '../api-results';
import {convertMoment} from '../helpers';
import baseApiAxios from './base-api';

export interface BaseQueryParams {
    search: string | undefined;
    start: number | undefined;
    length: number | undefined;
    cancelToken?: CancelToken | undefined;
}

export interface BaseModelRequest<TResourceModel, TUpsertCommand, TGetQueryParams> {
    get: (params: TGetQueryParams) => Promise<ApiListResult<TResourceModel>>;
    // detail: (id: number, params?: {}) => Promise<TResourceModel>;
    post: (body: TUpsertCommand | undefined, cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    put: (id: string, command: TUpsertCommand | undefined, cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    delete: (id: string , cancelToken?: CancelToken | undefined) => Promise<void>;
}
