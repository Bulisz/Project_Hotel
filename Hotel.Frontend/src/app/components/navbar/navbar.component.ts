import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { AccountService } from 'src/app/services/account.service';
import { RegistrationComponent } from '../registration/registration.component';
import { UserModel } from 'src/app/models/user-model';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user?: UserModel | null;

  constructor(public dialog: MatDialog, private as: AccountService, private router: Router, private cookieService: CookieService){  }

  async ngOnInit(): Promise<void> {
    this.as.user.subscribe({
      next: (user) => this.user = user
    })

    if(localStorage.getItem('accessToken')){
      await this.as.getCurrentUser()
        .catch(err => localStorage.removeItem('accessToken'))
    }

    this.cookieService.delete('numberOfBeds')
    this.cookieService.delete('maxNumberOfDogs')
    this.cookieService.delete('bookingFrom')
    this.cookieService.delete('bookingTo')
    this.cookieService.delete('nonStandardEquipments')
  }

  getActiveRootPath(): string{
    return this.router.url.substring(1)
  }

  adminPageSelector(page: string){
    localStorage.setItem('adminPageSelector',page)
    this.as.adminPageSelector.next(page)
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
    this.as.logout();
    if(this.router.url==='/admin' || this.router.url==='/operator' || this.router.url==='/personal'){
      this.router.navigate([''])
    }
  }
}
