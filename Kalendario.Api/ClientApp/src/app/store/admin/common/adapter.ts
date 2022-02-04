import {Dictionary, EntitySelectors, OutputParametricSelector} from '@reduxjs/toolkit';
import {CaseReducerActions, SliceCaseReducers} from '@reduxjs/toolkit/src/createSlice';
import {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {BaseModelRequestGet} from 'src/app/api/common/clients/base-django-api';
import {RootState, useAppDispatch} from 'src/app/store/store';

export function useInitializeStore<TGetQueryParams, TResourceModel>(
    client: BaseModelRequestGet<TGetQueryParams, TResourceModel>,
    selectors: BaseSelectors<TResourceModel>,
    params: TGetQueryParams,
    actions: CaseReducerActions<SliceCaseReducers<RootState>>
): [isInitialized: boolean, entities: TResourceModel[]] {
    const isInitialized = useSelector(selectors.selectIsInitialized);
    const entities = useSelector(selectors.selectAll);
    const dispatch = useAppDispatch();

    useEffect(() => {
            if (!isInitialized) {
                client.get(params)
                    .then(res => {
                        dispatch(actions.setInitialized(true))
                        dispatch(actions.setAll(res.entities || []));
                    })
            }
    }, [isInitialized]);

    return [isInitialized, entities];
}


export interface BaseSelectors<TEntity> extends EntitySelectors<TEntity, any> {
    selectByIds: OutputParametricSelector<any, string[], NonNullable<TEntity>[], (res1: Dictionary<TEntity>, res2: string[]) => NonNullable<TEntity>[]>;
    selectIsInitialized: (state: RootState) => boolean;
}
