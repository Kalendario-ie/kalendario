import React from 'react';
import {useHistory} from "react-router-dom";
import {CompaniesClient, CompanySearchResourceModel} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {companyClient} from 'src/app/api/publicCompanyApi';
import HomeView from 'src/app/modules/core/home/home-view';


const HomeContainer: React.FunctionComponent = () => {
    const history = useHistory();

    const promiseOptions = (search: string) => companyClient.companiesSearch(search).then(r => r.entities || []);

    const navigateToPage = (company: CompanySearchResourceModel | null) => {
        if (company) {
            history.push(`/c/${company.name}`)
        }
    }

    return (
            <HomeView values={promiseOptions}
                      onChange={navigateToPage}/>
    )
}

export default HomeContainer;
