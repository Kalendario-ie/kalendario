import {useFormikContext} from 'formik';
import React from 'react';
import {CreateScheduleFrame} from 'src/app/api/api';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {KFormikInput} from 'src/app/shared/components/forms';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';

interface ScheduleFormikInputProps {
    name: string;

}

const ScheduleFormikInput: React.FunctionComponent<ScheduleFormikInputProps> = (
    {
        name
    }) => {
    const formik = useFormikContext();
    const formikValues = formik.getFieldProps<CreateScheduleFrame[]>(name);
    const formikHelpers = formik.getFieldHelpers(name);

    const handleAddClick = () => {
        const newFrame = {start: `00:00`, end: `00:00`};
        const values = [...formikValues.value, newFrame];
        formikHelpers.setValue(values);
    }

    const handleFrameDelete = (indexToRemove: number) => () => {
        formikHelpers.setValue(formikValues.value.filter((v, index) => index !== indexToRemove));
    }

    return (
        <KFlexColumn>
            <KFlexRow>
                {name}
                <KIconButton icon={'plus'} color={'primary'} onClick={handleAddClick}/>
            </KFlexRow>
            {formikValues.value.map((frame, i) =>
                <KFlexRow key={i} align={'center'}>
                    <KFormikInput placeholder="Start" name={`${name}[${i}].start`} type="time"/>
                    <KFormikInput placeholder="End" name={`${name}[${i}].end`} type="time"/>
                    <KIconButton icon={'trash'} color={'danger'} onClick={handleFrameDelete(i)}/>
                </KFlexRow>
            )}
        </KFlexColumn>
    )
}


export default ScheduleFormikInput;
