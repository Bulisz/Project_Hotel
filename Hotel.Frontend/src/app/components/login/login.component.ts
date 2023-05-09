import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor (private as: AccountService, public dialogRef: MatDialogRef<LoginComponent>) {
    this.loginForm = new FormBuilder().group({
      userName: new FormControl('',Validators.required),
      password: new FormControl('',Validators.required)
    })
  }

  async onSubmit(){
    await this.as.login(this.loginForm.value)
      .then(() =>{
        this.dialogRef.close('ok')
      })
      .catch((err) => console.log(err))
  }
}
