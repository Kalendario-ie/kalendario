import React from 'react';
import {Route, Switch} from 'react-router-dom';
import LogoutContainer from 'src/app/modules/auth/logout-container';
import ResetPasswordConfirmContainer from 'src/app/modules/auth/reset-password-confirm-container';
import ResetPasswordContainer from 'src/app/modules/auth/reset-password-container';
import {AUTH_ROUTES} from 'src/app/modules/auth/urls';
import LoginContainer from './login/login-container';
import RegisterContainer from 'src/app/modules/auth/register-container';


const AuthRoutes = () =>
    <Switch>
        <Route path={AUTH_ROUTES.LOGIN} component={LoginContainer}/>
        <Route path={AUTH_ROUTES.REGISTER} component={RegisterContainer}/>
        <Route path={AUTH_ROUTES.RESET_PASSWORD} component={ResetPasswordContainer}/>
        <Route path={AUTH_ROUTES.RESET_PASSWORD_CONFIRM} component={ResetPasswordConfirmContainer}/>
        <Route path={AUTH_ROUTES.LOGOUT} component={LogoutContainer}/>
    </Switch>


export default AuthRoutes;
