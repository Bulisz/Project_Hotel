import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit {
  
  email!: string;
  token!: string;
  resetPasswordForm!: FormGroup;
  
  constructor(private route: ActivatedRoute, private accountService: AccountService, private router: Router){}
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });

    this.resetPasswordForm = new FormGroup({
      newPassword: new FormControl('', [Validators.required, Validators.pattern('^(?=.*\\d)(?!.*\\s).{6,250}$')]),
      confirmNewPassword: new FormControl('', [Validators.required, Validators.pattern('^(?=.*\\d)(?!.*\\s).{6,250}$')]),
    })
  }

  async onSubmit(){
    if(this.resetPasswordForm.valid){
      const newPasswords = this.resetPasswordForm.getRawValue();
      await this.accountService.resetPassword({...newPasswords, token: this.token, email: this.email})
      .then((isSuccessful) => {if(isSuccessful){
        this.router.navigate(['successfulReset']);
      }})
      .catch((err) => validationHandler (err, this.resetPasswordForm))
    }
  }

}
