import React from 'react';
import {ServicePublicResourceModel} from 'src/app/api/api';
import {KFlexRow, KFlexRowItem} from 'src/app/shared/components/flex';

interface CompanyServicesItemProps {
    service: ServicePublicResourceModel;
    onClick: (id: string) => void;
}

const CompanyServicesItem: React.FunctionComponent<CompanyServicesItemProps> = (
    {
        service,
        onClick
    }) => {
    const style = {borderBottom: '1px solid #A1A1A1'};

    return (
        <div className="c-pointer mb-4" style={style} onClick={() => onClick(service.id)}>
            <KFlexRow>
                <KFlexRowItem basisPercent={50}>
                    <div className="h6">
                        {service.name}
                    </div>
                </KFlexRowItem>
                <KFlexRowItem>
                    <div className="c-primary text-right">{service.price}</div>
                    <div className="tiny light-grey text-right">ETA: {service.duration}</div>
                </KFlexRowItem>
            </KFlexRow>
        </div>
    )
}


export default CompanyServicesItem;
