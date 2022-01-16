import {Profile} from 'oidc-client';
import React, {useEffect} from 'react';
import {useDispatch} from 'react-redux';
import {hasPermission, PermissionModel, PermissionType} from 'src/app/api/auth';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {useAppSelector} from 'src/app/store';
import {selectLoadingUser, selectUser, setLoadingUser, setUser} from 'src/app/store/auth';
import {AuthorizeService} from 'src/components/api-authorization/AuthorizeService';

interface AuthAutoLoginProps {
    children: React.ReactNode;
}

const authService = new AuthorizeService();

const AuthAutoLogin: React.FunctionComponent<AuthAutoLoginProps> = (
    {children}
) => {
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(setLoadingUser(true));
        authService
            .getUser()
            .then((user) => {
                if (!!user) {
                    dispatch(setUser(user.profile));
                    authService.getAccessToken().then(token => {
                        baseApiAxios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
                    });
                }
            });
    }, [dispatch]);

    return (
        <>
            {children}
        </>
    )
}

export default AuthAutoLogin;


export function useCurrentUser(): [boolean, Profile | null] {
    const user = useAppSelector(selectUser);
    const loading = useAppSelector(selectLoadingUser);
    return [loading, user];
}

export function useUserHasPermission(type: PermissionType, model?: PermissionModel): boolean {
    const user = useAppSelector(selectUser);
    if (!model) return true;
    return !!user && hasPermission(user, type, model);
}
