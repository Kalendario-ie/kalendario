import {AuthEmployee, AuthUser, SocialAccount} from 'src/app/api/auth/models';
import {PermissionModel} from 'src/app/api/auth/permissions';
import {RegisterRequest} from 'src/app/api/auth/requests';
import {personParser} from 'src/app/api/common/parsers';
import {scheduleParser} from 'src/app/api/schedule';
import {stringToMoment} from 'src/app/shared/util/moment-helpers';

const imageStorage = process.env.REACT_APP_IMAGE_API_URL || 'https://res.cloudinary.com/gchahm/';

export function parseSocial(item: any): SocialAccount {
    return {
        ...item,
        lastLogin: stringToMoment(item.lastLogin),
        dateJoined: stringToMoment(item.dateJoined)
    };
}


export function parseRegisterRequest(): RegisterRequest {
    return {email: '', firstName: '', lastName: '', password1: '', password2: ''}
}

export function authUserParser(data: any): AuthUser {
    return {
        ...data,
        employee: data.employee ? authEmployeeParser(data.employee) : null,
        company: data.owner ? {...data.owner} : null,
        verified: !!data.verified
    }
}

export function authEmployeeParser(data?: any): AuthEmployee {
    return data ? {
        ...data,
        ...personParser(data),
        schedule: scheduleParser(data.schedule),
        permissionModel: PermissionModel.employee,
        private: !!data.private,
        photoUrl: data.profileImg ? imageStorage + data.profileImg
            : 'img/default-avatar.jpg',
    } : {
        ...personParser(),
        private: false,
        photoUrl: null,
        instagram: '',
        schedule: 0,
        services: [],
    }

}
