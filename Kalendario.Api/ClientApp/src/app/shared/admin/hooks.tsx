import React, {useEffect, useState} from 'react';
import {IReadModel} from 'src/app/api/common/models';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import KModal from 'src/app/shared/components/modal/k-modal';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {BaseActions, BaseSelectors, CommandAndBaseActions} from 'src/app/store/admin/common/adapter';

export function useEditModal<TEntity extends IReadModel, TUpsertCommand>(
    baseSelectors: BaseSelectors<TEntity>,
    baseActions: CommandAndBaseActions<TUpsertCommand>,
    editContainer: React.FunctionComponent<AdminEditContainerProps<TUpsertCommand>>
): [(command: TUpsertCommand, id?: string | undefined) => void, JSX.Element, TEntity | undefined] {
    const dispatch = useAppDispatch();
    const handleSubmit = (command: TUpsertCommand, id: string | undefined): void => {
        if (!id) {
            dispatch(baseActions.createEntity({command}));
        } else {
            dispatch(baseActions.patchEntity({id, command}));
        }
    }

    return useEditModal2(baseSelectors, baseActions, editContainer, handleSubmit);
}

export function useEditModal2<TEntity extends IReadModel, TUpsertCommand>(
    baseSelectors: BaseSelectors<TEntity>,
    baseActions: BaseActions,
    EditContainer: React.FunctionComponent<AdminEditContainerProps<TUpsertCommand>>,
    handleSubmit: (command: TUpsertCommand, id: string | undefined) => void
): [(command: TUpsertCommand, id?: string | undefined) => void, JSX.Element, TEntity | undefined] {
    const [selectedEntityId, setSelectedEntityId] = useState<string | undefined>();
    const [selectedEntity, setSelectedEntity] = useState<TUpsertCommand | null>(null);
    const apiError = useAppSelector(baseSelectors.selectApiError);
    const editMode = useAppSelector(baseSelectors.selectEditMode);
    const createdEntity = useAppSelector(baseSelectors.selectCreatedEntity);
    const isSubmitting = useAppSelector(baseSelectors.selectIsSubmitting);
    const dispatch = useAppDispatch();

    const handleEditCancel = () => {
        setSelectedEntity(null);
        dispatch(baseActions.setEditMode(false));
    }

    const openModal = React.useMemo(() =>
        (entity: TUpsertCommand, id?: string | undefined) => {
            setSelectedEntity(entity);
            setSelectedEntityId(id);
            dispatch(baseActions.setEditMode(true));
        }, [baseActions, dispatch])

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
