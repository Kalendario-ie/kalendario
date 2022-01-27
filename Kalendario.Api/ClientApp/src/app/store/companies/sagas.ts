import moment, {Moment} from 'moment';
import {isMobile} from 'react-device-detect';
import {call, put, select, takeEvery} from 'redux-saga/effects'
import {CompanyDetailsResourceModel} from 'src/app/api/api';
import {ApiBaseError} from 'src/app/api/common/api-errors';
import {companyClient} from 'src/app/api/publicCompanyApi';
import {selectLoggedIn} from 'src/app/store/auth';
import {selectSelectedDate, selectSelectedServiceId} from 'src/app/store/companies/selectors';
import {
    companyDetailsRequestFail,
    companyDetailsRequestSuccess,
    currentCartRequest,
    currentCartRequestFail,
    deleteAppointmentRequestFail,
    setSelectedDate,
} from './actions';
import {ACTION_TYPES} from './types';


function* requestCompanyDetails(action: { type: string, payload: string }) {
    try {
        const company: CompanyDetailsResourceModel = yield call([companyClient, companyClient.companiesFind], action.payload)
        yield put(companyDetailsRequestSuccess(company));
    } catch (error) {
        yield put(companyDetailsRequestFail(error as ApiBaseError));
    }
}


function* triggerCartRequestIfLoggedIn(action: { type: string, payload: CompanyDetailsResourceModel }) {
    const isLoggedIn: boolean = yield select(selectLoggedIn);
    if (isLoggedIn) {
        yield put(currentCartRequest(action.payload.id))
    }
}


function* requestCartForCompany(action: { type: string, payload: number }) {
    try {
        // const request: RequestModel = yield call(companyRequestClient.current, action.payload); //TODO: FIX HERE
        // yield put(currentCartRequestSuccess());
        // yield put(setCurrentRequest(request));

    } catch (error) {
        yield put(currentCartRequestFail(error as ApiBaseError));
    }
}


function* triggerSlotRequest(action: { type: string, payload: number }) {
    const service: number = yield select(selectSelectedServiceId);
    if (service) {
        const start: Moment = yield select(selectSelectedDate);
        const end = start.clone().add(isMobile ? 0 : 1, 'day').endOf('day');
        // yield put(slotsRequest({start, end, service})); // TODO HERE.
    }
}


// function* requestSlots(action: { type: string, payload: SlotRequestParams }) {
//     try {
//         const slots: Slot[] = yield call(companyClient.slots, action.payload);
//         yield put(slotsRequestSuccess(slots));
//     } catch (error) {
//         yield put(slotsRequestFail());
//     }
// }


function* addOneDayToSelectedDate(action: { type: string, payload: number }) {
    const selectedDate: Moment = yield select(selectSelectedDate);
    yield put(setSelectedDate(selectedDate.clone().add(1, 'day')))
}


function* subtractOneDayToSelectedDate(action: { type: string, payload: number }) {
    const selectedDate: Moment = yield select(selectSelectedDate);
    yield put(setSelectedDate(selectedDate.clone().subtract(1, 'day')))
}


function* updateSelectedDate(action: { type: string, payload: number }) {
    yield put(setSelectedDate(moment.utc().startOf('day')));
}


// function* requestAddAppointment(action: { type: string, payload: CreateAppointmentRequest }) {
//     try {
//         const request: RequestModel = yield call(companyRequestClient.createAppointment, action.payload)
//         yield put(bookSlotRequestSuccess());
//         yield put(setCurrentRequest(request));
//     } catch (error) {
//         yield put(bookSlotRequestFail());
//     }
// }


function* requestRemoveAppointment(action: { type: string, payload: number }) {
    try {
        // const request: RequestModel = yield call(companyRequestClient.delete, action.payload)
        // yield put(deleteAppointmentRequestSuccess());
        // yield put(setCurrentRequest(request));
    } catch (error) {
        yield put(deleteAppointmentRequestFail(error as ApiBaseError));
    }
}


// function* requestAddNotes(action: { type: string, payload: AddNotesRequest }) {
//     try {
//         const request: RequestModel = yield call(companyRequestClient.patch, action.payload)
//         yield put(addNotesRequestSuccess());
//         yield put(setCurrentRequest(request));
//     } catch (error) {
//         yield put(addNotesRequestFail(error as ApiBaseError));
//     }
// }

//
// function* requestCartConfirmation(action: { type: string, payload: number }) {
//     try {
//         yield call(companyRequestClient.complete, action.payload);
//         yield put(confirmCartRequestSuccess());
//         yield put(setCurrentRequest(null));
//     } catch (error) {
//         yield put(confirmCartRequestFail(error as ApiBaseError));
//     }
// }


export function* companiesSaga() {
    yield takeEvery(ACTION_TYPES.COMPANY_DETAILS_REQUEST, requestCompanyDetails);
    yield takeEvery(ACTION_TYPES.COMPANY_DETAILS_REQUEST_SUCCESS, triggerCartRequestIfLoggedIn);
    yield takeEvery(ACTION_TYPES.CURRENT_CART_REQUEST, requestCartForCompany);
    yield takeEvery(ACTION_TYPES.SET_SELECTED_SERVICE_ID, triggerSlotRequest);
    yield takeEvery(ACTION_TYPES.SET_SELECTED_DATE, triggerSlotRequest);
    // yield takeEvery(ACTION_TYPES.SLOTS_REQUEST, requestSlots);
    yield takeEvery(ACTION_TYPES.SELECTED_DATE_ADD_ONE, addOneDayToSelectedDate);
    yield takeEvery(ACTION_TYPES.SELECTED_DATE_SUBTRACT_ONE, subtractOneDayToSelectedDate);
    yield takeEvery(ACTION_TYPES.SELECTED_DATE_TODAY, updateSelectedDate);
    // yield takeEvery(ACTION_TYPES.BOOK_SLOT_REQUEST, requestAddAppointment);
    yield takeEvery(ACTION_TYPES.DELETE_APPOINTMENT_REQUEST, requestRemoveAppointment);
    // yield takeEvery(ACTION_TYPES.ADD_NOTES_REQUEST, requestAddNotes);
    // yield takeEvery(ACTION_TYPES.CONFIRM_CART_REQUEST, requestCartConfirmation);
}
