import {CaseReducerActions, SliceCaseReducers} from '@reduxjs/toolkit/src/createSlice';
import React, {useEffect} from 'react';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import {BaseModelRequestDelete} from 'src/app/api/common/clients/base-django-api';
import {IReadModel} from 'src/app/api/common/models';
import AdminDeleteButton from 'src/app/shared/admin/delete-button';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {AdminFormProps, AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import {useKHistory} from 'src/app/shared/util/router-extensions';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {KFlexRow} from '../components/flex';
import AdminButton from './admin-button';

interface AdminListEditContainerProps<TEntity> {
    entities: TEntity[];
    actions: CaseReducerActions<SliceCaseReducers<any>>;
    modelType: PermissionModel;
    filter?: (value: string | undefined) => void;
    detailsUrl?: string;
    client: BaseModelRequestDelete;
    EditContainer: React.FunctionComponent<AdminFormProps<TEntity>>;
    ListContainer: React.FunctionComponent<AdminTableContainerProps<TEntity>>;
}

function AdminListEditContainer<TEntity extends IReadModel, TUpsertCommand>(
    {
        actions,
        entities,
        filter,
        detailsUrl,
        modelType,
        client,
        EditContainer,
        ListContainer,
    }: AdminListEditContainerProps<TEntity>) {
    const dispatch = useAppDispatch();
    const [openModal, formModal] = useEditModal(actions, EditContainer);
    const history = useKHistory();

    const buttonsColumn = React.useMemo(() =>
        ({
            Header: () =>
                <KFlexRow justify={'end'}>
                    <AdminButton type={PermissionType.add}
                                 model={modelType}
                                 onClick={() => openModal(null)}/>
                </KFlexRow>,
            id: 'buttons',
            Cell: (value: any) => (
                <KFlexRow align="end" justify="end">
                    {detailsUrl &&
                    <AdminButton type={PermissionType.view}
                                 model={modelType}
                                 onClick={() => history.push(`${detailsUrl}/${value.row.original.id}`)}/>
                    }
                    <AdminButton type={PermissionType.change}
                                 model={modelType}
                                 onClick={() => openModal(value.row.original)}/>
                    <AdminDeleteButton entity={value.row.original}
                                  modelType={modelType}
                                       onSuccess={() => dispatch(actions.removeOne(value.row.original.id))}
                                       client={client}/>
                </KFlexRow>
            )
            // eslint-disable-next-line react-hooks/exhaustive-deps
        }), [])

    return (
        <>
            {formModal}
            <ListContainer entities={entities}
                           filter={filter}
                           buttonsColumn={buttonsColumn}/>
        </>
    )
}


export default AdminListEditContainer;
