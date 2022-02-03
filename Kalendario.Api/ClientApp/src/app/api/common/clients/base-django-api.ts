import {CancelToken} from 'axios';
import {ApiListResult} from '../api-results';

export interface BaseQueryParams {
    search: string | undefined;
    start: number | undefined;
    length: number | undefined;
    cancelToken?: CancelToken | undefined;
}

export interface BaseModelRequest<TResourceModel, TUpsertCommand, TGetQueryParams> extends BaseModelRequestGet<TGetQueryParams, TResourceModel>,
    BaseModelRequestPostPut<TResourceModel, TUpsertCommand>,
    BaseModelRequestDelete {
}

export interface BaseModelRequestGet<TGetQueryParams, TResourceModel> {
    get: (params: TGetQueryParams) => Promise<ApiListResult<TResourceModel>>;
}


export interface BaseModelRequestDelete {
    delete: (id: string, cancelToken?: CancelToken | undefined) => Promise<void>;
}

export interface BaseModelRequestPostPut<TResourceModel, TUpsertCommand> {
    post: (body: TUpsertCommand | undefined, cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    put: (id: string, command: TUpsertCommand | undefined, cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
}
