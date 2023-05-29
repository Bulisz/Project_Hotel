import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { EquipmentService } from 'src/app/services/equipment.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-add-equipment-to-room',
  templateUrl: './add-equipment-to-room.component.html',
  styleUrls: ['./add-equipment-to-room.component.css']
})
export class AddEquipmentToRoomComponent implements OnInit {

  addEquipmentForm: FormGroup;
  room!: RoomDetailsModel;
  equipmentList: Array<{id:number, name:string}> = [];

  constructor(
    private rs:RoomService,
    private es:EquipmentService,
    private dialogRef: MatDialogRef<AddEquipmentToRoomComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {roomId: number}
    ){
      this.addEquipmentForm = new FormBuilder().group({
        equipment: new FormControl('', Validators.required)
      })
  }

  async ngOnInit() {
    await this.rs.getRoomById(this.data.roomId)
      .then(res => this.room = res)

    let standardEquipments = await this.es.getStandardEquipments()
    let nonStandardEquipments = await this.es.getNonStandardEquipments()
    standardEquipments.forEach(seq => {
      this.equipmentList.push({id: seq.id, name: seq.name+'(Standard)'})
    });
    nonStandardEquipments.forEach(nseq => {
      this.equipmentList.push({id: nseq.id, name: nseq.name+'(NemStandard)'})
    });
  }

  async onSubmit(){
    let formData = {roomId: this.data.roomId, equipmentId: this.addEquipmentForm.get('equipment')?.value}
    await this.es.addEquipmentToRoom(formData)
    this.dialogRef.close('changed')
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
