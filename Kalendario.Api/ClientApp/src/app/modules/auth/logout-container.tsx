import React, {useEffect} from 'react';
import {FormattedMessage} from 'react-intl';
import {useDispatch} from 'react-redux';
import {authApi} from 'src/app/api/auth/clients';
import {KFlexRow} from 'src/app/shared/components/flex';
import { KCard, KPageContainer } from 'src/app/shared/components/primitives/containers';
import {setUser} from 'src/app/store/auth';


const LogoutContainer: React.FunctionComponent = () => {
    const dispatch = useDispatch();
    useEffect(() => {
        authApi.logout().then(() => {
            dispatch(setUser(null));
        });
    }, [dispatch]);

    return (
        <KPageContainer>
            <KFlexRow align={'center'} justify={'center'}>
                <KCard maxWidth={400}>
                    <FormattedMessage id={'AUTH.LOGOUT-SUCCESS'}/>
                </KCard>
            </KFlexRow>
        </KPageContainer>
    )
}


export default LogoutContainer;
