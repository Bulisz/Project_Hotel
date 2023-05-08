import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor (private fb: FormBuilder) {
    this.loginForm = fb.group({
      userName: new FormControl('',Validators.required),
      password: new FormControl('',Validators.required)
    })
  }

  onSubmit(){
    
  }
}
