import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmailVerificationModel } from 'src/app/models/email-verification-model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent implements OnInit {
  
  email!: string;
  token!: string;
  
  constructor(private route: ActivatedRoute, private accountService: AccountService, private router: Router){}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });

    const verificationRequest: EmailVerificationModel = {
      email: this.email,
      token: this.token
    }

    let isEmailValid = false; 
    this.accountService.confirmEmail(verificationRequest)
      .then((res) => isEmailValid = res)
    if(isEmailValid){
      this.accountService.confirmEmail(verificationRequest)
        .then((res) => this.router.navigate(['']))
    }
  }


}
