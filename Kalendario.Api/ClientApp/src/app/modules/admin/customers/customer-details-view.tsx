import React from 'react';
import {CustomerAdminResourceModel} from 'src/app/api/api';
import {KFlexColumn} from 'src/app/shared/components/flex';
import KIcon from 'src/app/shared/components/primitives/k-icon';

interface CustomerDetailsViewProps {
    customer: CustomerAdminResourceModel;
}

const CustomerDetailsView: React.FunctionComponent<CustomerDetailsViewProps> = (
    {
        customer
    }) => {
    return (
        <KFlexColumn>
            <KIcon icon="user" text={customer.name}/>
            <KIcon icon="phone" text={customer.phoneNumber}/>
            <KIcon icon="at" text={customer.email}/>
            <KIcon icon="exclamation" text={customer.warning}/>
        </KFlexColumn>
    )
}


export default CustomerDetailsView;
