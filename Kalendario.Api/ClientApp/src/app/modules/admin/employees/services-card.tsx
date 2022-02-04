import React, {useEffect} from 'react';
import {FormattedMessage} from 'react-intl';
import {KCard, KTreeView} from 'src/app/shared/components/primitives/containers';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {serviceCategoryActions, useInitializeServiceCategories} from 'src/app/store/admin/serviceCategories';
import {
    selectServicesWithCategoriesByIds,
    serviceActions,
    serviceSelectors,
    useInitializeServices
} from 'src/app/store/admin/services';

interface ServicesCardProps {
    serviceIds: string[];
}

const ServicesCard: React.FunctionComponent<ServicesCardProps> = (
    {
        serviceIds
    }) => {
    const categories = useAppSelector((state: any) =>
        selectServicesWithCategoriesByIds(state, serviceIds))

    useInitializeServices();
    useInitializeServiceCategories();

    return (
        <KCard
            header={<FormattedMessage id="ADMIN.COMMON.SERVICES"/>}
            maxWidth={500}
            maxHeight={30}
            mhUnit={'vh'}
            hasShadow={false}
        >
            <KTreeView
                items={categories}
                renderComponent={(props => <>{props.name}</>)}
            />
        </KCard>
    )
}


export default ServicesCard;
