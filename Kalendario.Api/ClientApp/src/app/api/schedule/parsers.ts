import {Moment} from 'moment';
import {
    CreateScheduleFrame,
    ScheduleAdminResourceModel,
    ScheduleFrameAdminResourceModel,
    UpsertScheduleCommand
} from 'src/app/api/api';


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
