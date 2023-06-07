import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-forgotten-password',
  templateUrl: './forgotten-password.component.html',
  styleUrls: ['./forgotten-password.component.css']
})
export class ForgottenPasswordComponent implements OnInit{
  
  emailForm!: FormGroup
  
  constructor(private dialogRef: MatDialogRef<ForgottenPasswordComponent>, private accountService: AccountService, private router: Router){}
  
  ngOnInit(): void {
    this.emailForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern('^[a-z0-9.]{2,}[@][a-z0-9]{2,}[.][a-z]{2,}$')]),
    })
  }

  async onSubmit(){
    if(this.emailForm.valid){
      const emailAddress = this.emailForm.getRawValue();
      await this.accountService.forgotPassword(emailAddress)
      .then(() => {
        this.dialogRef.close('ok');
        this.router.navigate(['successfulEmailSend'])
      }
      )
      .catch((err) => validationHandler (err, this.emailForm))
    }

  }

  closeDialog(){
    this.dialogRef.close('ok')
  }

}
