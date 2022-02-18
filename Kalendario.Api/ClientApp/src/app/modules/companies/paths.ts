import {CompanyDetailsResourceModel} from 'src/app/api/api';
import {pathWithParams} from 'src/app/shared/util/router-extensions';


export const companiesUrls = (company: CompanyDetailsResourceModel) => {
    return {
        index: `/c/${company.name}`,
        cart: `/c/${company.name}/cart`,
        book: (params: CreateAppointmentRequest) => pathWithParams(`/c/${company.name}/book`, {...params}),
        checkout: `/c/${company.name}/checkout`,
    }
}

export const COMPANY_URLS = {
    CREATE: '/c/create',
}
