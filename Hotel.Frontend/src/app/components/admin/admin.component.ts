import { Component, OnInit } from '@angular/core';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { UserListModel } from 'src/app/models/user-list-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{

  users: Array<UserListModel> = [];
  reservations: Array<ReservationListItem> = [];
  pageSelector: string = '';

  constructor(private rs: ReservationService, private accountService: AccountService){}

  async ngOnInit() {
    this.accountService.adminPageSelector.subscribe({next: res => this.pageSelector = res});
    await this.loadReservations('reservationDeleted');
    await this.loadUsers();
  }

  async loadReservations(message: string){
    await this.rs.getAllReservations()
      .then(res => this.reservations = res)
      .catch(err => console.log(err))
  }

  async loadUsers(){
    await this.accountService.getUsers()
    .then(res => this.users = res)
    .catch(err => console.log(err))
  }

  refreshUsers(message: string) {
    this.loadUsers()
  }

}
