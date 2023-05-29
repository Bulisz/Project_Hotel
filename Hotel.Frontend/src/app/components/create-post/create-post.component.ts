import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { PostService } from 'src/app/services/post.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent{

  postForm: FormGroup;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {userName: string, role: string}, private postService: PostService, public dialogRef: MatDialogRef<CreatePostComponent>,){
    this.postForm = new FormBuilder().group({
      text: new FormControl('' , [Validators.required, Validators.minLength(2), Validators.maxLength(1000)])
    })
  }

  async onSubmit(){
    const formValue = this.postForm.getRawValue()
    const parsedFormValue = {...formValue, userName: this.data.userName, role: this.data.role}
    await this.postService.createPost(parsedFormValue)
      .then(() => {})
      .catch((err) => validationHandler(err,this.postForm))
    this.dialogRef.close()
  }

  closePost(){
    this.dialogRef.close()
  }
}
