import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EventDetailsModel } from 'src/app/models/event-details-model';
import { EventService } from 'src/app/services/event.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-update-event',
  templateUrl: './update-event.component.html',
  styleUrls: ['./update-event.component.css']
})
export class UpdateEventComponent implements OnInit{
  eventForm: FormGroup;
  image: File | null = null;
  updatedEvent!: EventDetailsModel;

  constructor(private es: EventService,
              private dialogRef: MatDialogRef<UpdateEventComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {title: string, text: string, schedule: string, id: number, oldImageUrl: string}){

    this.eventForm = new FormBuilder().group({
      title: new FormControl('' , Validators.required),
      text: new FormControl('' , Validators.required),
      schedule: new FormControl('' , Validators.required),
      image: new FormControl(null)
    })
  }
  ngOnInit(): void {
    this.eventForm.get("title")?.setValue(this.data.title)
    this.eventForm.get("text")?.setValue(this.data.text)
    this.eventForm.get("schedule")?.setValue(this.data.schedule)
  }

  public onImageUpload(event: any) {
    this.image = event.target.files[0];
    this.eventForm.get('image')?.setErrors(null)
  }

  async onSubmit(){
    const formValue = this.eventForm.getRawValue()
    const parsedFormValue = {...formValue, image: this.image, id: this.data.id, oldImageUrl: this.data.oldImageUrl};

    await this.es.updateEvent(parsedFormValue)
      .then((res) => {

        this.dialogRef.close()
      })
      .catch(err => validationHandler(err,this.eventForm))
  }

  closeEvent(){
    this.dialogRef.close()
  }
}
