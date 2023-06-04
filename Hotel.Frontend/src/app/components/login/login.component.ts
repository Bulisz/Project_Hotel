import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';
import { ForgottenPasswordComponent } from '../forgotten-password/forgotten-password.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor (private as: AccountService, public dialogRef: MatDialogRef<LoginComponent>, public dialog: MatDialog) {
    this.loginForm = new FormBuilder().group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    })
  }

  formChange(){
    this.loginForm.get('userName')?.setErrors(null)
    this.loginForm.get('password')?.setErrors(null)
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

  forgotPassword()
  {
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'}
    };
    this.dialogRef.close('ok');
    const dialogRef: MatDialogRef<ForgottenPasswordComponent> = this.dialog.open(ForgottenPasswordComponent, dialogBoxSettings);
  }
}
