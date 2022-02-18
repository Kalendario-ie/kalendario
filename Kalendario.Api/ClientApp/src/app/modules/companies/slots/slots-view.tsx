import React from 'react';
import {isMobile} from 'react-device-detect';
import {FormattedMessage} from 'react-intl';
import {Slot} from 'src/app/api/api';
import SlotButton from 'src/app/modules/companies/slots/slot-button';
import {KFlexRow} from 'src/app/shared/components/flex';

interface SlotsViewProps {
    isEmpty: boolean;
    slots: Slot[];
    selectedSlot: Slot | null;
    onClick: (slot: Slot) => void;
}

const SlotsView: React.FunctionComponent<SlotsViewProps> = (
    {
        isEmpty,
        slots,
        selectedSlot,
        onClick
    }) => {

    return (
        <KFlexRow flexWrap={true} justify={isMobile ? 'center' : 'between'}>
            {slots.map((slot) =>
                <SlotButton slot={slot}
                            key={slot.start}
                            isSelected={!!selectedSlot && slot.start === selectedSlot.start}
                            onClick={() => onClick(slot)}/>)
            }
            {isEmpty && <FormattedMessage id="COMPANY.NO-SLOTS"/>}
        </KFlexRow>
    )
}


export default SlotsView;
