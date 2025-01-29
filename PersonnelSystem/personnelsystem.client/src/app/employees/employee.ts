export interface Employee {
  employeeId: number;
  firstName: string;
  lastName: string;
  roleIds: number[];
  managerId: number;
  isManager: boolean;
}

export interface Manager {
  employeeId: number;
  firstName: string;
  lastName: string;
  roleIds: number[];
  managerId: number;
  reports: Employee[];
}

export interface Role {
  roleId: number;
  name: string;
}
