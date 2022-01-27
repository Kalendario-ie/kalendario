import {CompaniesClient} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';

export const companyClient = new CompaniesClient('', baseApiAxios);

