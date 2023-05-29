import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-update-user-as-admin',
  templateUrl: './update-user-as-admin.component.html',
  styleUrls: ['./update-user-as-admin.component.css']
})
export class UpdateUserAsAdmin implements OnInit {

  updateUser: FormGroup;

  constructor(private accountService: AccountService,
              private dialogRef: MatDialogRef<UpdateUserAsAdmin>,
              @Inject(MAT_DIALOG_DATA) public data: {
                id: string,
                firstName: string,
                lastName: string,
                username: string,
                email: string,
                emailConfirmed: string,
                role: string}){
                  this.updateUser = new FormBuilder().group({
                    firstName: new FormControl('' , Validators.required),
                    lastName: new FormControl(''),
                    username: new FormControl(''),
                    email: new FormControl(''),
                    emailConfirmed: new FormControl(''),
                    role: new FormControl(''),
                  })
                }
  
  ngOnInit(): void {
    if(this.data){
      this.updateUser.get('firstName')?.setValue(this.data.firstName)
      this.updateUser.get('lastName')?.setValue(this.data.lastName)
      this.updateUser.get('username')?.setValue(this.data.username)
      this.updateUser.get('email')?.setValue(this.data.email)
      this.updateUser.get('emailConfirmed')?.setValue(this.data.emailConfirmed)
      this.updateUser.get('role')?.setValue(this.data.role)
    }
  }

  async onSubmit(){
    if(this.data){
      let formValue = this.updateUser.getRawValue();
      let extendedFormValue = {...formValue, id: this.data.id};
  
      await this.accountService.updateUserAsAdmin(extendedFormValue)
        .then((res) => {
          this.dialogRef.close('changed')
        })
        .catch(err => validationHandler(err, this.updateUser))
    }
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }

}
