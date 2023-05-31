import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationForUserComponent } from '../reservation-for-user/reservation-for-user.component';
import { DialogService } from 'src/app/services/dialog.service';
import { RoomCalendarComponent } from '../room-calendar/room-calendar.component';

@Component({
  selector: 'app-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.css']
})
export class ReservationListComponent implements OnInit {

  @Input() reservationList!: Array<ReservationListItem>;
  @Output() reservationDeleted = new EventEmitter<string>;
  user: UserModel | null = null;


  constructor(private reservationService: ReservationService, private dialog: MatDialog, private as: AccountService, private dialogService: DialogService){}

  ngOnInit(): void {
    this.as.user.subscribe({
      next: res => this.user = res
    })
  }

  isDeletable(bookingFrom: Date){
    const currentDate = new Date(Date.now());
    currentDate.setDate(currentDate.getDate() + 10);

    return new Date(currentDate.setDate(currentDate.getDate())) < new Date(bookingFrom);
  }

  async deleteReservation(id: number){
    let result = await this.dialogService.confirmationDialog("Biztos, hogy törlöd a foglalást?")
    if(result === "agree"){
      this.reservationService.deleteReservation(id)
              .then(() => this.reservationDeleted.emit('reservationDeleted'))
              .catch((err) => console.log(err))
    }
  }

  reservationForUser(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'}
    };

    let dialogref = this.dialog.open(ReservationForUserComponent,dialogBoxSettings)

    dialogref.afterClosed().subscribe({
      next: res => {
        if(res === 'agree'){
          this.reservationDeleted.emit('reservationDeleted')
        }
      }
    })
  }


  showRoomCalendar(){
    let dialogBoxSettings = {
      width: '830px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '1.2%'}
    };

    let dialogref = this.dialog.open(RoomCalendarComponent,dialogBoxSettings)

    dialogref.afterClosed().subscribe({
      next: res => {
        if(res === 'agree'){
          this.reservationDeleted.emit('reservationDeleted')
        }
      }
    })
  }
}
