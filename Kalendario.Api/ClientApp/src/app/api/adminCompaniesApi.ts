import {CancelToken} from 'axios';
import {AccountsClient, CreateAccountCommand} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {companyConfigParser} from 'src/app/api/company-config/parsers';

const client = new AccountsClient('', baseApiAxios);


export const adminCompanyClient = {
    post(body: CreateAccountCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.accountsCreate(body, cancelToken);
    }
    // ...baseModelRequest<AdminCompany, null>(adminUrl, adminCompanyParser),
    //
    // config(id: number, model: CompanyConfig): Promise<CompanyConfig> {
    //     return baseApiAxios.patch<CompanyConfig>(adminUrl + id + '/config/', model)
    //         .then(result => companyConfigParser(result.data));
    // },
    //
    // stripeUrl(id: number): Promise<{ url: string }> {
    //     return baseApiAxios.post<{ url: string }>(`${stripeUrl}${id}/connect/`, {})
    //         .then(result => result.data);
    // }
}

//
//   uploadProfilePicture(id: number, file: File): Observable<Company> {
//     const formData = new FormData();
//     formData.append('image', file);
//     return this.http.patch<Company>(this.baseUrl + id + '/photo/', formData)
//       .pipe(map(this.adapter.adapt));
//   }
//

//
//   stripeDetails(id: number): Observable<CompanyStripeDetails> {
//     return this.http.get<CompanyStripeDetails>(`${this.stripe}${id}/`).pipe(map(this.stripeAdapter.adapt));
//   }
// }



export function adminCompanyParser(data: any): CreateAccountCommand {
    data = typeof data === 'object' ? data : {};
    return {
        ...data,
        permissionModel: PermissionModel.company,
        config: data?.config ? companyConfigParser(data.config) : null
    }
}
