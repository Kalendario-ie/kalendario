import baseApiAxios from 'src/app/api/common/clients/base-api';
import {AddNotesRequest, CreateAppointmentRequest, SlotRequestParams} from 'src/app/api/companies/requests';
import {RequestModel, requestParser} from 'src/app/api/requests';
import {convertMoment} from '../common/helpers';
import {CompanyDetails, Slot} from './models';
import {companyDetailsParser, slotParser} from './parsers';


const baseUrl = 'companies/';

export const companyClient = {
    // ...baseModelRequest(baseUrl, companyParser), //TODO: FIX LATER
    fromName: (name: string): Promise<CompanyDetails> => {
        return baseApiAxios.get<CompanyDetails>(baseUrl + name + '/')
            .then(result => companyDetailsParser(result.data));
    },

    slots: (slotsParams: SlotRequestParams): Promise<Slot[] | null> => {
        const params = convertMoment(slotsParams);
        return baseApiAxios.get<Slot[]>(baseUrl + 'slots/', {params})
            .then(result => result.data.map((slot, id) => slotParser(id, slot)))
            .catch(error => null);
    }
};

// const billingUrl = 'billing/';
const requestsUrl = 'requests/';

export const companyRequestClient = {
    // ...baseModelRequest<RequestModel>(requestsUrl, requestParser), //TODO: Fix Later

    createAppointment(data: CreateAppointmentRequest) {
        convertMoment(data);
        return baseApiAxios.post<RequestModel>(requestsUrl + 'add/', data)
            .then(result => requestParser(result.data));
    },

    patch(request: AddNotesRequest) {
        return baseApiAxios.patch<RequestModel>(`${requestsUrl}${request.id}/`, {customerNotes: request.notes})
            .then(result => requestParser(result.data));
    },

    complete(id: number) {
        return baseApiAxios.patch<RequestModel>(requestsUrl + id + '/confirm/', {})
            .then(result => requestParser(result.data));
    },

    delete(appointmentId: number) {
        return baseApiAxios.post<RequestModel>(requestsUrl + 'delete/', {appointment: appointmentId})
            .then(result => requestParser(result.data));
    },

    current(owner: number): Promise<RequestModel> {
        return baseApiAxios.get<RequestModel>(requestsUrl + 'current/', {params: {owner}}).then(
            result => requestParser(result.data)
        );
    },
}
  
