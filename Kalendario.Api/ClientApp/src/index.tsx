import {Elements} from '@stripe/react-stripe-js';
import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import {Provider} from 'react-redux';
import {BrowserRouter} from 'react-router-dom';
import {configureBaseApi} from 'src/app/api/common/clients/base-api';
import {configureStripe} from 'src/app/external-apis/configure-stripe';
import AuthAutoLogin from 'src/app/shared/context-providers/auth-auto-login';
import {store} from 'src/app/store/store';
import App from './app/App';
import reportWebVitals from './reportWebVitals';
import './styles/index.scss';

import * as serviceWorkerRegistration from './serviceWorkerRegistration';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') || undefined;
const rootElement = document.getElementById('root');

configureBaseApi();
const stripePromise = configureStripe();

ReactDOM.render(
    <React.StrictMode>
        <BrowserRouter basename={baseUrl}>
            {/*<Router history={history}>*/}
                <Provider store={store}>
                    <AuthAutoLogin>
                        <Elements stripe={stripePromise}>
                            <App/>
                        </Elements>
                    </AuthAutoLogin>
                </Provider>
            {/*</Router>*/}
        </BrowserRouter>
    </React.StrictMode>,
    rootElement);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
