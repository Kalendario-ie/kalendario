import React from 'react';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KBaseInputProps} from 'src/app/shared/components/primitives/inputs/interfaces';

interface KFormikDurationInputProps extends KBaseInputProps {
    value: string;
    name: string;
}


//TODO: REMOVE THIS AND USE DEFAULT TIME INPUT INSTEAD.
export const KDurationInput: React.FunctionComponent<KFormikDurationInputProps> = (
    {
        value,
        name,
        className,
        onBlur,
        onChange,
        onKeyUp,
    }) => {

    const style = {
        width: '30%'
    }

    return (
        <KFlexRow className={className} justify={'center'}>
            <input
                style={style}
                className="input-no-border"
                name={`${name}.hours`}
                type="number"/>
            <span className="mx-2">hour(s)</span>
            <input
                style={style}
                className="input-no-border"
                name={`${name}.minutes`}
                type="number"
                max={60}/>
            <span className="mx-2">min(s)</span>
        </KFlexRow>
    )
}
