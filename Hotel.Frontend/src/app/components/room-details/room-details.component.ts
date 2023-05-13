import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { RoomService } from 'src/app/services/room.service';
import { ImageViewerComponent } from '../image-viewer/image-viewer.component';

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

  constructor(private rs: RoomService, private ar: ActivatedRoute, private dialog: MatDialog){}

  async ngOnInit(): Promise<void> {
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
    .catch((err) => console.log(err))

    console.log(this.roomDetails)
    this.equipments = this.roomDetails.equipmentNames.join()
    this.firstImage = this.roomDetails.imageURLs[0]
  }

  openImages(){

    const config = new MatDialogConfig();
    config.data = { images: this.roomDetails.imageURLs }
    config.position = {top: '100px', left: '20%'}

    this.dialog.open(ImageViewerComponent,config);
  }

  reserve(){
  }
}
