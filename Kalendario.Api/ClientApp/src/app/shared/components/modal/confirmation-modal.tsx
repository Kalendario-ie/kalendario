import React, {useState} from 'react';
import {FormattedMessage} from 'react-intl';
import KModal, {KModalButtonProps} from 'src/app/shared/components/modal/k-modal';

interface ConfirmationModalProps {
    messageId?: string;
    isOpen: boolean;
    onConfirm: () => void;
    onCancel: () => void;
}

const ConfirmationModal: React.FunctionComponent<ConfirmationModalProps> = (
    {
        messageId = "COMMON.SURE-DELETE",
        isOpen,
        onCancel,
        onConfirm

    }) => {

    const buttons: KModalButtonProps[] = [
        {
            text: 'confirm',
            color: 'primary',
            onClick: onConfirm
        },
        {
            text: 'cancel',
            color: 'danger',
            onClick: onCancel
        }
    ]

    return (
        <KModal
            body={<FormattedMessage id={messageId}/>}
            isOpen={isOpen}
            buttons={buttons}
        />
    )
}


export function useConfirmationModal(
    onConfirm: (id: string) => Promise<any>,
    onSuccess: () => void,
): [(id: string) => void, JSX.Element] {
    const [id, setId] = useState<string | null>(null);

    const handleConfirm = () => {
        onConfirm(id!).then(() => {
            setId(null);
            onSuccess();
        });
    }

    const handleCancel = () => {
        setId(null);
    }

    const modal = <ConfirmationModal isOpen={!!id}
                                     onConfirm={handleConfirm}
                                     onCancel={handleCancel}/>

    return [setId, modal]
}

export default ConfirmationModal;
