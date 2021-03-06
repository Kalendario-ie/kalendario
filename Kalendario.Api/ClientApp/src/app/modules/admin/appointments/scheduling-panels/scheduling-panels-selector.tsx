import React, {useEffect, useState} from 'react';
import {adminSchedulingPanelsClient} from 'src/app/api/adminSchedulingPanels';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import SchedulingPanelForm from 'src/app/modules/admin/appointments/scheduling-panels/scheduling-panel-form';
import AdminButton from 'src/app/shared/admin/admin-button';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {KFlexRow} from 'src/app/shared/components/flex';
import {useConfirmationModal} from 'src/app/shared/components/modal/confirmation-modal';
import {KTextButton} from 'src/app/shared/components/primitives/buttons';
import {useKHistory, useQueryParams} from 'src/app/shared/util/router-extensions';
import {useAppDispatch} from 'src/app/store';
import {adminDashboardActions} from 'src/app/store/admin/dashboard';
import {panelActions, useInitializePanels} from 'src/app/store/admin/panels';


const SchedulingPanelsSelector: React.FunctionComponent = () => {
    const dispatch = useAppDispatch();
    const [, schedulingPanels] = useInitializePanels();
    const [selectedIndex, setSelectedIndex] = useState<number>(0);
    const [setDeleteId, confirmDeleteModal] = useConfirmationModal(id => adminSchedulingPanelsClient.delete(id),
        () => dispatch(panelActions.removeOne(schedulingPanels[selectedIndex].id)));
    const [openModal, formModal] = useEditModal(panelActions, SchedulingPanelForm);
    const params = useQueryParams();
    const history = useKHistory();

    useEffect(() => {
        let panelId = +(params['panel'] || selectedIndex);
        if (schedulingPanels.length > 0) {
            if (panelId >= schedulingPanels.length) {
                panelId = 0;
            }
            setSelectedIndex(panelId);
            dispatch(adminDashboardActions.setSelectedPanelId(schedulingPanels[panelId].id));
        }
    }, [dispatch, params, schedulingPanels, selectedIndex]);


    const handlePanelClick = (index: number) => () => {
        history.push(history.location.pathname, {...params, panel: index})
    }

    const handleDeleteClick = () => {
        setDeleteId(schedulingPanels[selectedIndex].id);
    }

    return (
        <KFlexRow className="sticky-top-left" align={'center'}>
            {confirmDeleteModal}
            {formModal}
            {schedulingPanels.map((panel, i) =>
                <KTextButton key={i}
                             className={selectedIndex === i ? 'underline-accent' : ''}
                             onClick={handlePanelClick(i)}
                >{panel.name}</KTextButton>)
            }
            <AdminButton type={PermissionType.add} model={PermissionModel.schedulePanel} onClick={() => openModal(null)}/>
            <AdminButton type={PermissionType.change} model={PermissionModel.schedulePanel} onClick={() => openModal(schedulingPanels[selectedIndex])}/>
            <AdminButton type={PermissionType.delete} model={PermissionModel.schedulePanel} onClick={handleDeleteClick}/>
        </KFlexRow>
    )
}


export default SchedulingPanelsSelector;
