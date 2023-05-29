import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { RoomListModel } from 'src/app/models/room-list.model';
import { UserListModel } from 'src/app/models/user-list-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { RoomService } from 'src/app/services/room.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-reservation-for-user',
  templateUrl: './reservation-for-user.component.html',
  styleUrls: ['./reservation-for-user.component.css']
})
export class ReservationForUserComponent implements OnInit {

  reservationForm: FormGroup;
  users: Array<UserListModel> = [];
  rooms: Array<RoomListModel> = [];

  constructor (
    private reservationService: ReservationService,
    public dialogRef: MatDialogRef<ReservationForUserComponent>,
    private rs: RoomService,
    private as: AccountService) {
    this.reservationForm = new FormBuilder().group({
      userId: new FormControl(Date , Validators.required),
      roomId: new FormControl(Date , Validators.required),
      bookingFrom: new FormControl(Date , Validators.required),
      bookingTo: new FormControl(Date, Validators.required)
    })
  }

  async ngOnInit() {
    await this.as.getUsers()
      .then(res => this.users = res)
      .catch(err => console.log(err))

    await this.rs.getAllRooms()
      .then(res => this.rooms = res)
      .catch(err =>  console.log(err))
  }

  async onSubmit(){
    await this.reservationService.createReservationForRoom(this.reservationForm.value)
      .then(res => this.dialogRef.close('agree'))
      .catch((err) => validationHandler(err,this.reservationForm))
  }

  closeReservation() {
    this.dialogRef.close('close')
  }

}
