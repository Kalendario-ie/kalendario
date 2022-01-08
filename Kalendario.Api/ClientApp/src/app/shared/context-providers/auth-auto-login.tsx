import React, {useEffect} from 'react';
import {useDispatch} from 'react-redux';
import {AuthUser, hasPermission, PermissionModel, PermissionType} from 'src/app/api/auth';
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
            .then((result) => dispatch(setUser(result)));
    }, [dispatch]);

    return (
        <>
            {children}
        </>
    )
}

export default AuthAutoLogin;


export function useCurrentUser(): [boolean, AuthUser | null] {
    const user = useAppSelector(selectUser);
    const loading = useAppSelector(selectLoadingUser);
    return [loading, user];
}

export function useUserHasPermission(type: PermissionType, model?: PermissionModel): boolean {
    const user = useAppSelector(selectUser);
    if (!model) return true;
    return !!user && hasPermission(user, type, model);
}
