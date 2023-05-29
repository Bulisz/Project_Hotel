import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-room-calendar',
  templateUrl: './room-calendar.component.html',
  styleUrls: ['./room-calendar.component.css']
})
export class RoomCalendarComponent implements OnInit {

  constructor(private accountService: AccountService,
    private router: Router,
    public dialogRef: MatDialogRef<RoomCalendarComponent>){}


  ngOnInit(): void {}

  closeCalendar() {
    this.dialogRef.close('ok')
  }
}