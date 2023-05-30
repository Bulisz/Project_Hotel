import { Component, Input, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
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
  public dateToday: Date | undefined;
  public monthToday: number | undefined;
  public thisMonth: string | undefined;
  public monthNames = ["Január", "Február", "Március", "Április", "Május", "Június",
  "Július", "Augusztrus", "Szeptember", "Október", "November", "December"];
  public weekDays = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"]
  public firstDayOfMonth: Date | undefined;
  public yearNow: number | undefined;
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


  constructor(private accountService: AccountService,
    private calendarService: CalendarService,
    private router: Router,
    public dialogRef: MatDialogRef<RoomCalendarComponent>,
    private rs: RoomService,
    private as: AccountService){}
    


    async ngOnInit() {

      this.dateToday = new Date(2023, 6, 6);
      // this.dateToday = new Date(Date.now());
      this.monthToday = this.dateToday.getMonth();
      this.yearNow = this.dateToday.getFullYear();
      this.thisMonth = this.monthNames[this.monthToday - 1];
      this.firstDayOfMonth = new Date(this.yearNow, this.monthToday, 1);
      this.lastOfMonth = new Date(this.yearNow, this.monthToday+1, 0);
      
      await this.as.getUsers()
        .then(res => this.users = res)
        .catch(err => console.log(err))
  
      this.rs.getAllRooms().subscribe({
        next: res => this.rooms = res,
        error: err => console.log(err)
      })

      this.calendarService.getAllDaysOfMonth().subscribe({
        next: res => {
          this.daysOfthisMonth = res;
        console.log(this.daysOfthisMonth);
        this.rows = Math.ceil((this.daysOfthisMonth.length + (this.daysOfthisMonth[0].weekDayNumber-1)) /7);

          if (this.daysOfthisMonth[0].weekDayNumber != 1){

        let i: number = 0;
        while (this.daysOfthisMonth[i].weekDayNumber != 1) {
            this.firstMondayNumber++;
            i++;
        }
       } else {
        this.firstMondayNumber = 0;
       }
        console.log(this.firstMondayNumber)

        for (let i = 0; i < this.rows; i++) {
          this.calendar[i] = [];
          for (let j = 0; j < 7; j++) {
            this.calendar[i][j] =  {
              day: new Date(),
              dateNumber: 0,
              weekDayNumber: 0,
          };
        }
      }
        if (this.firstMondayNumber != 0) {
          for (let i = 0; i < this.firstMondayNumber; i++) {
            this.calendar[0][this.daysOfthisMonth[i].weekDayNumber -1] = this.daysOfthisMonth[i];
          }
        
          for (let r = 1; r < this.rows - 1; r++){

            for (let i = (this.firstMondayNumber) + (r-1)*7, k = 0; k < 7; i++, k++) {
              this.calendar[r][k] = this.daysOfthisMonth[i];
            }
          }
          let column = 0;
          for (let i = this.firstMondayNumber + (this.rows - 2)*7; i < this.daysOfthisMonth.length; i++) {
            
            this.calendar[this.rows - 1][column] = this.daysOfthisMonth[i];
            column++;
          }
          console.log(this.calendar)

        } else {
          for (let r = 0; r < this.rows - 1; r++){

            for (let i = 0 + (r*7), k = 0; i < this.numberOfDaysPerWeek +(r*7); i++, k++) {
              this.calendar[r][k] = this.daysOfthisMonth[i];
            }
          }
          let column = 0;
          for (let i = this.firstMondayNumber  + ((this.rows - 1) * this.numberOfDaysPerWeek); i < this.daysOfthisMonth.length; i++) {
           
            this.calendar[this.rows - 1][column] = this.daysOfthisMonth[i];
            column++;
          }
          console.log(this.calendar)

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

  closeCalendar() {
    this.dialogRef.close('ok')
  }
}