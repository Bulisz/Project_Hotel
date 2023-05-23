import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EventDetailsModel } from 'src/app/models/event-details-model';
import { EventService } from 'src/app/services/event.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css']
})
export class CreateEventComponent {

  eventForm: FormGroup;
  image: File | null = null;
  createdEvent!: EventDetailsModel;

  constructor(private es: EventService, private dialogRef: MatDialogRef<CreateEventComponent>){
    this.eventForm = new FormBuilder().group({
      title: new FormControl('' , Validators.required),
      text: new FormControl('' , Validators.required),
      schedule: new FormControl('' , Validators.required)
    })
  }

  public onImageUpload(event: any) {
    this.image = event.target.files[0];
  }

  async onSubmit(){
    const formValue = this.eventForm.getRawValue()
    const parsedFormValue = {...formValue, image: this.image};
    console.log(parsedFormValue)
    await this.es.createEvent(parsedFormValue)
      .then(res => this.createdEvent = res)
      .catch(err => validationHandler(err,this.eventForm))
    this.dialogRef.close()
  }

  closeEvent(){
    this.dialogRef.close()
  }
}
