import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { RegistrationComponent } from '../registration/registration.component';
import { UserModel } from 'src/app/models/user-model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user?: UserModel | null;

  constructor(public dialog: MatDialog, private router: Router, private as: AccountService){  }

  async ngOnInit(): Promise<void> {
    this.as.user.subscribe({
      next: (user) => this.user = user
    })

    if(localStorage.getItem('accessToken')){
      await this.as.getCurrentUser()
    }
  }

  loginPopup(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'}
    };

    this.dialog.open(LoginComponent,dialogBoxSettings)
  }

  registerPopup(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'}
    };

    this.dialog.open(RegistrationComponent, dialogBoxSettings)
  }



  logout(){
    this.as.logout()
  }
}
