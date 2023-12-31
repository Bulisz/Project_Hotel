import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  registerForm!: FormGroup;

  constructor(private accountService: AccountService,
              private router: Router,
              public dialogRef: MatDialogRef<RegistrationComponent>){
                this.registerForm = new FormBuilder().group({
                  userName: new FormControl('', [Validators.required, Validators.pattern('^(?!.*(?:admin|Admin|operator|Operator))[a-zA-Z0-9]{2,30}$')]),
                  password: new FormControl('', [Validators.required, Validators.pattern('^(?=.*\\d)(?!.*\\s).{6,250}$')]),
                  confirmPassword: new FormControl('', [Validators.required, Validators.pattern('^(?=.*\\d)(?!.*\\s).{6,}$')]),
                  email: new FormControl('', [Validators.required, Validators.pattern('^[a-z0-9.]{2,}[@][a-z0-9]{2,}[.][a-z]{2,}$')]),
                  firstName: new FormControl('', [Validators.required, Validators.pattern("^[A-ZÀ-ÖÜŐÚŰa-zá-öüőúű](?!.*  )[a-zA-ZÀ-ÖØ-öø-ÿűő '-]{1,49}$")]),
                  lastName: new FormControl('', [Validators.required, Validators.pattern("^^[A-ZÀ-ÖÜŐÚŰa-zá-öüőúű](?!.*  )[a-zA-ZÀ-ÖØ-öø-ÿűő '.-]{1,49}$")])
                });
              }

  async onSubmit(){
    if(this.registerForm.valid) {
      const newAccount = this.registerForm.value;
      //this.router.navigate(['patient']);
      await this.accountService.registerNewAccout(newAccount)
      .then(() => { this.dialogRef.close('ok');
        this.router.navigate(['patient']);

    })
      .catch((err) => validationHandler (err, this.registerForm))
    }
  }

  changeMe(controlName: string){
    this.registerForm.get(controlName)?.setErrors(null)
  }

  closeRegistration() {
    this.dialogRef.close()
  }

}

