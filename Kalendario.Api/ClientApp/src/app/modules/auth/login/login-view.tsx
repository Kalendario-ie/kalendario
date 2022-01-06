import React from 'react';
import {FormattedMessage} from 'react-intl';
import {LoginRequest} from 'src/app/api/auth/requests';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {KFormikForm, KFormikInput, KFormikSubmit} from 'src/app/shared/components/forms';

export interface LoginViewProps {
    data: LoginRequest;
    isSubmitting: boolean;
    onSubmit: (data: LoginRequest) => void;
    apiError: ApiValidationError | null;
    validationSchema?: any | (() => any);
}

const LoginView: React.FunctionComponent<LoginViewProps> = (
    {
        data,
        isSubmitting,
        onSubmit,
        apiError,
        validationSchema
    }) => {
    return (
        <KFormikForm initialValues={data}
                     apiError={apiError}
                     onSubmit={onSubmit}
                     isSubmitting={isSubmitting}
                     validationSchema={validationSchema}>
            <>
                <KFormikInput name="email" type="email"/>
                <KFormikInput name="password" type="password"/>
                <KFormikSubmit text={<FormattedMessage id={"AUTH.LOGIN"}/>}
                               isBlock={true}
                />
            </>
        </KFormikForm>
    )
}

export default LoginView;
