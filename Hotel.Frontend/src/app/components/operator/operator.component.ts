import { Component, OnInit } from '@angular/core';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-operator',
  templateUrl: './operator.component.html',
  styleUrls: ['./operator.component.css']
})
export class OperatorComponent  implements OnInit{

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
