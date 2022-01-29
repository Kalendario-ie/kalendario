import moment from 'moment';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import {IReadModel} from 'src/app/api/common/models';


export const compareByName = (a: IReadModel, b: IReadModel): number => {
    return a.name.localeCompare(b.name);
}


export const compareByStartDate = (a: AppointmentAdminResourceModel, b: AppointmentAdminResourceModel): number => {
    return moment.utc(a.start).diff(moment.utc(b.start));
}
