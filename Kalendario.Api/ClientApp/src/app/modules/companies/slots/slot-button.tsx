import moment from 'moment';
import React from 'react';
import {Slot} from 'src/app/api/api';

interface SlotButtonProps {
    slot: Slot;
    isSelected: boolean;
    onClick: () => void;
}

const SlotButton: React.FunctionComponent<SlotButtonProps> = (
    {
        slot,
        isSelected,
        onClick
    }) => {
    let className = 'slot-button bs-primary m-1';
    if (isSelected) {
        className += ' no-border bg-primary-gradient'
    }
    return (
        <div className={className} onClick={() => onClick()}>
            <div>{moment.utc(slot.start).format("HH:mm")} - {moment.utc(slot.end).format('HH:mm')}</div>
            <div hidden={!isSelected}>
                Book Now
            </div>
        </div>
    )
}


export default SlotButton;
