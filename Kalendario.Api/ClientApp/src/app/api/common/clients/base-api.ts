import axios from 'axios';
import {setupAuthHandlers} from './common-api';

const baseApiAxios = axios.create({});

export const  configureBaseApi = () => {
    baseApiAxios.defaults.transformResponse = (data) => data;
    setupAuthHandlers(baseApiAxios);
}

export default baseApiAxios;
