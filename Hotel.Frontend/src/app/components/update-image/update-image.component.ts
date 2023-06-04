import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SaveMoreImageModel } from 'src/app/models/save-more-image-model';
import { SaveOneImageModel } from 'src/app/models/save-one-image-model';
import { RoomService } from 'src/app/services/room.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-update-image',
  templateUrl: './update-image.component.html',
  styleUrls: ['./update-image.component.css']
})
export class UpdateImageComponent {

  updateImageForm: FormGroup;
  images: Array<File> = [];

  constructor(
    private rs: RoomService,
    private dialogRef: MatDialogRef<UpdateImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {roomId: number}
    ){
    this.updateImageForm  = new FormBuilder().group({
      description: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      images: new FormControl(null, Validators.required)
    })
  }

  public onImageUpload(event: any) {
    this.images = event.target.files;
    this.updateImageForm.get('images')?.setErrors(null)
  }

  async onSubmit(){
    if(this.images.length === 1){
      const saveOneImage: SaveOneImageModel =
      {
        roomId: this.data.roomId,
        description: this.updateImageForm.get('description')?.value,
        image: this.images[0]
      }
      await this.rs.saveOneImage(saveOneImage)
      .then(res => {
        this.dialogRef.close('changed')
      })
      .catch(err => validationHandler(err,this.updateImageForm))
    } else if(this.images.length > 1){
      const saveMoreImage: SaveMoreImageModel =
      {
        roomId: this.data.roomId,
        description: this.updateImageForm.get('description')?.value,
        images: this.images
      }
      await this.rs.saveMoreImage(saveMoreImage)
      .then(res => {
        this.dialogRef.close('changed')
      })
      .catch(err => validationHandler(err,this.updateImageForm))
    }
  }

  closeEvent(){
    this.dialogRef.close('close')
  }
}
