import React, {useState} from 'react';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import KFileInput from 'src/app/shared/components/forms/k-file-input';
import KModal from 'src/app/shared/components/modal/k-modal';
import AvatarImg from 'src/app/shared/components/primitives/avatar-img';
import {KButton} from 'src/app/shared/components/primitives/buttons';
import styles from './editable-avatar-img.module.scss';

interface EditableAvatarImgProps {
    src: string;
    size: number;
    onSubmit: (file: File) => Promise<boolean>;
}

const EditableAvatarImg: React.FunctionComponent<EditableAvatarImgProps> = (
    {
        src,
        size,
        onSubmit
    }) => {
    const [modalOpen, setModalOpen] = useState(false);
    const [file, setFile] = useState<File | null>(null);

    const avatarStyle: React.CSSProperties = {
        width: `${size}rem`,
        height: `${size}rem`,
    }

    const handleCancel = () => setModalOpen(false);
    const handleSubmit = () => file && onSubmit(file).then(res => setModalOpen(!res));

    const modalBody =
        <>
            <KFileInput onChange={(file) => setFile(file)}/>
            <KFlexRow className="w-100" justify={'end'}>
                <KButton onClick={handleSubmit} color="primary" className="mr-2">Submit</KButton>
                <KButton onClick={handleCancel} color="danger">Cancel</KButton>
            </KFlexRow>
        </>

    const modal = <KModal body={modalBody} isOpen={modalOpen}/>

    return (
        <KFlexColumn className="position-relative" justify={'center'}>
            {modal}
            <AvatarImg src={src} size={size}/>
            <KButton type="button"
                     onClick={() => setModalOpen(true)}
                     style={avatarStyle}
                     className={`${styles.btnOverlay} round-image`}>
                edit
            </KButton>
        </KFlexColumn>
    )
}

export default EditableAvatarImg;
