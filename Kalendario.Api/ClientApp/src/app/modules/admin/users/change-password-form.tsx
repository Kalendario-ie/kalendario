import React, {useState} from 'react';
import {Button, FormGroup} from 'reactstrap';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {
    adminUserClient,
    ChangeUserPasswordRequest,
    changeUserPasswordRequestParser,
    ChangeUserPasswordValidation
} from 'src/app/api/users';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import KModal from 'src/app/shared/components/modal/k-modal';

interface ChangePasswordFormProps {
    apiError: ApiValidationError | null;
    isSubmitting: boolean;
    onSubmit: (value: ChangeUserPasswordRequest) => void;
    onCancel: () => void;
}

const ChangePasswordForm: React.FunctionComponent<ChangePasswordFormProps> = (
    {
        apiError,
        isSubmitting,
        onSubmit,
        onCancel
    }) => {

    return (
        <KFormikForm initialValues={changeUserPasswordRequestParser()}
                     apiError={apiError}
                     onSubmit={onSubmit}
                     onCancel={onCancel}
                     isSubmitting={isSubmitting}
                     validationSchema={ChangeUserPasswordValidation}
        >
            <KFormikInput name="password1" type="password"/>
            <KFormikInput name="password2" type="password"/>
            <KFormikInput name="userPassword" type="password"/>
        </KFormikForm>
    )
}

interface ChangePasswordContainerProps {
    id: number;
}

const ChangePasswordContainer: React.FunctionComponent<ChangePasswordContainerProps> = ({id}) => {
    const [isOpen, setIsOpen] = useState(false);
    const [apiError, setApiError] = useState<ApiValidationError | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleButtonClick = () => {
        setIsOpen(true)
    }

    const handleCancel = () => {
        setIsOpen(false);
    }
    const handleSubmit = (form: ChangeUserPasswordRequest) => {
        setIsSubmitting(true);
        adminUserClient.changePassword(id, form)
            .then(() => setIsOpen(false))
            .catch(apiError => setApiError(apiError))
            .finally(() => setIsSubmitting(false));
    }

    const form = <ChangePasswordForm
        apiError={apiError}
        isSubmitting={isSubmitting}
        onSubmit={handleSubmit}
        onCancel={handleCancel}/>

    return (
        <>
            <KModal body={form} isOpen={isOpen}/>
            <FormGroup>
                <Button block={true} color={'primary'} type={'button'} onClick={handleButtonClick}> change
                    password</Button>
            </FormGroup>
        </>
    )
}

export default ChangePasswordContainer;
