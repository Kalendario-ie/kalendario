import React from 'react';
import {adminScheduleClient} from 'src/app/api/adminSchedulesApi';
import {PermissionModel} from 'src/app/api/auth';
import ScheduleUpsertForm from 'src/app/modules/admin/schedules/schedule-upsert-form';
import SchedulesTable from 'src/app/modules/admin/schedules/schedules-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {scheduleActions, useInitializeSchedules} from 'src/app/store/admin/schedules';


interface SchedulesContainerProps {
}

const SchedulesContainer: React.FunctionComponent<SchedulesContainerProps> = () => {
    const [, entities] = useInitializeSchedules({length: undefined, search: undefined, start: undefined});

    return (
            <AdminListEditContainer entities={entities}
                                    actions={scheduleActions}
                                    client={adminScheduleClient}
                                    modelType={PermissionModel.schedule}
                                    EditContainer={ScheduleUpsertForm}
                                    ListContainer={SchedulesTable}/>
    )
}


export default SchedulesContainer;
