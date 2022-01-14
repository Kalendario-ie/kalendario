import {PaymentIntentResult} from './models';
import baseApiAxios from 'src/app/api/common/clients/base-api';


const billingUrl = 'billing/';

export const billingClient = {
    payment(requestId: string): Promise<PaymentIntentResult> {
        return baseApiAxios.put<PaymentIntentResult>(billingUrl + `payment/${requestId}/`,)
            .then(res => res.data);
    }
}
  
