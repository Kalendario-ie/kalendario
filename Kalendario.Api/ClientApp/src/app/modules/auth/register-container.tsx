import React from 'react';
import {FormattedMessage} from 'react-intl';
import {useSelector} from 'react-redux';
import {RegisterRequest} from 'src/app/api/auth';
import {parseRegisterRequest} from 'src/app/api/auth/parsers';
import {RegisterRequestValidation} from 'src/app/api/auth/validations';
import {useRedirectIfLoggedInEffect} from 'src/app/modules/auth/effects';
import {KFlexColumn} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput, KFormikSubmit} from 'src/app/shared/components/forms';
import {KCard, KPageContainer} from 'src/app/shared/components/primitives/containers';
import {useAppDispatch} from 'src/app/store';
import {registerRequest, selectApiError, selectIsSubmitting} from 'src/app/store/auth';


const RegisterContainer = () => {
    useRedirectIfLoggedInEffect();

    const apiError = useSelector(selectApiError);
    const isSubmitting = useSelector(selectIsSubmitting)
    const dispatch = useAppDispatch();

    const handleSubmit = (data: RegisterRequest) => {
        dispatch(registerRequest(data));
    };

    return (
        <KPageContainer>
            <KFlexColumn align="center">
                <KCard maxWidth={400}
                       header={<FormattedMessage id="AUTH.REGISTER-HEADER"/>}
                >
                    <KFormikForm initialValues={parseRegisterRequest()}
                                 apiError={apiError}
                                 onSubmit={handleSubmit}
                                 isSubmitting={isSubmitting}
                                 validationSchema={RegisterRequestValidation}
                    >
                        <KFormikInput name="firstName"/>
                        <KFormikInput name="lastName"/>
                        <KFormikInput type="email" name="email"/>
                        <KFormikInput type="password" name="password1" placeholder="password"/>
                        <KFormikInput type="password" name="password2" placeholder="confirm password"/>
                        <KFormikSubmit text={<FormattedMessage id={"AUTH.REGISTER"}/>}
                                       isBlock={true}
                        />
                    </KFormikForm>
                </KCard>
            </KFlexColumn>
        </KPageContainer>
    )
}

export default RegisterContainer;
