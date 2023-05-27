import { Component, Inject, OnInit, Pipe, PipeTransform } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoomService } from 'src/app/services/room.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-delete-image',
  templateUrl: './delete-image.component.html',
  styleUrls: ['./delete-image.component.css']
})
export class DeleteImageComponent implements OnInit{

  imageUrls: Array<string> = [];
  deleteImageForm: FormGroup;
  selectedImageUrl = '';

  constructor(
    private rs: RoomService,
    private dialogRef: MatDialogRef<DeleteImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {imageUrls: Array<string>}
    ){
      this.deleteImageForm = new FormBuilder().group({
        imageUrl: new FormControl('')
      })
  }

  ngOnInit(): void {
    this.imageUrls = this.data.imageUrls
    this.deleteImageForm.get('imageUrl')?.setValue(this.imageUrls[0])
    this.selectedImageUrl = this.imageUrls[0];
  }

  async onSubmit(){
      await this.rs.deleteImageOfRoom(this.deleteImageForm.get('imageUrl')?.value)
      .then(res => {
        this.dialogRef.close('changed')
      })
      .catch(err => validationHandler(err,this.deleteImageForm))
  }

  closeEvent(){
    this.dialogRef.close('closed')
  }
}
