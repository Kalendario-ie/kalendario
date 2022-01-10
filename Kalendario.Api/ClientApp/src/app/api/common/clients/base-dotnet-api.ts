import {CancelToken} from 'axios';
import {ApiListResult} from 'src/app/api/common/api-results';


export interface BaseModelRequest<TResourceModel, TUpsertCommand> {
    get: (search: string | undefined, start: number | undefined, length: number | undefined) => Promise<ApiListResult<TResourceModel>>;
    // detail: (id: number, params?: {}) => Promise<TResourceModel>;
    post: (body: TUpsertCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    put: (id: string, command: TUpsertCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<TResourceModel>;
    // delete: (id: string) => Promise<AxiosResponse>;
}

