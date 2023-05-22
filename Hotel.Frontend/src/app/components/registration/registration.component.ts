import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm!: FormGroup;

  constructor(private accountService: AccountService,
              private router: Router,
              public dialogRef: MatDialogRef<RegistrationComponent>){}

  ngOnInit(): void {
    this.registerForm = new FormGroup ({
      userName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.pattern('^[a-zA-Z0-9]{2,}$')]), //TODO
      password: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9.]{2,}@[a-zA-Z0-9]{2,}.[a-zA-Z0-9]{2,}$')]),
      firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    });
  }

  async onSubmit(){
    if(this.registerForm.valid) {
      const newAccount = this.registerForm.value;
      await this.accountService.registerNewAccout(newAccount)
      .then(() => { this.router.navigate([''])
                this.dialogRef.close('ok')
    })
      .catch((err) => validationHandler (err, this.registerForm))
    }
  }

  closeRegistration() {
    this.dialogRef.close('ok')
  }

}

