import React from 'react';
import {adminServiceClient, upsertServiceCommandParser} from 'src/app/api/adminServicesApi';
import {PermissionModel} from 'src/app/api/auth';
import ServiceUpsertForm from 'src/app/modules/admin/services/service-upsert-form';
import ServicesTable from 'src/app/modules/admin/services/services-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {serviceActions, serviceSelectors, serviceSlice} from 'src/app/store/admin/services';


interface ServicesContainerProps {
}

const ServicesContainer: React.FunctionComponent<ServicesContainerProps> = () => {
    return (
        <AdminListEditContainer baseSelectors={serviceSelectors}
                                baseActions={serviceActions}
                                actions={serviceSlice.actions}
                                onCreate={adminServiceClient.post}
                                onUpdate={adminServiceClient.put}
                                parser={upsertServiceCommandParser}
                                modelType={PermissionModel.service}
                                EditContainer={ServiceUpsertForm}
                                ListContainer={ServicesTable}/>
    )
}


export default ServicesContainer;
