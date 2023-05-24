import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.css']
})
export class ReservationListComponent implements OnInit {
  
  @Input() reservationList!: Array<ReservationListItem>;
  

  constructor(private reservationService: ReservationService, private router: Router){}
  
  ngOnInit(): void {}

  isDeletable(bookingFrom: Date){
    const currentDate = new Date(Date.now());
    currentDate.setDate(currentDate.getDate() + 10);
    
    return new Date(currentDate.setDate(currentDate.getDate())) < new Date(bookingFrom);
  }

  async deleteReservation(id: number){
    this.reservationService.deleteReservation(id)
    .then(() => this.router.navigate(['']))
    .catch((err) => console.log(err))
  }

}
