import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.css']
})
export class PersonalComponent implements OnInit {
  
  currentUser: UserModel | null = null;
  myReservations!: Array<ReservationListItem>; 

  constructor(private accountService: AccountService, private reservationService: ReservationService, private router: Router){}
  
  ngOnInit(): void {
    this.accountService.user.subscribe({
      next: (res) => this.currentUser = res
    })
      if(this.currentUser){
        this.getMyReservations(this.currentUser.id)
      }
  }

  async getMyReservations(userId: string){
    await this.reservationService.getMyOwnReservations(userId)
    .then((res) => this.myReservations = res)
      .catch((err) => console.log(err))
  }

  async deleteProfile(userId: string){
    await this.accountService.deleteProfile(userId)
    .then(() => this.router.navigate(['']))
  }

}
