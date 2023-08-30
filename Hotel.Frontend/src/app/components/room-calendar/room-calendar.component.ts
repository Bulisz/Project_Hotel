import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DailyReservationModel } from 'src/app/models/daily-reservation-model';
import { DayOfMonthModel } from 'src/app/models/day-of-month-model';
import { DaysModel } from 'src/app/models/days-model';
import { ReservationListItem } from 'src/app/models/reservation-list-item';
import { RoomListModel } from 'src/app/models/room-list.model';
import { UserListModel } from 'src/app/models/user-list-model';
import { AccountService } from 'src/app/services/account.service';
import { CalendarService } from 'src/app/services/calendar.service';
import { RoomService } from 'src/app/services/room.service';


@Component({
  selector: 'app-room-calendar',
  templateUrl: './room-calendar.component.html',
  styleUrls: ['./room-calendar.component.css']
})
export class RoomCalendarComponent implements OnInit {
  
  @Input() reservationList!: Array<ReservationListItem>;

  numberOfDaysPerWeek = 7;
  numberOfMonthsPerYear = 12;
  public dateToday: Date | undefined;
  thisYear: number = 0;
  monthToday: number = 0;
  public thisMonth: string | undefined;
  public monthNames = ["Január", "Február", "Március", "Április", "Május", "Június",
  "Július", "Augusztus", "Szeptember", "Október", "November", "December"];
  public weekDays = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"]
  public firstDayOfMonth: Date | undefined;
  public yearNow: number =0;
  public lastOfMonth: Date | undefined;
  daysOfthisMonth: Array<DayOfMonthModel> = [];
  rows: number | null | undefined;
  users: Array<UserListModel> = [];
  rooms: Array<RoomListModel> = [];
  calendar: any[][] = [];
  firstMonday: DaysModel = {
    day: new Date(),
    dateNumber: 0,
    weekDayNumber: 0,
  }
  firstMondayNumber: number = 0;
  nextMonthName: string | undefined;
  previousMonthName: string | undefined;

  constructor(private calendarService: CalendarService,
    public dialogRef: MatDialogRef<RoomCalendarComponent>,
    private rs: RoomService,
    private as: AccountService){}
    
  async ngOnInit() {

    await this.as.getUsers()
      .then(res => this.users = res)
      .catch(err => console.log(err))

    await this.rs.getAllRooms()
      .then( res => this.rooms = res)
      .catch(err => console.log(err))

    this.dateToday = new Date(Date.now());
        
    this.showCalendar(this.dateToday);
  }

  closeCalendar() {
    this.dialogRef.close('ok')
  }

  showCalendar(dateToday: Date) {

      this.monthToday = dateToday.getMonth()+1;
      this.thisMonth = this.monthNames[this.monthToday-1];
      this.yearNow = dateToday.getFullYear();
      this.nextMonthName = this.monthNames[this.monthToday % this.numberOfMonthsPerYear];

      if (this.monthToday-2 >= 0) {
        this.previousMonthName = this.monthNames[(this.monthToday-2)];
      }else{
        this.previousMonthName = this.monthNames[(this.monthToday-2 + this.numberOfMonthsPerYear)];
      }
      
      this.calendarService.getAllDaysOfMonth(this.yearNow, this.monthToday).subscribe({
        next: res => {
          this.daysOfthisMonth = res;
       console.log(this.daysOfthisMonth)
        this.rows = Math.ceil((this.daysOfthisMonth.length + (this.daysOfthisMonth[0].weekDayNumber-1)) /7);
          this.firstMondayNumber = this.numberOfDaysPerWeek - this.daysOfthisMonth[0].weekDayNumber + 1
             
          this.calendar = [];
        for (let i = 0; i < this.rows; i++) {
          this.calendar[i] = [];
          for (let j = 0; j < this.numberOfDaysPerWeek; j++) {
            this.calendar[i][j] =  {
              day: new Date(),
              dateNumber: 0,
              weekDayNumber: 0,
          };
        }
      }
        
      for (let i = 0; i < this.firstMondayNumber; i++) {
        this.calendar[0][this.daysOfthisMonth[i].weekDayNumber -1] = this.daysOfthisMonth[i];
      }
      for (let r = 1; r < this.rows - 1; r++){
        for (let i = (this.firstMondayNumber) + (r-1)*this.numberOfDaysPerWeek, k = 0; k < this.numberOfDaysPerWeek; i++, k++) {
          this.calendar[r][k] = this.daysOfthisMonth[i];
        }
      }
      let column = 0;
      for (let i = this.firstMondayNumber + (this.rows - 2)*this.numberOfDaysPerWeek; i < this.daysOfthisMonth.length; i++) {
        this.calendar[this.rows - 1][column] = this.daysOfthisMonth[i];
        column++;
      }
      },
        error: err => console.log(err)
      })
        
  }

  getColor(roomNumber: number) {
    switch (roomNumber) { 
      case 2:
      return '#AF9E66';
      case 3:
      return '#6B3923';
      case 4:
      return '#9A7294';
      case 5:
      return '#C7A086';
      case 6:
      return '#55171E';
      case 7:
      return '#915934';
      case 9:
      return '#A4255C';
      default:
      return '#3F221B';
    }
  }

  nextMonth(direction: number) {
    this.dateToday = new Date(this.yearNow, this.monthToday - 1 + direction, 1);
    this.showCalendar(this.dateToday);
  }
}