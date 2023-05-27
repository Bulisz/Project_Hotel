import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateRoomComponent } from '../create-room/create-room.component';
import { RoomService } from 'src/app/services/room.service';
import { RoomListModel } from 'src/app/models/room-list.model';
import { ConfirmationComponent } from '../confirmation/confirmation.component';
import { UpdateImageComponent } from '../update-image/update-image.component';

@Component({
  selector: 'app-room-manager',
  templateUrl: './room-manager.component.html',
  styleUrls: ['./room-manager.component.css']
})
export class RoomManagerComponent implements OnInit{

  rooms!: Array<RoomListModel>

  constructor (private dialog: MatDialog, private rs: RoomService) {}

  ngOnInit(): void {
    this.loadRooms()
  }

  loadRooms(){
    this.rs.getAllRooms()
      .subscribe({
        next: res => this.rooms = res
      })
  }

  addNewRoom(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'}
    };
    let dialog = this.dialog.open(CreateRoomComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: res => {
        if(res === 'changed'){
          this.loadRooms()
        }
      }
    })
  }

  async modifyRoom(id: number){
    let room = await this.rs.getRoomById(id)

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'},
      data: {
        id: room?.id,
        name: room?.name,
        price: room?.price,
        numberOfBeds: room?.numberOfBeds,
        description: room?.description,
        size: room?.size,
        longDescription: room.longDescription,
        maxNumberOfDogs: room.maxNumberOfDogs}
    };
    let dialog = this.dialog.open(CreateRoomComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: res => {
        if(res === 'changed'){
          this.loadRooms()
        }
      }
    })
  }

  addImages(roomId: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: {roomId: roomId}
    };

    let dialog = this.dialog.open(UpdateImageComponent, dialogBoxSettings)

    dialog.afterClosed().subscribe({
      next: res => {
        if(res === 'changed'){
          this.loadRooms()
        }
      }
    })
  }

  async deleteRoom(id: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '10%' },
      data: { message: "Biztosan törlöd a szobát?" }
    };

    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings);
    dialogRef.afterClosed().subscribe({
      next: async (res) => {
        if (res === "agree") {
          await this.rs.deleteRoom(id)
            .catch(err => console.log(err));
          this.loadRooms();
        }
      }
    })
  }

}
