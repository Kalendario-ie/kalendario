import {
    ActionCreatorWithoutPayload,
    ActionCreatorWithPayload,
    createAction,
    createEntityAdapter,
    createSelector,
    createSlice,
    Dictionary,
    EntitySelectors,
    EntityState,
    OutputParametricSelector
} from '@reduxjs/toolkit';
import {call, put, select, takeEvery} from 'redux-saga/effects';
import {ApiListResult} from 'src/app/api/common/api-results';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import {IReadModel} from 'src/app/api/common/models';
import {compareByName} from 'src/app/shared/util/comparers';
import {PayloadAction} from 'typesafe-actions';


interface BaseState<TEntity> extends EntityState<TEntity> {
    isInitialized: boolean;
    isLoading: boolean;
}

export interface BaseSelectors<TEntity> extends EntitySelectors<TEntity, any> {
    selectByIds: OutputParametricSelector<any, string[], NonNullable<TEntity>[], (res1: Dictionary<TEntity>, res2: string[]) => NonNullable<TEntity>[]>;
    selectIsInitialized: (state: any) => boolean;
    selectIsLoading: (state: any) => boolean;
}

export interface QueryActionPayload<TGetQueryParams> {
    query: TGetQueryParams;
}

export interface ExtendedBaseActions<TGetQueryParams>
    extends BaseActions, QueryActions<TGetQueryParams> {
}

export interface QueryActions<TGetQueryParams> {
    fetchEntities: ActionCreatorWithPayload<QueryActionPayload<TGetQueryParams>>;
    fetchEntitiesWithSetAll: ActionCreatorWithPayload<QueryActionPayload<TGetQueryParams>>;
}

export interface BaseActions {
    initializeStore: ActionCreatorWithoutPayload;
    fetchEntity: ActionCreatorWithPayload<string>;
}


export function kCreateBaseStore<TEntity extends IReadModel, TUpsertCommand, TGetQueryParams>(
    sliceName: string,
    client: BaseModelRequest<TEntity, TUpsertCommand, TGetQueryParams>,
    selector: (state: any) => BaseState<TEntity>
) {

    const adapter = createEntityAdapter<TEntity>({
        selectId: (entity) => entity.id,
        sortComparer: compareByName,
    })

    const actions: ExtendedBaseActions<TGetQueryParams> = {
        initializeStore: createAction<void>(`${sliceName}/initializeStore`),
        fetchEntities: createAction<QueryActionPayload<TGetQueryParams>>(`${sliceName}/fetchEntities`),
        fetchEntity: createAction<string>(`${sliceName}/fetchEntity`),
        fetchEntitiesWithSetAll: createAction<QueryActionPayload<TGetQueryParams>>(`${sliceName}/fetchEntitiesWithSetAll`),
    }

    const slice = createSlice({
        name: sliceName,
        initialState: adapter.getInitialState({
            isInitialized: false,
            isLoading: false,
        }) as BaseState<TEntity>,
        reducers: {
            // @ts-ignore
            upsertMany: adapter.upsertMany,
            // @ts-ignore
            setAll: adapter.setAll,
            // @ts-ignore
            upsertOne: adapter.upsertOne,
            // @ts-ignore
            removeOne: adapter.removeOne,
            // @ts-ignore
            removeAll: adapter.removeAll,
            setInitialized: (state, action) => {
                state.isInitialized = action.payload
            },
            setIsLoading: (state, action: PayloadAction<string, boolean>) => {
                state.isLoading = action.payload;
            }
        }
    });

    const adapterSelectors = adapter.getSelectors(selector);
    const selectors: BaseSelectors<TEntity> = {
        ...adapterSelectors,
        selectByIds: createSelector(
            adapterSelectors.selectEntities,
            (state: any, ids: number[] | string[]) => ids,
            (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
        ),
        selectIsInitialized: createSelector(selector, store => store.isInitialized),
        selectIsLoading: createSelector(selector, store => store.isLoading),
    }

    function* initializeStore(action: { type: string, payload: QueryActionPayload<TGetQueryParams> }) {
        const isInitialized: boolean = yield select(selectors.selectIsInitialized);
        if (isInitialized) return;
        yield put(actions.fetchEntities(action.payload))
    }

    function* fetchEntities(action: { type: string, payload: QueryActionPayload<TGetQueryParams> }) {
        try {
            const result: ApiListResult<TEntity> = yield call([client, client.get], action.payload?.query);
            yield put(slice.actions.upsertMany(result.entities));
            yield put(slice.actions.setInitialized(true));
        } catch (error) {
            yield put(slice.actions.setInitialized(false));
        }
    }

    function* fetchEntity(action: { type: string, payload: number }) {
        yield put(slice.actions.setIsLoading(true));
        try {
            // const entity: TEntity = yield call(client.detail, action.payload);
            // yield put(slice.actions.upsertOne(entity));
        } catch (error) {
        }
        yield put(slice.actions.setIsLoading(false));
    }

    function* fetchEntitiesWithSetAll(action: { type: string, payload: QueryActionPayload<TGetQueryParams> }) {
        yield put(slice.actions.setIsLoading(true));
        yield put(slice.actions.removeAll([]));
        try {
            const result: ApiListResult<TEntity> = yield call([client, client.get], action.payload?.query);
            yield put(slice.actions.setAll(result.entities));
        } catch (error) {
        }
        yield put(slice.actions.setIsLoading(false));
    }

    function* sagas() {
        yield takeEvery(actions.initializeStore.type, initializeStore);
        yield takeEvery(actions.fetchEntities.type, fetchEntities);
        yield takeEvery(actions.fetchEntity.type, fetchEntity);
        yield takeEvery(actions.fetchEntitiesWithSetAll.type, fetchEntitiesWithSetAll);
    }

    return {
        actions,
        adapter,
        reducer: slice.reducer,
        slice,
        sagas,
        selectors
    }
}
