import {Moment} from 'moment';
import {
    CreateScheduleFrame,
    ScheduleAdminResourceModel,
    ScheduleFrameAdminResourceModel,
    UpsertScheduleCommand
} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import {timeToISOString} from 'src/app/api/common/models';
import {Schedule} from 'src/app/api/schedule/models';
import {UpsertScheduleRequest, UpsertScheduleRequestShift} from 'src/app/api/schedule/requests';
import {Shift, shiftParser} from 'src/app/api/shifts';

export function scheduleParser(data: any): Schedule {
    return {
        ...data,
        permissionModel: PermissionModel.schedule,
        mon: shiftParser(data.mon),
        tue: shiftParser(data.tue),
        wed: shiftParser(data.wed),
        thu: shiftParser(data.thu),
        fri: shiftParser(data.fri),
        sat: shiftParser(data.sat),
        sun: shiftParser(data.sun),
        shifts: ['mon', 'tue', 'wed', 'thu', 'fri', 'sat', 'sun'],
    }
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

function shiftToUpsertShift(frames: ScheduleFrameAdminResourceModel[]): CreateScheduleFrame[] {
    return frames;
}

export function upsertScheduleRequestParser(schedule: ScheduleAdminResourceModel | null): UpsertScheduleCommand {
    return schedule ? {
        name: schedule.name,
        monday: shiftToUpsertShift(schedule.monday),
        tuesday: shiftToUpsertShift(schedule.tuesday),
        wednesday: shiftToUpsertShift(schedule.wednesday),
        thursday: shiftToUpsertShift(schedule.thursday),
        friday: shiftToUpsertShift(schedule.friday),
        saturday: shiftToUpsertShift(schedule.saturday),
        sunday: shiftToUpsertShift(schedule.sunday)

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
