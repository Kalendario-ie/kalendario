import React from 'react';
import {EmployeeAdminResourceModel} from 'src/app/api/api';
import ServicesCard from 'src/app/modules/admin/employees/services-card';
import {KFlexRow} from 'src/app/shared/components/flex';

interface EmployeeRowExpandedProps {
    employee: EmployeeAdminResourceModel;
}

const EmployeeRowExpanded: React.FunctionComponent<EmployeeRowExpandedProps> = (
    {
        employee
    }) => {
    return (
        <KFlexRow>
            <ServicesCard serviceIds={employee.services || []}/>
        </KFlexRow>
    )
}


export default EmployeeRowExpanded;
