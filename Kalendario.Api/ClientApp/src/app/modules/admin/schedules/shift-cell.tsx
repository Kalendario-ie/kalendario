import React from 'react';
import {frameName} from 'src/app/api/adminSchedulesApi';
import {ScheduleFrameAdminResourceModel} from 'src/app/api/api';
import {KFlexColumn} from 'src/app/shared/components/flex';

interface ShiftCellProps {
    frames: ScheduleFrameAdminResourceModel[];
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
