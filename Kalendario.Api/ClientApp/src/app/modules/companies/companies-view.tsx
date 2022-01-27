import React from 'react';
import {CompanyDetailsResourceModel} from 'src/app/api/api';
import CompanyServicesList from 'src/app/modules/companies/company-services/company-services-list';
import {KPageContainer} from 'src/app/shared/components/primitives/containers';
import CompanyAvatar from './avatar/company-avatar';

interface CompaniesViewProps {
    company: CompanyDetailsResourceModel;
    serviceClick: (id: string) => void;
}

const CompaniesView: React.FunctionComponent<CompaniesViewProps> = (
    {
        company,
        serviceClick
    }) => {
    return (
        <KPageContainer>
            <CompanyAvatar company={company}
            />
            <CompanyServicesList services={company.services || []}
                                 categories={company.serviceCategories || []}
                                 serviceClick={serviceClick}
            />
        </KPageContainer>
    )
}


export default CompaniesView;
