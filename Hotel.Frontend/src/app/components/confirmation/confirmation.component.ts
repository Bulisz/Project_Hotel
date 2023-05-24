import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent implements OnInit{

  message = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: {message: string}, public dialogRef: MatDialogRef<ConfirmationComponent>,){
  }

  ngOnInit(): void {
    this.message = this.data.message;
  }

  agree(){
    this.dialogRef.close('agree')
  }

  cancel(){
    this.dialogRef.close('cancel')
  }
}
