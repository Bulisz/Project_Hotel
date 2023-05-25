import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';
import { UpdateUserComponent } from '../update-user/update-user.component';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.css']
})
export class PersonalComponent implements OnInit {

  currentUser: UserModel | null = null;
  myReservations!: Array<ReservationListItem>;
  titulus = '';

  constructor(private accountService: AccountService,
    private reservationService: ReservationService,
    private router: Router,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.refreshUser()
  }

  refreshUser() {
    this.accountService.user.subscribe({
      next: (res) => this.currentUser = res
    })
    if (this.currentUser) {
      this.getMyReservations(this.currentUser.id)
    }
    if(this.currentUser?.role === "Guest"){
      this.titulus = 'Vendég'
    } else if(this.currentUser?.role === "Operator"){
      this.titulus = 'Operátor'
    } else {
      this.titulus = 'Adminisztrátor'
    }
  }

  async getMyReservations(userId: string) {
    await this.reservationService.getMyOwnReservations(userId)
      .then((res) => this.myReservations = res)
      .catch((err) => console.log(err))
  }

  async deleteProfile(userId: string) {

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '10%' },
      data: { message: "Biztosan törlöd a profilodat?" }
    };

    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings);
    dialogRef.afterClosed().subscribe({
      next: (res) => {
        if (res === "agree") {
          this.accountService.deleteProfile(userId)
            .then(() => this.router.navigate(['']))
        }
      }
    })
  }

  modifyProfile(id: string) {

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '10%' },
      data: { userName: this.currentUser?.userName, email: this.currentUser?.email, firstName: this.currentUser?.firstName, lastName: this.currentUser?.lastName, id: this.currentUser?.id }
    };

    let dialogRef = this.dialog.open(UpdateUserComponent, dialogBoxSettings)
    dialogRef.afterClosed().subscribe({
      next: () => {
        this.accountService.getCurrentUser()
        this.refreshUser()
      }
    })
  }

  refreshReservations(message: string) {
    if (this.currentUser) {
      this.getMyReservations(this.currentUser.id)
    }
  }

}
