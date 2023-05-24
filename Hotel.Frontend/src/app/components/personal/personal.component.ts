import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.css']
})
export class PersonalComponent implements OnInit {
  
  currentUser: UserModel | null = null;
  myReservations!: Array<ReservationListItem>; 

  constructor(private accountService: AccountService,
              private reservationService: ReservationService,
              private router: Router,
              private dialog: MatDialog){}
  
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

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: {message: "Biztosan törlöd a profilodat?"}
    };

    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings);
    dialogRef.afterClosed().subscribe({
      next: (res) => {
        if(res === "agree"){
          this.accountService.deleteProfile(userId)
            .then(() => this.router.navigate(['']))
        }
      }
    })

    
  }

  refreshReservations(message: string){
    if(this.currentUser){
      this.getMyReservations(this.currentUser.id)
    }
  }

}
