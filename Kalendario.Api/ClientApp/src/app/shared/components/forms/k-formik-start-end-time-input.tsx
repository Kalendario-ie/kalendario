import {useFormikContext} from 'formik';
import {Moment} from 'moment';
import React, {ChangeEvent, useEffect, useState} from 'react';
import {FormGroup, Input, Label} from 'reactstrap';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {KDateInput} from 'src/app/shared/components/primitives/inputs';
import {addHours, stringToMoment} from 'src/app/shared/util/moment-helpers';

function useDateHelper(name: string): [Moment, (value: Moment) => void, string, (event: ChangeEvent<HTMLInputElement>) => void] {
    const formik = useFormikContext();
    const {value} = formik.getFieldMeta<string>(name);
    const {setValue} = formik.getFieldHelpers(name);

    const momentValue = stringToMoment(value);
    const [time, setTime] = useState(momentValue.format('HH:mm'));

    useEffect(() => {
        const momentValue = stringToMoment(value);
        setTime(momentValue.format('HH:mm'))
    }, [value]);

    const handleDateChange = (value: Moment) => {
        setValue((addHours(value.startOf('day'), time)));

    }
    const handleTimeChange = (e: ChangeEvent<HTMLInputElement>) => {
        setTime(e.target.value);
        setValue((addHours(momentValue.startOf('day'), e.target.value)));
    }


    return [momentValue, handleDateChange, time, handleTimeChange]
}

const KFormikStartEndTimeInput: React.FunctionComponent = () => {
    const [start, handleDateChange, startTime, handleStartTimeChange] = useDateHelper('start');
    const [, handleEndDateChange, endTime, handleEndTimeChange] = useDateHelper('end');

    return (
        <>
            <FormGroup>
                <KFlexColumn>
                    <Label>Date</Label>
                    <KDateInput value={start}
                                onChange={(e) => {
                                    handleDateChange(e);
                                    handleEndDateChange(e);
                                }}/>
                </KFlexColumn>
            </FormGroup>
            <FormGroup>
                <KFlexRow align={'center'} justify={'center'}>
                    <KFlexColumn className="w-100">
                        Start
                        <Input value={startTime} onChange={handleStartTimeChange} type={'time'}/>
                    </KFlexColumn>
                    <KFlexColumn className="w-100">
                        Finish
                        <Input value={endTime} onChange={handleEndTimeChange} type={'time'}/>
                    </KFlexColumn>
                </KFlexRow>
            </FormGroup>
        </>
    )
}


export default KFormikStartEndTimeInput;
