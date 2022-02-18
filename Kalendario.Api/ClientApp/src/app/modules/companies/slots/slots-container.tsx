import React, {useEffect, useState} from 'react';
import {useSelector} from 'react-redux';
import {Slot} from 'src/app/api/api';
import {companyClient} from 'src/app/api/publicCompanyApi';
import {companiesUrls} from 'src/app/modules/companies/paths';
import SlotsView from 'src/app/modules/companies/slots/slots-view';
import {useKHistory} from 'src/app/shared/util/router-extensions';
import {selectUser} from 'src/app/store/auth';
import {selectCompany, selectSelectedDate, selectSelectedServiceId,} from 'src/app/store/companies';

interface SlotsContainerProps {
}

const SlotsContainer: React.FunctionComponent<SlotsContainerProps> = () => {
    const useHistory = useKHistory();

    const company = useSelector(selectCompany);
    const user = useSelector(selectUser);
    const serviceId = useSelector(selectSelectedServiceId);
    const dateFrom = useSelector(selectSelectedDate);

    const [slots, setSlots] = useState<Slot[]>([]);
    const [selectedSlot, setSelectedSlot] = useState<Slot | null>(null);

    const isEmpty = !slots || Object.keys(slots).length === 0;


    useEffect(() => {
        serviceId &&
        companyClient.companiesSlots(serviceId, undefined, dateFrom.format("YYYY-MM-DD"))
            .then(res => setSlots(res.slots || []));
    }, [dateFrom, serviceId]);


    const selectSlotOrAddToCart = (slot: Slot) => {
        if (!serviceId) {
            return;
        }

        if (!selectedSlot || slot.start !== selectedSlot.start) {
            setSelectedSlot(slot);
            return;
        }

        useHistory.push(companiesUrls(company!).book({start: slot.start, service: serviceId}))
    }

    return (
        <>
            <SlotsView isEmpty={isEmpty}
                       slots={slots}
                       selectedSlot={selectedSlot}
                       onClick={selectSlotOrAddToCart}/>
        </>
    )
}


export default SlotsContainer;
