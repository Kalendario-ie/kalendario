import React, {useEffect, useState} from 'react';
import {IReadModel} from 'src/app/api/common/models';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import KModal from 'src/app/shared/components/modal/k-modal';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {ExtendedBaseActions, BaseSelectors, BaseActions} from 'src/app/store/admin/common/adapter';

export function useEditModal<TEntity extends IReadModel, TUpsertCommand>(
    baseSelectors: BaseSelectors<TEntity>,
    baseActions: ExtendedBaseActions<TUpsertCommand>,
    EditContainer: React.FunctionComponent<AdminEditContainerProps<TEntity, TUpsertCommand>>
): [(entity: TEntity | null) => () => void, JSX.Element, TEntity | undefined] {
    const [selectedEntity, setSelectedEntity] = useState<TEntity | null>(null);
    const apiError = useAppSelector(baseSelectors.selectApiError);
    const editMode = useAppSelector(baseSelectors.selectEditMode);
    const createdEntity = useAppSelector(baseSelectors.selectCreatedEntity);
    const isSubmitting = useAppSelector(baseSelectors.selectIsSubmitting);
    const dispatch = useAppDispatch();

    const handleEditCancel = () => {
        setSelectedEntity(null);
        dispatch(baseActions.setEditMode(false));
    }

    const handleSubmit = (command: TUpsertCommand, id: string | undefined): void => {
        if (!id) {
            dispatch(baseActions.createEntity({command}));
        } else {
            dispatch(baseActions.patchEntity({id, command}));
        }
    }

    const openModal = React.useMemo(() =>
        (entity: TEntity | null) => () => {
            setSelectedEntity(entity);
            dispatch(baseActions.setEditMode(true));
        }, [baseActions, dispatch])

    const modal = <KModal body={<EditContainer entity={selectedEntity}
                                               apiError={apiError}
                                               onSubmit={handleSubmit}
                                               isSubmitting={isSubmitting}
                                               onCancel={handleEditCancel}/>}
                          isOpen={editMode}/>

    return [openModal, modal, createdEntity]
}


export function useSelectAll<TEntity, TUpsertCommand>(baseSelectors: BaseSelectors<TEntity>, baseActions: ExtendedBaseActions<TUpsertCommand>) {
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
