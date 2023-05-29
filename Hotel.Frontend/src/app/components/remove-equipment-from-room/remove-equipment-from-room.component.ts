import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoomDetailsModel } from 'src/app/models/room-details-model';
import { DialogService } from 'src/app/services/dialog.service';
import { EquipmentService } from 'src/app/services/equipment.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-remove-equipment-from-room',
  templateUrl: './remove-equipment-from-room.component.html',
  styleUrls: ['./remove-equipment-from-room.component.css']
})
export class RemoveEquipmentFromRoomComponent implements OnInit {

  removeEquipmentForm: FormGroup;
  room!: RoomDetailsModel;
  equipmentList: Array<{id:number, name:string}> = [];

  constructor(
    private rs: RoomService,
    private es: EquipmentService,
    private ds: DialogService,
    private dialogRef: MatDialogRef<RemoveEquipmentFromRoomComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {roomId: number}
    ){
      this.removeEquipmentForm = new FormBuilder().group({
        equipment: new FormControl('', Validators.required)
      })
  }

  async ngOnInit() {
    await this.rs.getRoomById(this.data.roomId)
      .then(res => this.room = res)
      let standardEquipments = await this.es.getStandardEquipments()
      let nonStandardEquipments = await this.es.getNonStandardEquipments()
      standardEquipments.forEach(seq => {
        if(this.room.equipmentNames.some(en => en === seq.name)){
          this.equipmentList.push({id: seq.id, name: seq.name+'(Standard)'})
        }
      });
      nonStandardEquipments.forEach(nseq => {
        if(this.room.equipmentNames.some(en => en === nseq.name)){
          this.equipmentList.push({id: nseq.id, name: nseq.name+'(NemStandard)'})
        }
      });
  }

  async onSubmit(){
    let result = await this.ds.confirmationDialog("Biztosan leveszed a felszereltséget?")
    if(result === "agree"){
      let formData = {roomId: this.data.roomId, equipmentId: this.removeEquipmentForm.get('equipment')?.value}
      await this.es.removeEquipmentFromRoom(formData)
      this.dialogRef.close('changed')
    }
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }

}
