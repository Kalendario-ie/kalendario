import {Moment} from 'moment';
import {
    ScheduleAdminResourceModel,
    ScheduleFrameAdminResourceModel,
    SchedulesClient,
    UpsertScheduleCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';

const baseUrl = 'admin/schedules/';
const client = new SchedulesClient(baseUrl, baseApiAxios);

export const adminScheduleClient: BaseModelRequest<ScheduleAdminResourceModel, UpsertScheduleCommand> = {
    get: client.schedulesGet,
    post: client.schedulesPost,
    put: client.schedulesPut
}

export const isAvailable = (frame: ScheduleFrameAdminResourceModel, hour: number, minute: number): boolean => {
    const start = frame.start.hour + frame.start.minute / 60;
    const end = frame.end.hour + frame.end.minute / 60;
    const value = hour + minute / 60;
    return start <= value && end > value;
}

export function getShift(schedule: ScheduleAdminResourceModel, date: Moment): ScheduleFrameAdminResourceModel[] {
    switch (date.isoWeekday()) {
        case 1:
            return schedule.monday;
        case 2:
            return schedule.tuesday;
        case 3:
            return schedule.wednesday;
        case 4:
            return schedule.thursday;
        case 5:
            return schedule.friday;
        case 6:
            return schedule.saturday;
        case 7:
            return schedule.sunday;
    }
    return schedule.sunday;
}

export function frameName(scheduleFrame: ScheduleFrameAdminResourceModel){
    return `${scheduleFrame.start.hour}:${scheduleFrame.start.minute} - ${scheduleFrame.end.hour}:${scheduleFrame.end.minute}`;
}
