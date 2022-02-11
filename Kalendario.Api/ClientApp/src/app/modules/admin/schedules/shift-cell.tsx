import React from 'react';
import {frameName} from 'src/app/api/adminSchedulesApi';
import {ScheduleFrameResourceModel} from 'src/app/api/api';
import {KFlexColumn} from 'src/app/shared/components/flex';

interface ShiftCellProps {
    frames: ScheduleFrameResourceModel[];
}

const ShiftCell: React.FunctionComponent<ShiftCellProps> = (
    {
        frames
    }) => {
    return (
        <KFlexColumn>
            {frames.map((frame, key) => <div key={key}>{frameName(frame)}</div>)}
        </KFlexColumn>
    )
}


export default ShiftCell;
