import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EquipmentService } from 'src/app/services/equipment.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-create-equipment',
  templateUrl: './create-equipment.component.html',
  styleUrls: ['./create-equipment.component.css']
})
export class CreateEquipmentComponent {

  createEquipmentForm: FormGroup;

  constructor(private dialogRef: MatDialogRef<CreateEquipmentComponent>, private es: EquipmentService){
    this.createEquipmentForm = new FormBuilder().group({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      isStandard: new FormControl('', Validators.required)
    })
  }

  async onSubmit(){
    await this.es.createEquipment(this.createEquipmentForm.value)
      .then(()=>this.dialogRef.close('changed'))
      .catch(err => validationHandler(err, this.createEquipmentForm))
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
