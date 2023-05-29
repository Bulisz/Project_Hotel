import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { DialogService } from 'src/app/services/dialog.service';
import { EquipmentService } from 'src/app/services/equipment.service';
import { RoomService } from 'src/app/services/room.service';
import { CreateEquipmentComponent } from '../create-equipment/create-equipment.component';
import { DeleteEquipmentComponent } from '../delete-equipment/delete-equipment.component';
import { AddEquipmentToRoomComponent } from '../add-equipment-to-room/add-equipment-to-room.component';
import { RemoveEquipmentFromRoomComponent } from '../remove-equipment-from-room/remove-equipment-from-room.component';

@Component({
  selector: 'app-equipment-manager',
  templateUrl: './equipment-manager.component.html',
  styleUrls: ['./equipment-manager.component.css']
})
export class EquipmentManagerComponent implements OnInit{

  rooms: Array<{id: number, name: string, equipmentNames: string}> = [];

  constructor(private rs: RoomService, private dialog: MatDialog){}

  async ngOnInit() {
    await this.refreshRooms()
  }

  async refreshRooms(){
    await this.rs.getAllRooms()
      .then(res => {
        res.forEach(room => {
          this.rooms.push({id: room.id, name: room.name, equipmentNames: room.equipmentNames.join(', ')})
        });
      })
  }

  createEquipment(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'}
    };
    this.dialog.open(CreateEquipmentComponent,dialogBoxSettings);
  }

  deleteEquipment(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'}
    };
    this.dialog.open(DeleteEquipmentComponent,dialogBoxSettings);
  }

  addEquipmentToRoom(roomId: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'},
      data: {roomId: roomId}
    };
    let dialog = this.dialog.open(AddEquipmentToRoomComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: async res => {
        if(res === 'changed'){
          await this.refreshRooms()
        }
      }
    })
  }

  removeEquipmentFromRoom(roomId: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'},
      data: {roomId: roomId}
    };
    let dialog = this.dialog.open(RemoveEquipmentFromRoomComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: async res => {
        if(res === 'changed'){
          await this.refreshRooms()
        }
      }
    })
  }
}
