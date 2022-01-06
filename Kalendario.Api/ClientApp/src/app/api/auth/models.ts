import {Moment} from 'moment';
import {IReadModel, Person} from 'src/app/api/common/models';
import {Schedule} from 'src/app/api/schedule';

type SocialProvider = 'facebook';

export interface AuthEmployee extends Person {
    id: number;
    private: boolean;
    firstName: string;
    lastName: string;
    name: string;
    email: string;
    phone: string;
    schedule: Schedule;
    instagram: string;
    photoUrl: string;
    bio: string;
    services: number[];
}

export interface AuthUser extends IReadModel {
    firstName: string;
    lastName: string;
    email: string;
    employee: AuthEmployee | null;
    employeeId: number;
    groups: number[];
    permissions: string[];
    company: { id: number, name: string } | null;
    verified: boolean;
}

export interface SocialAccount {
    id: number;
    provider: SocialProvider;
    uid: string;
    lastLogin: Moment;
    dateJoined: Moment;
}

export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
    user: AuthUser;
}

export interface RefreshAccessTokenResponse {
    access: string;
    accessTokenExpiration: string;
}

export interface ForgotPasswordResponse {
    detail: string;
}
