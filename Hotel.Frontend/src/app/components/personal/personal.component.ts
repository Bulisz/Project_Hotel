import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { UpdateUserComponent } from '../update-user/update-user.component';
import { DialogService } from 'src/app/services/dialog.service';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.css']
})
export class PersonalComponent implements OnInit {

  currentUser: any;
  myReservations!: Array<ReservationListItem>;
  titulus = '';

  constructor(private accountService: AccountService,
    private reservationService: ReservationService,
    private router: Router,
    private dialogService: DialogService,
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
    let result = await this.dialogService.confirmationDialog("Biztosan törlöd a profilodat?")
    if(result === "agree"){
      this.accountService.deleteProfile(userId)
        .then(() => this.router.navigate(['']))
    }
  }

  modifyProfile(id: string) {

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '10%' },
      data: { userName: this.currentUser?.username, email: this.currentUser?.email, firstName: this.currentUser?.firstName, lastName: this.currentUser?.lastName, id: this.currentUser?.id }
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
