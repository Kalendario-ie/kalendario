import {IReadModel} from 'src/app/api/common/models';

export interface PermissionGroup extends IReadModel {
  name: string;
  permissions: number[];

}

export interface Permission extends IReadModel {
  id: string;
  name: string;
  codename: string;
}
