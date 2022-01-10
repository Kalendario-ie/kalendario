import React from 'react';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KCard} from 'src/app/shared/components/primitives/containers';

interface CompanyServicesListProps {
    services: number[]; // TODO PUBLIC SERVICE RESOURCE MODEL.
    categories: number[]; // TODO PUBLIC SERVICE CATEGORY RESOURCE MODEL.
    serviceClick: (id: number) => void;
}

const CompanyServicesList: React.FunctionComponent<CompanyServicesListProps> = (
    {
        services,
        categories,
        serviceClick
    }) => {
    return (
        <KCard>
            <KFlexRow>
                {/*{categories?.length > 1 &&*/}
                {/*<KFlexRowItem>*/}
                {/*    {categories.map((c, k) => <div key={k}>{c.name}</div>)}*/}
                {/*</KFlexRowItem>*/}
                {/*}*/}
                {/*<KFlexRowItem grow={4}>*/}
                {/*    <KGrid size={isMobile ? 12 : 6}>*/}
                {/*        {services?.map((s, k) => <CompanyServicesItem key={k}*/}
                {/*                                                      onClick={serviceClick}*/}
                {/*                                                      service={s}/>)}*/}
                {/*    </KGrid>*/}
                {/*</KFlexRowItem>*/}
            </KFlexRow>
        </KCard>
    )
}

export default CompanyServicesList;
