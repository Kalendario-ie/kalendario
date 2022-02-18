import {createSelector} from '@reduxjs/toolkit';
import moment, {Moment} from 'moment';
import {CompanyDetailsResourceModel} from 'src/app/api/api';
import {RequestModel} from 'src/app/api/requests';
import {RootState} from 'src/app/store/store';


export const selectCompany: (rootState: RootState) => CompanyDetailsResourceModel | null =
    (rootState) => rootState.companies.company;


export const selectSelectedDate: (rootState: RootState) => Moment =
    (rootState) => moment.utc(rootState.companies.selectedDate);


export const selectCurrentRequest: (rootState: RootState) => RequestModel | null =
    (rootState) => rootState.companies.currentRequest;


export const selectSelectedServiceId: (rootState: RootState) => string | null =
    (rootState) => rootState.companies.selectedServiceId;


export const selectCompanyRequestCompleted: (rootState: RootState) => boolean =
    (rootState) => rootState.companies.companyRequestCompleted;


export const selectCurrentRequestCompleted: (rootState: RootState) => boolean =
    (rootState) => rootState.companies.currentRequestCompleted;


const selectServices = createSelector(
    [selectCompany],
    (company) =>
        company?.services
)


export const selectService = createSelector(
    [selectServices, selectSelectedServiceId],
    (services, id) =>
        services && services.find(service => service.id === id)
)

export const selectIsStoreReady = createSelector(
    [selectCompanyRequestCompleted, selectCurrentRequestCompleted],
    (company, request) => company && request

)

export const selectCartIsEmpty = createSelector(
    [selectCurrentRequest],
    (request) => !!request && request.itemsCount === 0
)

export const selectCartIsLoadedAndEmpty = createSelector(
    [selectCartIsEmpty, selectIsStoreReady],
    (emptyCart, storeReady) => storeReady && emptyCart
)
