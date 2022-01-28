import React, {ChangeEvent, useState} from 'react';
import {Input} from 'reactstrap';

interface KFileInputProps {
    onChange: (file: File) => void;
}

const KFileInput: React.FunctionComponent<KFileInputProps> = (
    {
        onChange
    }) => {
    const [error, setError] = useState<string | null>(null);

    const handleFileChange = ({target: {files}}: ChangeEvent<HTMLInputElement>) => {
        if (files && files.length > 0) {
            const file = files[0];

            if (!file.name.toLowerCase().match(/\.(jpg|jpeg|png)$/)) {
                setError('Please select valid image.');
                return false;
            }

            if (file.size > 5242880) {
                setError('File is too big.');
                return false;
            }

            onChange(file);
        }
    }

    return (
        <>
            <Input
                withIcon={true}
                singleImage={true}
                withPreview={true}
                buttonText='Choose images'
                type={'file'}
                onChange={handleFileChange}
            />
            {error &&
            <div className="c-danger">
                {error}
            </div>
            }
        </>
    )
}


export default KFileInput;
