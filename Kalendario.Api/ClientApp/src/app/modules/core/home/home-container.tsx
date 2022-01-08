import React, {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {useHistory} from "react-router-dom";
import {Company} from 'src/app/api/companies';
import {selectCompany, selectOwnerId} from 'src/app/store/companies';


const HomeContainer: React.FunctionComponent = () => {
    const history = useHistory();
    const ownerId = useSelector(selectOwnerId);
    const company = useSelector(selectCompany);

    useEffect(() => {
        if (company && ownerId) {
            history.push(`/c/${company.name}`);
        }
        if (ownerId && !company) {
            // companyClient.detail(ownerId)
            //     .then(company => history.push(`/c/${company.name}`)) //todo: fix here.
        }
    }, [company, history, ownerId]);


    // const promiseOptions = (value: string) => companyClient.get({search: value})
    //     .then(res => res.results); // todo fix here

    const navigateToPage = (company: Company | null) => {
        if (company) {
            history.push(`/c/${company.name}`)
        }
    }

    return (
        <>
            {!ownerId &&
                <></>
            // <HomeView values={promiseOptions}
            //           onChange={navigateToPage}/>
            }
        </>
    )
}

export default HomeContainer;
