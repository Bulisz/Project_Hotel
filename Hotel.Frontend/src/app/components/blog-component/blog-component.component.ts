import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-blog-component',
  templateUrl: './blog-component.component.html',
  styleUrls: ['./blog-component.component.css']
})
export class BlogComponentComponent implements OnInit{

  currentUser: any;
  postForm: FormGroup;

  constructor(private as: AccountService, private rs: ReservationService){
    this.postForm = new FormBuilder().group({
      text: new FormControl('' , Validators.required)
    })
  }

  ngOnInit(): void {
    this.as.user.subscribe({
      next: (res) => this.currentUser=res
    })
  }



}
