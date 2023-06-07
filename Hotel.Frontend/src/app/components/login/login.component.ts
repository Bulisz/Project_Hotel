import { Component, Inject, NgZone, OnInit, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { validationHandler } from 'src/utils/validationHandler';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor (private as: AccountService, public dialogRef: MatDialogRef<LoginComponent>, private _ngZone: NgZone,private _renderer2: Renderer2, @Inject(DOCUMENT) private _document: Document) {
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
        client_id: '395659035574-820j3194u2k30g9t6h24q9s98evdunlq.apps.googleusercontent.com',
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });
      // @ts-ignore
      google.accounts.id.renderButton(
      // @ts-ignore
      document.getElementById("buttonDiv"),
        { theme: "outline", size: "large", width: "100%" }
      );
      // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => {});
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
    await this.as.LoginWithGoogle(response.credential).subscribe(
      (x:any) => {
        this._ngZone.run(() => {
        })},
      (error:any) => {
          console.log(error);
        }
      );
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
}
