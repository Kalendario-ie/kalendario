import {CaseReducerActions, SliceCaseReducers} from '@reduxjs/toolkit';
import React, {useState} from 'react';
import {IReadModel} from 'src/app/api/common/models';
import {AdminFormProps} from 'src/app/shared/admin/interfaces';
import KModal from 'src/app/shared/components/modal/k-modal';
import {useAppDispatch} from 'src/app/store';

export function useEditModal<TEntity extends IReadModel>(
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
        if (!selectedEntity) {
            setCreatedEntity(entity);
        }
    }

    const formModal = <KModal body={<EditContainer entity={selectedEntity}
                                                   onSuccess={onSuccess}
                                                   onCancel={handleEditCancel}/>}
                              isOpen={editMode}/>

    return [openModal, formModal, createdEntity]
}
