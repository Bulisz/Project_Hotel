import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { DialogService } from 'src/app/services/dialog.service';
import { EquipmentService } from 'src/app/services/equipment.service';

@Component({
  selector: 'app-delete-equipment',
  templateUrl: './delete-equipment.component.html',
  styleUrls: ['./delete-equipment.component.css']
})
export class DeleteEquipmentComponent implements OnInit{

  deleteEquipmentForm: FormGroup;
  equipmentList: Array<{id:number, name:string}> = [];

  constructor(private dialogRef: MatDialogRef<DeleteEquipmentComponent>, private es: EquipmentService, private ds: DialogService){
    this.deleteEquipmentForm = new FormBuilder().group({
      equipment: new FormControl('', Validators.required)
    })
  }

  async ngOnInit() {
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
    let result = await this.ds.confirmationDialog("Biztosan törlöd a felszereltséget?")
    if(result === "agree"){
      await this.es.deleteEquipment(this.deleteEquipmentForm.get('equipment')?.value)
      this.dialogRef.close('changed')
    }
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
