import {Moment} from 'moment';
import {IReadModel} from '../common/models';
import {CompanyConfig} from '../company-config/models';
import {Employee} from '../employees/models';

export interface Company extends IReadModel {
    id: string;
    name: string;
    address: string;
    avatar: string;
    about: string;
    instagram: string;
    phoneNumber: string;
    whatsapp: string;
    facebook: string;
    config: CompanyConfig;
}

export interface CompanyDetails extends Company {
    employees: Employee[];
    services: number[]; // todo public service RM
    serviceCategories: number[]; // todo public service cat
    config: CompanyConfig;
}

export interface Slot {
    id: number;
    date: string;
    start: Moment;
    end: Moment;
    title: string;
}
