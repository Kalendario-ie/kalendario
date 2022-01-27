import {CompanySearchResourceModel} from 'src/app/api/api';
import {IReadModel} from 'src/app/api/common/models';
import {Employee} from 'src/app/api/employees';

export interface RequestItem {
  employee: Employee;
  // appointments: CustomerRequestAppointment[];
}

export interface RequestModel extends IReadModel {
  id: string;
  owner: CompanySearchResourceModel;
  name: string;
  scheduledDate: string;
  items: RequestItem[];
  itemsCount: number;
  total: number;
  complete: boolean;
  customerNotes: string | null | undefined;
  status: string;
  // user: AdminUser;
}

