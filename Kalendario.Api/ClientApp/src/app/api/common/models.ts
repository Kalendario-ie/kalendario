import {TimeSpan} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';

export interface IReadModel {
    id: number | string; // todo: change to string only
}

export interface Person extends IReadModel {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
}

export function modelId(model: IReadModel) {
    if (model) {
        return model.id;
    }
    return null;
}


function stringfy(value: number): string {
    if (value < 10) {
        return '0' + value.toString();
    }
    return value.toString();
}

export interface TimeOfDay {
    hour: number;
    minute: number;
}

export const Zero = (): TimeSpan => ({
    days: 0,
    milliseconds: 0,
    seconds: 0,
    ticks: 0,
    totalDays: 0,
    totalHours: 0,
    totalMilliseconds: 0,
    totalMinutes: 0,
    totalSeconds: 0,
    hours: 0,
    minutes: 0
});

export const timeFromString = (time: string): TimeSpan => {
    const hours = +time.substr(0, 2);
    const minutes = +time.substr(3, 2);
    return {...Zero(), hours, minutes};
}

export const timeToString = (t: TimeSpan) => stringfy(t.hours) + ':' + stringfy(t.minutes);

export const timeToISOString = (t: TimeSpan) => stringfy(t.hours) + ':' + stringfy(t.minutes) + ':00';

// hashCode(): number {
//     return this.hour + this.minute / 60;
// }
