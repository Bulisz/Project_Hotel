import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoomService } from 'src/app/services/room.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-create-room',
  templateUrl: './create-room.component.html',
  styleUrls: ['./create-room.component.css']
})
export class CreateRoomComponent implements OnInit {

  createRoomForm: FormGroup;

  constructor(
    private rs: RoomService,
    private dialogRef: MatDialogRef<CreateRoomComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
        id: number,
        name: string,
        price: number,
        numberOfBeds: number,
        description: string,
        size: string,
        longDescription: string,
        maxNumberOfDogs: number
    }){
    this.createRoomForm = new FormBuilder().group({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern('^[A-ZÖÜÓŐÚÉÁŰÍ]{1,}[a-z0-9öüóőúéáűí]{1,49}$')]),
      price: new FormControl(0, [Validators.min(1000), Validators.max(100000), Validators.pattern('^[0-9]{4,6}$')]),
      numberOfBeds: new FormControl(0, [Validators.min(1), Validators.max(10), Validators.pattern('^[0-9]{1,2}$')]),
      description: new FormControl('', [Validators.minLength(10), Validators.maxLength(250)]),
      size: new FormControl('', [Validators.minLength(5), Validators.maxLength(50)]),
      longDescription: new FormControl('', [Validators.minLength(100), Validators.maxLength(4000)]),
      maxNumberOfDogs: new FormControl(0, [Validators.min(1), Validators.max(10), Validators.pattern('^[0-9]{1,2}$')]),
    })
  }

  ngOnInit(): void {
    if(this.data){
      this.createRoomForm.get('name')?.setValue(this.data.name)
      this.createRoomForm.get('price')?.setValue(this.data.price)
      this.createRoomForm.get('numberOfBeds')?.setValue(this.data.numberOfBeds)
      this.createRoomForm.get('description')?.setValue(this.data.description)
      this.createRoomForm.get('size')?.setValue(this.data.size)
      this.createRoomForm.get('longDescription')?.setValue(this.data.longDescription)
      this.createRoomForm.get('maxNumberOfDogs')?.setValue(this.data.maxNumberOfDogs)
    }
  }

  async onSubmit(){
    if(this.data){
      let formData = this.createRoomForm.getRawValue()
      let parsedFormData = {...formData, id: this.data.id, available: true}

      await this.rs.updateRoom(parsedFormData)
      .then(res => {
        this.dialogRef.close('changed')
      })
      .catch(err => validationHandler(err,this.createRoomForm))
    } else {
      await this.rs.createRoom(this.createRoomForm.value)
      .then(res => {
        this.dialogRef.close('changed')
      })
      .catch(err => validationHandler(err,this.createRoomForm))
    }
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
