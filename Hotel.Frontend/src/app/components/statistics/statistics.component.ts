import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { DayOfMonthModel } from "src/app/models/day-of-month-model";



import { RoomListModel } from "src/app/models/room-list.model";
import { RoomResMonthModel } from "src/app/models/room-res-month-model";
import { CalendarService } from "src/app/services/calendar.service";
import { RoomService } from "src/app/services/room.service";



@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  monthNumber: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  monthString: Array<string> = ["Január", "Február", "Március", "Április", "Május", "Június",
  "Július", "Augusztrus", "Szeptember", "Október", "November", "December"];

  rooms!: Array<RoomListModel>
  roomsForDiagram: FormGroup;
  dateToday: Date | undefined;
  daysOfthisMonth: Array<DayOfMonthModel> = [];
  data: Array<RoomResMonthModel> = [];


  constructor(private rs: RoomService, 
              private formBuilder: FormBuilder,
              private calendarService: CalendarService) {

    this.roomsForDiagram = this.formBuilder.group({
      rooms: [''],
      months: [''],
      
    })

  }
    
  async ngOnInit() {
    await this.loadRooms()
  }

  async loadRooms(){
    await this.rs.getAllRooms()
      .then(res => this.rooms = res)
      .catch(err =>  console.log(err))
  }

  onSubmit(){
    this.dateToday = new Date(Date.now());
        console.log(this.dateToday.getMonth())
    this.calendarService.getAllDaysOfMonth(this.dateToday.getFullYear(), this.dateToday.getMonth() + 1).subscribe({
      next: res => {
        this.daysOfthisMonth = res;

        for (let h = 0; h < this.rooms.length; h++) {
          let counter = 0;
          for (let i = 0; i < this.daysOfthisMonth.length; i++) {
             for (let j = 0; j < this.daysOfthisMonth[i].roomStatus.length; j++) {
               if (this.daysOfthisMonth[i].roomStatus[j].roomNumber == this.rooms[h].id) {
                  counter++;
               }
             }
          }
          
          // this.data.push(new RoomResMonthModel{
          //   id : this.rooms[h].id,
          //   name: this.rooms[h].name,
          //   percentage: counter
          // });

        }
      },
      error: err => console.log(err)
    })
  }

 
}