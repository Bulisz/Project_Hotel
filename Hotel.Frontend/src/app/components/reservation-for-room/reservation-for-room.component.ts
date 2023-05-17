import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ReservationDetailsModel } from 'src/app/models/reservation-details-model';
import { UserModel } from 'src/app/models/user-model';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-reservation-for-room',
  templateUrl: './reservation-for-room.component.html',
  styleUrls: ['./reservation-for-room.component.css']
})
export class ReservationForRoomComponent {

  reservationForm: FormGroup;
  createdReservation: ReservationDetailsModel | null = null;

  constructor (
    private reservationService: ReservationService,
    public dialogRef: MatDialogRef<ReservationForRoomComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {roomId: number},
    private as: AccountService) {
    this.reservationForm = new FormBuilder().group({
      bookingFrom: new FormControl(Date , Validators.required),
      bookingTo: new FormControl(Date, Validators.required)
    })
  }

  async onSubmit(){
    const formValue = this.reservationForm.getRawValue()
    const parsedFormValue = {...formValue, userId: this.as.user.value?.id, roomId: this.data.roomId}

    await this.reservationService.createReservationForRoom(parsedFormValue)
      .then((res) =>{
        this.createdReservation = res;
      })
      .catch((err) => validationHandler(err,this.reservationForm))
  }

  closeReservation() {
    this.dialogRef.close('ok')
  }

}
