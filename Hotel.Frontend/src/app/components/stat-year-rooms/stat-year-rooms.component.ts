import { Component, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";

import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexStroke
} from "ng-apexcharts";

import { RoomListModel } from "src/app/models/room-list.model";
import { RoomService } from "src/app/services/room.service";

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
  monthString: Array<string> = ["Január", "Február", "Március", "Április", "Május", "Június",
  "Július", "Augusztrus", "Szeptember", "Október", "November", "December"];

  rooms!: Array<RoomListModel>
  roomsForDiagram: FormGroup;





  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: ChartOptions;

  constructor(private rs: RoomService, private formBuilder: FormBuilder) {

    this.roomsForDiagram = this.formBuilder.group({
      rooms: [''],
      months: [''],
      
    })


    this.chartOptions = {
      series: [
        {
          name: "series1",
          data: [31, 40, 28, 51, 42, 109, 100],
          color: "#AF9E66"
        },
        {
          name: "series2",
          data: [11, 32, 45, 32, 34, 52, 41],
          color: "#55171E"
        },
        {
          name: "series2",
          data: [70, 62, 32, 12, 27, 60, 48],
          color: "#A4255C"
        }
      ],
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
        type: "datetime",
        categories: [
          "2018-09-19T00:00:00.000Z",
          "2018-09-19T01:30:00.000Z",
          "2018-09-19T02:30:00.000Z",
          "2018-09-19T03:30:00.000Z",
          "2018-09-19T04:30:00.000Z",
          "2018-09-19T05:30:00.000Z",
          "2018-09-19T06:30:00.000Z"
        ]
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
  }

  async loadRooms(){
    await this.rs.getAllRooms()
      .then(res => this.rooms = res)
      .catch(err =>  console.log(err))
  }

  onSubmit(){}

 
}

