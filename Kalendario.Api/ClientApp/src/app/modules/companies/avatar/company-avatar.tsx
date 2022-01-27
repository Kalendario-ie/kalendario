import React from 'react';
import {CompanyDetailsResourceModel} from 'src/app/api/api';
import { KFlexRow } from 'src/app/shared/components/flex';
import AvatarImg from 'src/app/shared/components/primitives/avatar-img';

interface CompanyAvatarProps {
    company: CompanyDetailsResourceModel;
}

const CompanyAvatar: React.FunctionComponent<CompanyAvatarProps> = (
    {company}) => {
    return (
        <KFlexRow align="center">
            <AvatarImg src={company.avatar || ''} />
            <div className="ml-2">
                <h3 className="pb-0 c-pointer">{company.name}</h3>
                <div className="c-accent">
                    {/*{company.address}*/} // todo here
                </div>
            </div>
        </KFlexRow>
    );
}

export default CompanyAvatar;
