import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';

import { Employee, Manager } from './employee';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  standalone: false,
})
export class EmployeesComponent implements OnInit {
  public displayedColumns: string[] = ['employeeId', 'firstName', 'lastName'];
  public selectedManagerId!: number;
  public employees!: Employee[];
  public managers!: Manager[];

  constructor(private http: HttpClient) { }
  onManagerChange() {
    if (this.selectedManagerId) {
      this.http
        .get<Employee[]>(
          environment.baseUrl +
          '/employees/managers/' +
          this.selectedManagerId +
          '/reports'
        )
        .subscribe({
          next: (result) => {
            this.employees = result;
            console.log(this.employees);
          },
          error: (error) => console.error(error),
        });
    }
  }

  ngOnInit() {
    this.http
      .get<Manager[]>(environment.baseUrl + '/employees/managers')
      .subscribe({
        next: (result) => {
          this.managers = result;
        },
        error: (error) => console.error(error),
      });
  }
}
