import React, {ChangeEvent, useState} from 'react';
import {TimeSpan} from 'src/app/api/api';
import {timeFromString, TimeOfDay, timeToISOString, Zero} from 'src/app/api/common/models';
import {KBaseInputProps} from 'src/app/shared/components/primitives/inputs/interfaces';
import {KFlexRow} from 'src/app/shared/components/flex';

interface KFormikDurationInputProps extends KBaseInputProps {
    value: string;
    name: string;
}

export const KDurationInput: React.FunctionComponent<KFormikDurationInputProps> = (
    {
        value,
        name,
        className,
        onBlur,
        onChange,
        onKeyUp,
    }) => {
    const [timeOfDay, setTimeOfDay] = useState(timeFromString(value));

    const hourHandler = (e: ChangeEvent<HTMLInputElement>) => {
        handleChange(e, {...Zero(), hours: +e.target.value, minutes: timeOfDay.minutes});
    }

    const minuteHandler = (e: ChangeEvent<HTMLInputElement>) => {
        handleChange(e, {...Zero(), hours: timeOfDay.hours, minutes: +e.target.value})
    }

    const handleChange = (e: ChangeEvent<HTMLInputElement>, newValue: TimeSpan) => {
        const type = 'string';
        setTimeOfDay(newValue);
        onChange && onChange({...e, target: {...e.target, type, value: timeToISOString(newValue)}});
    }

    const style = {
        width: '30%'
    }

    return (
        <KFlexRow className={className} justify={'center'}>
            <span className="mx-2">hour(s)</span>
            <input
                style={style}
                className="input-no-border"
                name={name}
                onBlur={onBlur}
                onChange={hourHandler}
                type="number"
                value={timeOfDay.hours}/>
            <span className="mx-2">min(s)</span>
            <input
                style={style}
                className="input-no-border"
                name={name}
                onBlur={onBlur}
                onChange={minuteHandler}
                type="number"
                max={60}
                value={timeOfDay.minutes}/>
        </KFlexRow>
    )
}
