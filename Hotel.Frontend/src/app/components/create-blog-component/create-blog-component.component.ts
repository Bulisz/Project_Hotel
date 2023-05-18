import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-create-blog-component',
  templateUrl: './create-blog-component.component.html',
  styleUrls: ['./create-blog-component.component.css']
})
export class CreateBlogComponentComponent{

  postForm: FormGroup;

  constructor(private as: AccountService, private rs: ReservationService, public dialogRef: MatDialogRef<CreateBlogComponentComponent>,){
    this.postForm = new FormBuilder().group({
      text: new FormControl('' , Validators.required)
    })
  }

  async createPost(){
    const formValue = this.postForm.getRawValue()
    const parsedFormValue = {...formValue, userName: this.as.user.value?.userName, role: this.as.user.value?.role}

    await this.rs.createPost(parsedFormValue)
      .then(() => {})
      .catch((err) => validationHandler(err,this.postForm))
  }

  closeDialog(){
    this.dialogRef.close()
  }
}
