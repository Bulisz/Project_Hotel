import { Component, OnInit } from '@angular/core';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{

  reservations: Array<ReservationListItem> = [];

  constructor(private rs: ReservationService){}

  async ngOnInit() {
    this.loadReservations('reservationDeleted')
  }

  async loadReservations(message: string){
    await this.rs.getAllReservations()
      .then(res => this.reservations = res)
      .catch(err => console.log(err))
  }


}
