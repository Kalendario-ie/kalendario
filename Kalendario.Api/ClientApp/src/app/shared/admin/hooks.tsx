import {CaseReducers} from '@reduxjs/toolkit/src/createReducer';
import {CaseReducerActions, SliceCaseReducers} from '@reduxjs/toolkit/src/createSlice';
import React, {useEffect, useState} from 'react';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {IReadModel} from 'src/app/api/common/models';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import KModal from 'src/app/shared/components/modal/k-modal';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {BaseActions, BaseSelectors} from 'src/app/store/admin/common/adapter';

export function useEditModal<TEntity extends IReadModel, TUpsertCommand>(
    baseSelectors: BaseSelectors<TEntity>,
    actions: CaseReducerActions<SliceCaseReducers<any>>,
    EditContainer: React.FunctionComponent<AdminEditContainerProps<TUpsertCommand>>,
    onCreate: (command: TUpsertCommand) => Promise<TEntity>,
    onUpdate: (id: string, command: TUpsertCommand) => Promise<TEntity>
): [(command: TUpsertCommand, id?: string | undefined) => void, JSX.Element, TEntity | undefined] {
    const [selectedEntityId, setSelectedEntityId] = useState<string | undefined>();
    const [selectedEntity, setSelectedEntity] = useState<TUpsertCommand | null>(null);
    const [createdEntity, setCreatedEntity] = useState<TEntity | undefined>();
    const [apiError, setApiError] = useState<ApiValidationError | null>(null);
    const [editMode, setEditMode] = useState(false);
    const dispatch = useAppDispatch();

    const isSubmitting = useAppSelector(baseSelectors.selectIsSubmitting);

    const handleEditCancel = () => {
        setSelectedEntity(null);
        setEditMode(false);
    }

    const openModal = React.useMemo(() =>
        (entity: TUpsertCommand, id?: string | undefined) => {
            setSelectedEntity(entity);
            setSelectedEntityId(id);
            setEditMode(true);
            setCreatedEntity(undefined);

        }, [])

    const handleSubmit = (command: TUpsertCommand, id: string | undefined): void => {
        setApiError(null);

        if (!id) {
            onCreate(command)
                .then(entity => {
                    dispatch(actions.upsertOne(entity));
                    setEditMode(false);
                    setCreatedEntity(entity);
                }).catch(error => setApiError(error));
        } else {
            onUpdate(id, command)
                .then(entity => {
                    dispatch(actions.upsertOne(entity));
                    setEditMode(false);
                }).catch(error => setApiError(error));
        }
    }

    const modal = <KModal body={<EditContainer id={selectedEntityId}
                                               command={selectedEntity!}
                                               apiError={apiError}
                                               onSubmit={handleSubmit}
                                               isSubmitting={isSubmitting}
                                               onCancel={handleEditCancel}/>}
                          isOpen={editMode}/>

    return [openModal, modal, createdEntity]
}


export function useSelectAll<TEntity>(baseSelectors: BaseSelectors<TEntity>, baseActions: BaseActions) {
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(baseActions.initializeStore());
    }, [baseActions, dispatch]);
    return useAppSelector(baseSelectors.selectAll);
}

/**
 * a shortcut effect to dispatch the initialize store action
 * @param baseActions The base action for the store that needs to be initialized
 */
export function useInitializeEffect(baseActions: BaseActions) {
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(baseActions.initializeStore());
    }, [baseActions, dispatch]);
}
