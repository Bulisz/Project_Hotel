import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  constructor(public dialog: MatDialog, private router: Router){
  }

  loginPopup(){
    let dialogRef = this.dialog.open(LoginComponent)

    dialogRef.afterClosed().subscribe(() => {
      this.router.navigate([''])
    })
  }
}
