import {SchedulesClient} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import {SchedulingPanel} from 'src/app/api/scheduling-panels/models';
import {schedulingPanelParser} from 'src/app/api/scheduling-panels/parsers';

const baseUrl = 'admin/panels/';
const client = new SchedulesClient(baseUrl, baseApiAxios);

// TODO: Fix Here.
export const adminSchedulingPanelsClient: BaseModelRequest<SchedulingPanel, SchedulingPanel> = {
    get: search => Promise.resolve({entities: []}),
    put: (id, command) => Promise.resolve(schedulingPanelParser(null)),
    post: body => Promise.resolve(schedulingPanelParser(null))
}
