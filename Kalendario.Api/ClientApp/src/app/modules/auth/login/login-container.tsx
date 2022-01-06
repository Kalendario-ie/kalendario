import React from 'react';
import {FormattedMessage} from 'react-intl';
import {useDispatch, useSelector} from 'react-redux'
import {LoginRequest} from 'src/app/api/auth/requests';
import {LoginRequestValidation} from 'src/app/api/auth/validations';
import {useRedirectIfLoggedInEffect} from 'src/app/modules/auth/effects';
import {AUTH_ROUTES} from 'src/app/modules/auth/urls';
import {KFlexColumn} from 'src/app/shared/components/flex';
import {KTextButton} from 'src/app/shared/components/primitives/buttons';
import {KCard, KPageContainer} from 'src/app/shared/components/primitives/containers';
import {useKHistory} from 'src/app/shared/util/router-extensions';
import {loginRequest, selectApiError, selectIsSubmitting} from 'src/app/store/auth';
import LoginView from './login-view';


const LoginContainer = () => {
    useRedirectIfLoggedInEffect();
    const dispatch = useDispatch();
    const apiError = useSelector(selectApiError);
    const isSubmitting = useSelector(selectIsSubmitting);
    const initialValues: LoginRequest = {email: '', password: ''};

    const handleSubmit = (data: LoginRequest): void => {
        dispatch(loginRequest(data));
    };

    const history = useKHistory();

    const handlePasswordReset = () => {
        history.push(AUTH_ROUTES.RESET_PASSWORD);
    }

    // const faceBookAppId = process.env.REACT_APP_FACEBOOK_APP_ID;
    //
    // const facebookCallback = (userInfo: ReactFacebookLoginInfo | ReactFacebookFailureResponse) => {
    //     if ('id' in userInfo) {
    //         dispatch(facebookLoginRequest(userInfo.accessToken));
    //     }
    // }

    return (
        <KPageContainer>
            <KFlexColumn align="center">
                <KCard maxWidth={400}
                       header={<FormattedMessage id="AUTH.LOGIN-HEADER"/>}
                >
                    <>
                        {/*{faceBookAppId &&*/}
                        {/*<FacebookLogin*/}
                        {/*    appId={faceBookAppId}*/}
                        {/*    autoLoad={false}*/}
                        {/*    size={'small'}*/}
                        {/*    fields="name,email,picture"*/}
                        {/*    callback={facebookCallback}/>*/}
                        {/*}*/}

                        <LoginView onSubmit={handleSubmit}
                                   isSubmitting={isSubmitting}
                                   data={initialValues}
                                   apiError={apiError}
                                   validationSchema={LoginRequestValidation}
                        />
                    </>
                    <KTextButton onClick={handlePasswordReset}>
                        <FormattedMessage id="AUTH.RESET-PASS-TEXT"/>
                    </KTextButton>
                </KCard>
            </KFlexColumn>
        </KPageContainer>
    )
}

export default LoginContainer;
