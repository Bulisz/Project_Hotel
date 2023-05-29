import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EquipmentService } from 'src/app/services/equipment.service';

@Component({
  selector: 'app-create-equipment',
  templateUrl: './create-equipment.component.html',
  styleUrls: ['./create-equipment.component.css']
})
export class CreateEquipmentComponent {

  createEquipmentForm: FormGroup;

  constructor(private dialogRef: MatDialogRef<CreateEquipmentComponent>, private es: EquipmentService){
    this.createEquipmentForm = new FormBuilder().group({
      name: new FormControl('', Validators.required),
      isStandard: new FormControl('', Validators.required)
    })
  }

  async onSubmit(){
    console.log(this.createEquipmentForm.value)
    await this.es.createEquipment(this.createEquipmentForm.value)
    this.dialogRef.close('changed')
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
