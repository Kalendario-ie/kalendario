import {useFormikContext} from 'formik';
import React, {useEffect, useState} from 'react';
import AsyncSelect from 'react-select/async';
import {FormGroup, Label} from 'reactstrap';
import {adminCustomerClient} from 'src/app/api/adminCustomerApi';
import {CustomerAdminResourceModel} from 'src/app/api/api';
import CustomerUpsertForm from 'src/app/modules/admin/customers/customer-upsert-form';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {KFlexColumn, KFlexRow, KFlexSpacer} from 'src/app/shared/components/flex';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';
import KIcon from 'src/app/shared/components/primitives/k-icon';
import {customerActions} from 'src/app/store/admin/customers';

interface FormikCustomerInput {
    initialCustomer: CustomerAdminResourceModel | undefined;
}

export const KFormikCustomerInput: React.FunctionComponent<FormikCustomerInput> = ({initialCustomer}) => {
    const [customer, setCustomer] = useState<CustomerAdminResourceModel | undefined>(initialCustomer);
    const [openModal, formModal, createdCustomer] = useEditModal(customerActions, CustomerUpsertForm);
    const formik = useFormikContext();
    const {setValue} = formik.getFieldHelpers('customerId');

    useEffect(() => {
        if (createdCustomer) {
            setCustomer(createdCustomer);
            setValue(createdCustomer.id);
        }
        return () => {
        }
        // the below is disabled because setValue will cause an infinite loop if added to deps.
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [createdCustomer]);

    const promiseOptions = (search: string) => adminCustomerClient.get({
        search,
        start: 0,
        length: 200
    }).then(res => res.entities!);

    const onChange = (selectedCustomer: CustomerAdminResourceModel | null) => {
        setCustomer(selectedCustomer || undefined);
        setValue(selectedCustomer?.id || null);
    }


    return (
        <FormGroup>
            {formModal}
            <Label>Customer</Label>
            <FormGroup>
                <KFlexRow align={'center'}>
                    <AsyncSelect className={"flex-fill"}
                                 cacheOptions
                                 backspaceRemovesValue
                                 value={customer}
                                 getOptionValue={(option) => option.id.toString()}
                                 getOptionLabel={(option) => option.name}
                                 formatOptionLabel={(option) =>
                                     <KFlexColumn justify={'between'} className={option.warning ? 'bg-danger' : ''}>
                                         <span className="font-bold">{option.name}</span> {option.email}
                                     </KFlexColumn>
                                 }
                                 onChange={onChange}
                                 loadOptions={promiseOptions}/>
                    <KIconButton color="primary" icon={'plus'} onClick={() => openModal(null)}/>
                </KFlexRow>
            </FormGroup>

            {customer &&
            <KFlexColumn>
                <KFlexRow justify={'between'}>
                    <KIcon icon="phone" color="primary" text={customer.phoneNumber}/>
                    <KIcon icon="at" color="primary" text={customer.email}/>
                </KFlexRow>
                <KFlexSpacer size={0.4}/>
                <KFlexRow>
                    {customer.warning &&
                    <KIcon icon="exclamation" color="danger" text={customer.warning}/>
                    }
                </KFlexRow>
            </KFlexColumn>
            }
        </FormGroup>
    )
}
