import {CaseReducerActions, SliceCaseReducers} from '@reduxjs/toolkit';
import React, {useEffect, useState} from 'react';
import {IReadModel} from 'src/app/api/common/models';
import {AdminFormProps} from 'src/app/shared/admin/interfaces';
import KModal from 'src/app/shared/components/modal/k-modal';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {BaseActions, BaseSelectors} from 'src/app/store/admin/common/adapter';

export function useEditModal<TEntity extends IReadModel>(
    baseSelectors: BaseSelectors<TEntity>,
    actions: CaseReducerActions<SliceCaseReducers<any>>,
    EditContainer: React.FunctionComponent<AdminFormProps<TEntity>>,
): [(command: TEntity | null) => void, JSX.Element, TEntity | undefined] {
    const [selectedEntity, setSelectedEntity] = useState<TEntity | null>(null);
    const [createdEntity, setCreatedEntity] = useState<TEntity | undefined>();
    const [editMode, setEditMode] = useState(false);
    const dispatch = useAppDispatch();

    const handleEditCancel = () => {
        setSelectedEntity(null);
        setEditMode(false);
    }

    const openModal = React.useMemo(() =>
        (entity: TEntity | null) => {
            setSelectedEntity(entity);
            setEditMode(true);
            setCreatedEntity(undefined);

        }, [])

    const onSuccess = (entity: TEntity) => {
        dispatch(actions.upsertOne(entity));
        setEditMode(false);
        if (!!selectedEntity) {
            setCreatedEntity(entity);
        }
    }

    const formModal = <KModal body={<EditContainer entity={selectedEntity}
                                                   onSuccess={onSuccess}
                                                   onCancel={handleEditCancel}/>}
                              isOpen={editMode}/>

    return [openModal, formModal, createdEntity]
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
