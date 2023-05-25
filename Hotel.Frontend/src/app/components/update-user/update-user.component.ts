import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit{

  userModifyForm: FormGroup;

  constructor(private as: AccountService,
    private dialogRef: MatDialogRef<UpdateUserComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {userName: string, email: string, firstName: string, lastName: string, id: string}){
    this.userModifyForm = new FormBuilder().group({
      userName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.pattern('^[a-zA-Z0-9]{2,}$')]),
      email: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9.]{2,}@[a-zA-Z0-9]{2,}.[a-zA-Z0-9]{2,}$')]),
      firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2)])
    })
  }

  ngOnInit(): void {
    this.userModifyForm.get("userName")?.setValue(this.data.userName)
    this.userModifyForm.get("email")?.setValue(this.data.email)
    this.userModifyForm.get("firstName")?.setValue(this.data.firstName)
    this.userModifyForm.get("lastName")?.setValue(this.data.lastName)
  }

  async onSubmit(){
    const formValue = this.userModifyForm.getRawValue()
    const parsedFormValue = {...formValue, id: this.data.id};

    await this.as.updateUser(parsedFormValue)
    .then(res => this.dialogRef.close())
    .catch(err => validationHandler(err, this.userModifyForm))
  }

  closeUserModify(){
    this.dialogRef.close()
  }
}
