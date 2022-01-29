import React, {useState} from 'react';
import {useHistory} from 'react-router-dom';
import {adminCompanyClient} from 'src/app/api/adminCompaniesApi';
import {CreateAccountCommand} from 'src/app/api/api';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {ADMIN_ROUTES} from 'src/app/modules/admin/urls';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput, KFormikSubmit} from 'src/app/shared/components/forms';
import {KCard, KPageContainer} from 'src/app/shared/components/primitives/containers';

const CreateCompanyContainer: React.FunctionComponent = () => {
    const [apiError, setApiError] = useState<ApiValidationError | null>(null);
    const history = useHistory();

    const onSubmit = (company: CreateAccountCommand): Promise<any> =>  adminCompanyClient
            .post(company)
            .then(_ => history.push(ADMIN_ROUTES.ROOT))
            .catch(e => setApiError(e));

    return (
        <KPageContainer>
            <KFlexRow justify="center">
                <KCard header="Create Company">
                    <KFormikForm initialValues={{name: ''}}
                                 apiError={apiError}
                                 onSubmit={onSubmit}
                    >
                        <KFormikInput name="name"/>
                        <KFormikSubmit/>
                    </KFormikForm>
                </KCard>
            </KFlexRow>
        </KPageContainer>
    )
}


export default CreateCompanyContainer;
