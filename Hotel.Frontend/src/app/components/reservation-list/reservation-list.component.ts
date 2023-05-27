import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.css']
})
export class ReservationListComponent implements OnInit {

  @Input() reservationList!: Array<ReservationListItem>;
  @Output() reservationDeleted = new EventEmitter<string>;
  user: UserModel | null = null;


  constructor(private reservationService: ReservationService, private dialog: MatDialog, private as: AccountService){}

  ngOnInit(): void {}

  isDeletable(bookingFrom: Date){
    this.as.user.subscribe({
      next: res => this.user = res
    })

    const currentDate = new Date(Date.now());
    currentDate.setDate(currentDate.getDate() + 10);

    return new Date(currentDate.setDate(currentDate.getDate())) < new Date(bookingFrom);
  }

  async deleteReservation(id: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: {message: "Biztos, hogy törlöd a foglalásod?"}
    };

    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings);
    dialogRef.afterClosed().subscribe({
      next: (res) => {
        if(res === "agree"){
          this.reservationService.deleteReservation(id)
              .then(() => this.reservationDeleted.emit('reservationDeleted'))
              .catch((err) => console.log(err))
        }
      }
    })
  }

}
