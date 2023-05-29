import { Component, OnInit } from '@angular/core';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { DialogService } from 'src/app/services/dialog.service';
import { EquipmentService } from 'src/app/services/equipment.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-equipment-manager',
  templateUrl: './equipment-manager.component.html',
  styleUrls: ['./equipment-manager.component.css']
})
export class EquipmentManagerComponent implements OnInit{

  rooms: Array<{id: number, name: string, equipmentNames: string}> = [];

  constructor(private rs: RoomService, private es: EquipmentService, private ds: DialogService){}

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
}
