import { Component, Inject, OnInit, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';
import { ForgottenPasswordComponent } from '../forgotten-password/forgotten-password.component';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { DOCUMENT } from '@angular/common';
import { GoogleLoginModel } from 'src/app/models/google-login-model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  private clientId = environment.clientId

  constructor (private as: AccountService,
    public dialogRef: MatDialogRef<LoginComponent>,
    public dialog: MatDialog,
    private _renderer2: Renderer2,
    @Inject(DOCUMENT) private _document: Document
    ) {
    this.loginForm = new FormBuilder().group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    })
  }

  ngOnInit(): void {

    // @ts-ignore
    window.onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: this.clientId,
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: false,

      });
      // @ts-ignore
      google.accounts.id.renderButton(
      // @ts-ignore
      document.getElementById("buttonDiv"),
        { theme: "outline", size: "large", width: "100%", shape: "pill" }
      );
    };
  }

  ngAfterViewInit() {
    const script1 = this._renderer2.createElement('script');
    script1.src = `https://accounts.google.com/gsi/client`;
    script1.async = `true`;
    script1.defer = `true`;
    this._renderer2.appendChild(this._document.body, script1);
  }

  async handleCredentialResponse(response: CredentialResponse) {
    let credential: GoogleLoginModel = {credential:response.credential}
    await this.as.LoginWithGoogle(credential)
      .then(() => {
        this.dialogRef.close('ok')
        window.location.reload()
      })
      .catch(err => validationHandler(err, this.loginForm))
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
    this.dialogRef.close()
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
