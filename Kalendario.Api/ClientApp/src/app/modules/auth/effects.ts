import {useSelector} from 'react-redux';
import {useKHistory, useQueryParams} from 'src/app/shared/util/router-extensions';
import {selectLoggedIn} from 'src/app/store/auth';


export const useRedirectIfLoggedInEffect = () => {
    const isLoggedIn = useSelector(selectLoggedIn);
    const history = useKHistory();
    const {returnUrl, ...params} = useQueryParams();
    if (isLoggedIn) {
        const redirectUrl = returnUrl || '/';
        history.push(redirectUrl, params);
    }
}
