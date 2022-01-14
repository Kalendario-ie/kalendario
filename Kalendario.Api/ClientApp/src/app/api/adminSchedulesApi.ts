import {CancelToken} from 'axios';
import {Moment} from 'moment';
import {
    CreateScheduleFrame,
    ScheduleAdminResourceModel,
    ScheduleFrameAdminResourceModel,
    SchedulesClient,
    UpsertScheduleCommand,
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest, BaseQueryParams} from 'src/app/api/common/clients/base-django-api';

const client = new SchedulesClient('', baseApiAxios);

export const adminScheduleClient: BaseModelRequest<ScheduleAdminResourceModel, UpsertScheduleCommand, BaseQueryParams> = {
    get(params) {
        return client.schedulesGet(params?.search, params?.start, params?.length, params?.cancelToken);
    },
    post(body: UpsertScheduleCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.schedulesPost(body, cancelToken);
    },
    put(id: string, command: UpsertScheduleCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.schedulesPut(id, command, cancelToken);
    }
}


function framesToCreateScheduleFrame(frames: ScheduleFrameAdminResourceModel[]): CreateScheduleFrame[] {
    return frames;
}

export function upsertScheduleCommandParser(schedule: ScheduleAdminResourceModel | null): UpsertScheduleCommand {
    return schedule ? {
        name: schedule.name,
        monday: framesToCreateScheduleFrame(schedule.monday),
        tuesday: framesToCreateScheduleFrame(schedule.tuesday),
        wednesday: framesToCreateScheduleFrame(schedule.wednesday),
        thursday: framesToCreateScheduleFrame(schedule.thursday),
        friday: framesToCreateScheduleFrame(schedule.friday),
        saturday: framesToCreateScheduleFrame(schedule.saturday),
        sunday: framesToCreateScheduleFrame(schedule.sunday)

    } : {
        name: '',
        monday: [],
        tuesday: [],
        wednesday: [],
        thursday: [],
        friday: [],
        saturday: [],
        sunday: []
    }
}

export const stringToTime = (value: string): { hour: number, minute: number } => {
    return value ? {hour: +value.substring(0, 2), minute: +value.substring(3, 5)} : {hour: 0, minute: 0};
}

export const isAvailable = (frame: ScheduleFrameAdminResourceModel, hour: number, minute: number): boolean => {
    const startTime = stringToTime(frame.start);
    const endTime = stringToTime(frame.end);

    const start = startTime.hour + startTime.minute / 60;
    const end = endTime.hour + endTime.minute / 60;
    const value = hour + minute / 60;
    return start <= value && end > value;
}

export function getFramesForDate(schedule: ScheduleAdminResourceModel, date: Moment): ScheduleFrameAdminResourceModel[] {
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

export function frameName(scheduleFrame: ScheduleFrameAdminResourceModel) {
    return `${scheduleFrame.start} - ${scheduleFrame.end}`;
}
