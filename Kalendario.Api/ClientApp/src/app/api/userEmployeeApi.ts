import {UserEmployeeClient} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';

export const userEmployeeClient = new UserEmployeeClient('', baseApiAxios);

