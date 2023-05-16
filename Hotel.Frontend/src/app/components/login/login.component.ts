import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor (private as: AccountService, public dialogRef: MatDialogRef<LoginComponent>) {
    this.loginForm = new FormBuilder().group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    })
  }

  async onSubmit(){
    await this.as.login(this.loginForm.value)
      .then(() =>{
        this.dialogRef.close('ok')
      })
      .catch((err) => validationHandler(err,this.loginForm))
  }

  closeLogin() {
    this.dialogRef.close('ok')
  }
}
