import React from 'react';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import {BaseModelRequestDelete} from 'src/app/api/common/clients/base-django-api';
import {IReadModel} from 'src/app/api/common/models';
import AdminButton from 'src/app/shared/admin/admin-button';
import {useConfirmationModal} from 'src/app/shared/components/modal/confirmation-modal';

interface DeleteButtonProps {
    entity: IReadModel;
    modelType: PermissionModel;
    client: BaseModelRequestDelete;
    onSuccess: () => void;
}

const DeleteButton: React.FunctionComponent<DeleteButtonProps> = (
    {
        entity,
        modelType,
        client,
        onSuccess
    }) => {
    const [setDeleteId, modal] = useConfirmationModal(id => client.delete(id), onSuccess);

    const handleDeleteClick = (id: string) => () => {
        setDeleteId(id);
    }
    return (
        <>
            {modal}
            <AdminButton type={PermissionType.delete}
                         model={modelType}
                         onClick={handleDeleteClick(entity.id)}/>
        </>
    )
}


export default DeleteButton;
