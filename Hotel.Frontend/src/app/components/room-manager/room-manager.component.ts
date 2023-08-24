import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateRoomComponent } from '../create-room/create-room.component';
import { RoomService } from 'src/app/services/room.service';
import { RoomListModel } from 'src/app/models/room-list.model';
import { UpdateImageComponent } from '../update-image/update-image.component';
import { DeleteImageComponent } from '../delete-image/delete-image.component';
import { DialogService } from 'src/app/services/dialog.service';

@Component({
  selector: 'app-room-manager',
  templateUrl: './room-manager.component.html',
  styleUrls: ['./room-manager.component.css']
})
export class RoomManagerComponent implements OnInit{

  rooms!: Array<RoomListModel>

  constructor (private dialog: MatDialog, private rs: RoomService, private ds: DialogService) {}

  async ngOnInit() {
    await this.loadRooms()
  }

  async loadRooms(){
    await this.rs.getAllRooms()
      .then(res => this.rooms = res)
      .catch(err =>  console.log(err))
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

  async deleteImageOfRoom(id: number){
    let room = await this.rs.getRoomById(id)

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '3%' },
      data: { imageUrls: room.imageURLs }
    };

    let dialog = this.dialog.open(DeleteImageComponent, dialogBoxSettings)

    dialog.afterClosed().subscribe({
      next: res => {
        if(res === 'changed'){
          this.loadRooms()
        }
      }
    })
  }

  async deleteRoom(id: number){
    let result = await this.ds.confirmationDialog("Biztosan törlöd a szobát?")
    if(result === "agree"){
      await this.rs.deleteRoom(id)
      .catch(err => console.log(err));
      this.loadRooms();
    }
  }

}
