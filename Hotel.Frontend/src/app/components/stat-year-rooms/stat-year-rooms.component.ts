import { Component, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";

import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexStroke,
  
} from "ng-apexcharts";

import { RoomListModel } from "src/app/models/room-list.model";
import { YearStatModel } from "src/app/models/year-stat-model";
import { RoomService } from "src/app/services/room.service";
import { StatisticsService } from "src/app/services/statistics.service";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
};

@Component({
  selector: 'app-stat-year-rooms',
  templateUrl: './stat-year-rooms.component.html',
  styleUrls: ['./stat-year-rooms.component.css']
})
export class StatYearRoomsComponent  {
  monthNumber: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  monthString: Array<string> = ["január", "február", "március", "április", "május", "június",
  "július", "augusztrus", "szeptember", "október", "november", "december"];

  rooms!: Array<RoomListModel>
  roomsForDiagram: FormGroup;
  yearsForStat: Array<number> = [];
  dateToday: Date | undefined;
  foundationYear: number = 2022;
  yearToday: number = new Date().getFullYear();
  nextYear: number = new Date().getFullYear() + 1;
  monthToday: number = new Date().getMonth()+1;
  preSelectedRooms: Array<string> = ['Bodri', 'Buksi', 'Morzsa'];
  roomNames: Array<string> = [];
  data: Array<YearStatModel> =[];
  dogHotelColors: Array<string> = ['#AF9E66', '#6B3923', '#9A7294', '#C7A086', '#55171E', '#915934', '#A4255C', '#3F221B', '#800020'];



  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: ChartOptions;
  

  constructor(private rs: RoomService, 
              private statisticsService: StatisticsService,
              private formBuilder: FormBuilder) {

    this.roomsForDiagram = this.formBuilder.group({
      rooms: [this.preSelectedRooms],
      year: [this.yearToday],
      
    })


    this.chartOptions = {
      
      series: this.data,
      chart: {
        height: 350,
        type: "area"
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        type: "category",
        categories: this.monthString
      },
      tooltip: {
        x: {
          format: "dd/MM/yy HH:mm"
        }
      }
      
    };
  }

  async ngOnInit() {
    await this.loadRooms()

    this.dateToday = new Date(Date.now());
    this.yearToday = this.dateToday.getFullYear();
    this.monthToday = this.dateToday.getMonth()+1;
    let yearsOfDogHotel = this.nextYear - this.foundationYear;
    for (let i = 0; i <= yearsOfDogHotel; i++) {
      this.yearsForStat[i] = this.foundationYear + i;
    }

    this.statisticsService.getYearStatistics(this.roomsForDiagram.value).subscribe({
      next: res => {
        this.data = res
        for (let i = 0; i < this.data.length; i++) {
          let j = i % 8;
          this.data[i].color = this.dogHotelColors[j];
          
        }
        this.generateDate();
      },
      error: err => console.log(err)
      });


  }

  async loadRooms() {
    try {
      const res = await this.rs.getAllRooms();
      this.rooms = res;
  
      for (let i = 0; i < this.rooms.length; i++) {
        this.roomNames[i] = this.rooms[i].name;
      }
    } catch (err) {
      console.log(err);
    }
  }

  onSubmit(){
    
    this.statisticsService.getYearStatistics(this.roomsForDiagram.value).subscribe({
      next: res => {
        this.data = res
        for (let i = 0; i < this.data.length; i++) {
          this.data[i].color = this.dogHotelColors[i];
        }
        this.generateDate();
      },
      error: err => console.log(err)
      });
      
  }

  generateDate() {
    this.chartOptions = {
      
      series: this.data,
      chart: {
        height: 350,
        type: "area"
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        type: "category",
        categories: this.monthString
      },
      tooltip: {
        x: {
          format: "dd/MM/yy HH:mm"
        }
      }
      
    };
  }
}

