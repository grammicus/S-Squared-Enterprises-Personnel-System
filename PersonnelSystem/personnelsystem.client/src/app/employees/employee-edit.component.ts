import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { environment } from './../../environments/environment';
import { toSignal } from '@angular/core/rxjs-interop';

import { Employee, Manager, Role } from './employee';

@Component({
  selector: 'app-employee-edit',
  standalone: false,
  templateUrl: './employee-edit.component.html',
})
export class EmployeeEditComponent implements OnInit {
  readonly managerRequiredControl = new FormControl(false);
  protected readonly hideRequired = toSignal(
    this.managerRequiredControl.valueChanges
  );

  form!: FormGroup;

  managers!: Manager[];
  roles!: Role[];
  selectedRoles!: number[];
  selectedManagerId!: number[];
  isManager: boolean = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) { }

  ngOnInit() {
    this.http
      .get<Manager[]>(environment.baseUrl + '/employees/managers')
      .subscribe({
        next: (result) => {
          this.managers = result;
        },
        error: (error) => console.error(error),
      });

    this.http.get<Role[]>(environment.baseUrl + '/roles').subscribe({
      next: (result) => {
        this.roles = result;
      },
      error: (error) => console.error(error),
    });

    this.form = new FormGroup({
      employeeId: new FormControl(''),
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      roles: new FormControl(''),
      managerId: new FormControl(''),
    });
    this.loadData();
  }

  loadData() {
    let idParam = this.activatedRoute.snapshot.paramMap.get('id');
    let id = idParam ? +idParam : 0;
  }

  onSubmit() {
    let employee = <Employee>{};
    employee.firstName = this.form.controls['firstName'].value;
    employee.lastName = this.form.controls['lastName'].value;
    employee.employeeId = this.form.controls['employeeId'].value;
    employee.roleIds = this.form.controls['roles'].value;
    employee.isManager = this.isManager;
    if (this.form.controls['managerId'].value != '') {
      employee.managerId = this.form.controls['managerId'].value;
    }
    console.log(employee);

    const employeeUrl = environment.baseUrl + '/employees';
    this.http.post<Employee>(employeeUrl, employee).subscribe({
      next: (result) => {
        this.router.navigate(['/employees']);
      },
      error: (error) => console.error(error),
    });
  }
}
