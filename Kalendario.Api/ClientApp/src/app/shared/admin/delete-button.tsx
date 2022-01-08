import React from 'react';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import {IReadModel} from 'src/app/api/common/models';
import AdminButton from 'src/app/shared/admin/admin-button';
import {UseConfirmationModalWithDispatch} from 'src/app/shared/components/modal/delete-confirmation-modal';
import {BaseActions, ExtendedBaseActions} from 'src/app/store/admin/common/adapter';

interface DeleteButtonProps {
    entity: IReadModel;
    modelType: PermissionModel;
    baseActions: BaseActions;
}

const DeleteButton: React.FunctionComponent<DeleteButtonProps> = (
    {
        entity,
        modelType,
        baseActions
    }) => {
    const [setDeleteId, confirmDeleteModal] = UseConfirmationModalWithDispatch(baseActions.deleteEntity);

    const handleDeleteClick = (id: number | string) => () => { // todo string only
        setDeleteId(+id); // todo remove +
    }
    return (
        <>
            {confirmDeleteModal}
            <AdminButton type={PermissionType.delete}
                         model={modelType}
                         onClick={handleDeleteClick(entity.id)}/>
        </>
    )
}


export default DeleteButton;
