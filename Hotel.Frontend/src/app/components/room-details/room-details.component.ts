import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { RoomService } from 'src/app/services/room.service';
import { ImageViewerComponent } from '../image-viewer/image-viewer.component';
import { ReservationForRoomComponent } from '../reservation-for-room/reservation-for-room.component';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css']
})
export class RoomDetailsComponent implements OnInit {

  roomDetails!: RoomDetailsModel;
  roomId!: number;
  equipments!: string;
  firstImage!: string;
  currentUser: any;

  constructor(
    private rs: RoomService,
    private ar: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private as: AccountService){}

  async ngOnInit(): Promise<void> {
    this.as.user.subscribe({
      next: (res) => this.currentUser=res
    })

    this.ar.paramMap.subscribe(
      paramMap => {
        const roomId: number = Number(paramMap.get('id'));
        if (roomId) {
          this.roomId = roomId;
        }
      }
    )

    await this.rs.getRoomById(this.roomId)
    .then((res) => this.roomDetails = res)
    .catch((err) => this.router.navigate(['error'],err))

    this.equipments = this.roomDetails.equipmentNames.join(', ')
    this.firstImage = this.roomDetails.imageURLs[0]
  }

  openImages(){

    const config = new MatDialogConfig();
    config.data = { images: this.roomDetails.imageURLs }
    let dialogBoxSettings = {
      width: '1000px',
      margin: '0 auto',
      hasBackdrop: true,
      position: {top: '0%'},
      data: { images: this.roomDetails.imageURLs }
    };

    this.dialog.open(ImageViewerComponent,dialogBoxSettings);
  }

  reservationPopup(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: { roomId: this.roomId }
    };

    this.dialog.open(ReservationForRoomComponent,dialogBoxSettings)
  }
}
