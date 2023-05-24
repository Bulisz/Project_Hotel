import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';

@Component({
  selector: 'app-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.css']
})
export class ReservationListComponent implements OnInit {
  
  @Input() reservationList!: Array<ReservationListItem>;
  @Output() reservationDeleted = new EventEmitter<string>
  

  constructor(private reservationService: ReservationService, private router: Router, private dialog: MatDialog){}
  
  ngOnInit(): void {}

  isDeletable(bookingFrom: Date){
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
