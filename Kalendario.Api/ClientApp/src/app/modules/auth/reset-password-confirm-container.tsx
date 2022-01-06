import React, {useState} from 'react';
import {FormattedMessage} from 'react-intl';
import {useParams} from 'react-router-dom';
import {authApi, ResetPasswordRequest} from 'src/app/api/auth';
import {ResetPasswordRequestValidation} from 'src/app/api/auth/validations';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {AUTH_ROUTES} from 'src/app/modules/auth/urls';
import {KFlexColumn} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput, KFormikSubmit} from 'src/app/shared/components/forms';
import {KButton} from 'src/app/shared/components/primitives';
import {KCard, KPageContainer} from 'src/app/shared/components/primitives/containers';
import {useKHistory} from 'src/app/shared/util/router-extensions';


const ResetPasswordConfirmContainer: React.FunctionComponent = () => {
    const [apiError, setApiError] = useState<ApiValidationError | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [complete, setComplete] = useState(false);
    const {uid, token} = useParams<{ uid: string, token: string }>();
    const history = useKHistory();

    const initialValue: ResetPasswordRequest = {newPassword1: '', newPassword2: '', uid, token};

    const handleSubmit = (values: ResetPasswordRequest) => {
        setIsSubmitting(true);
        authApi.resetPasswordConfirm(values)
            .then(() => setComplete(true))
            .catch(error => setApiError(error))
            .finally(() => setIsSubmitting(false));
    }

    const handleLoginClick = () => history.push(AUTH_ROUTES.LOGIN);

    return (
        <KPageContainer>
            <KFlexColumn align="center">
                <KCard maxWidth={400}
                       header={<FormattedMessage id="AUTH.RESET-PASS-HEADER"/>}
                >
                    {!complete &&
                    < KFormikForm initialValues={initialValue}
                                  apiError={apiError}
                                  onSubmit={handleSubmit}
                                  isSubmitting={isSubmitting}
                                  validationSchema={ResetPasswordRequestValidation}
                    >
                        <KFormikInput type="password" name="newPassword1" placeholder="Password"/>
                        <KFormikInput type="password" name="newPassword2" placeholder="Confirm Password"/>
                        <KFormikSubmit text={<FormattedMessage id={"AUTH.RESET-PASS-CONFIRM-BUTTON"}/>}
                                       isBlock={true}
                        />
                    </KFormikForm>
                    }
                    {complete &&
                    <>
                        <FormattedMessage id="AUTH.RESET-PASS-CONFIRM-SENT"/>
                        <KButton block={true} color="primary" onClick={handleLoginClick}>
                            <FormattedMessage id="AUTH.LOGIN"/>
                        </KButton>
                    </>
                    }

                </KCard>
            </KFlexColumn>
        </KPageContainer>
    )
}


export default ResetPasswordConfirmContainer;
