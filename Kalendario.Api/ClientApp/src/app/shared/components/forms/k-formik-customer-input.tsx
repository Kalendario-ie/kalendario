import {useFormikContext} from 'formik';
import React, {useState} from 'react';
import AsyncSelect from 'react-select/async';
import {FormGroup, Label} from 'reactstrap';
import {adminCustomerClient} from 'src/app/api/adminCustomerApi';
import {CustomerAdminResourceModel} from 'src/app/api/api';
import {KFlexColumn, KFlexRow, KFlexSpacer} from 'src/app/shared/components/flex';
import KIcon from 'src/app/shared/components/primitives/k-icon';
import {useAppDispatch} from 'src/app/store';

interface FormikCustomerInput {
    initialCustomer: CustomerAdminResourceModel | null;
}

export const KFormikCustomerInput: React.FunctionComponent<FormikCustomerInput> = ({initialCustomer}) => {
    const [customer, setCustomer] = useState<CustomerAdminResourceModel | null>(initialCustomer);
    // const [openModal, modal, createdCustomer] = useEditModal(customerSelectors, customerActions, CustomerUpsertForm);
    const dispatch = useAppDispatch();
    const formik = useFormikContext();
    const {setValue} = formik.getFieldHelpers('customer');

    // useEffect(() => {
    //     if (createdCustomer) {
    //         setCustomer(createdCustomer);
    //         setValue(createdCustomer.id);
    //     }
    //     return () => {
    //         dispatch(customerSlice.actions.setCreatedEntityId(null))
    //     }
    //     // the below is disabled because setValue will cause an infinite loop if added to deps.
    //     // eslint-disable-next-line react-hooks/exhaustive-deps
    // }, [createdCustomer]);

    const promiseOptions = (search: string) => adminCustomerClient.get(search, 0, 200).then(res => res.entities!);

    const navigateToPage = (selectedCustomer: CustomerAdminResourceModel | null) => {
        setCustomer(selectedCustomer);
        setValue(selectedCustomer?.id || null);
    }


    return (
        <FormGroup>
            {/*{modal}*/}
            <Label>Customer</Label>
            <FormGroup>
                <KFlexRow align={'center'}>
                    <AsyncSelect className={"flex-fill"}
                                 cacheOptions
                                 backspaceRemovesValue
                                 defaultInputValue={initialCustomer?.name}
                                 getOptionValue={(option) => option.id.toString()}
                                 getOptionLabel={(option) => option.name}
                                 formatOptionLabel={(option) =>
                                     <KFlexColumn justify={'between'} className={option.warning ? 'bg-danger' : ''}>
                                         <span className="font-bold">{option.name}</span> {option.email}
                                     </KFlexColumn>
                                 }
                                 onChange={navigateToPage}
                                 loadOptions={promiseOptions}/>
                    {/*<KIconButton color="primary" icon={'plus'} onClick={openModal(null)}/>*/}
                </KFlexRow>
            </FormGroup>

            {customer &&
            <KFlexColumn>
                <KFlexRow justify={'between'}>
                    <KIcon icon="user" color="primary" text={customer.name}/>
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
