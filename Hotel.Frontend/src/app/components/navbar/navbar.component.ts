import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
  
  ngOnInit(): void {
    this.as.user.subscribe({
      next: (user) => this.user = user
    })
  }

  loginPopup(){
    let dialogRef = this.dialog.open(LoginComponent)

    dialogRef.afterClosed().subscribe(() => {
      this.router.navigate([''])
    })
  }

  registerPopup(){
    let dialogRef = this.dialog.open(RegistrationComponent)

    dialogRef.afterClosed().subscribe(() => {
      this.router.navigate([''])
    })
  }

  

  logout(){
    this.as.logout()
  }
}
