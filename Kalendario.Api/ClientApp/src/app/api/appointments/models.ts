import {Moment} from 'moment';
import {HistoryType} from 'src/app/api/common/HistoryType';
import {IReadModel} from 'src/app/api/common/models';
import {Company} from 'src/app/api/companies';
import {Employee} from 'src/app/api/employees/models';
import {AdminUser} from 'src/app/api/users/models';

export enum EventType {
    EmployeeEvent,
    CustomerAppointment,
    CustomerRequestAppointment,
    CustomerEvent,
}

export type Appointment = CustomerEvent | CustomerRequestAppointment | CustomerAppointment | EmployeeEvent;

export interface CustomerEvent extends CustomerRequestAppointment {
    owner: Company;
}

export interface CustomerRequestAppointment extends CustomerAppointment {
    request: number;
    status: string;
    customerNotes: string;
    companyName: string;
    owner: number | Company;
}

export interface CustomerAppointment extends BaseAppointment {
    customer: null;
    service: number;
}

export interface EmployeeEvent extends BaseAppointment {
    customer: null;
    service: null;
}

export interface BaseAppointment extends IReadModel {
    customer: null;
    service: number | null;
    type: EventType;
    start: string;
    end: string;
    employee: Employee;
    owner: number | Company;
    internalNotes: string;
    customerNotes: string | null;
    deleted: string | null;
}

export interface History {
    historyType: HistoryType;
    historyDate: Moment | null;
    historyUser: AdminUser | null;
}

export type AppointmentHistory = Appointment & History;

