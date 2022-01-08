import {TimeSpan} from 'src/app/api/api';

export interface TimeFrame {
    start: TimeSpan;
    end: TimeSpan;
    name: string;
}

export interface Shift {
    frames: TimeFrame[];
    name: string;
}




