import React from 'react';
import {adminServiceClient} from 'src/app/api/adminServicesApi';
import {PermissionModel} from 'src/app/api/auth';
import ServiceUpsertForm from 'src/app/modules/admin/services/service-upsert-form';
import ServicesTable from 'src/app/modules/admin/services/services-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {serviceActions, useInitializeServices} from 'src/app/store/admin/services';


interface ServicesContainerProps {
}

const ServicesContainer: React.FunctionComponent<ServicesContainerProps> = () => {
    const [, entities] = useInitializeServices({length: undefined, search: undefined, start: undefined});

    return (
        <AdminListEditContainer entities={entities}
                                actions={serviceActions}
                                client={adminServiceClient}
                                modelType={PermissionModel.service}
                                EditContainer={ServiceUpsertForm}
                                ListContainer={ServicesTable}/>
    )
}


export default ServicesContainer;
