import { Component, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import {
  ApexAxisChartSeries,
  ApexChart,
  ChartComponent,
  ApexDataLabels,
  ApexPlotOptions,
  ApexYAxis,
  ApexTitleSubtitle,
  ApexXAxis,
  ApexFill
} from "ng-apexcharts";
import { RoomListModel } from "src/app/models/room-list.model";
import { RoomResMonthModel } from "src/app/models/room-res-month-model";
import { RoomService } from "src/app/services/room.service";
import { StatisticsService } from "src/app/services/statistics.service";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  fill: ApexFill;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-stat-month-bar',
  templateUrl: './stat-month-bar.component.html',
  styleUrls: ['./stat-month-bar.component.css']
})
export class StatMonthBarComponent {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: ChartOptions;

  monthNumber: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  monthString: Array<string> = ["Január", "Február", "Március", "Április", "Május", "Június",
  "Július", "Augusztrus", "Szeptember", "Október", "November", "December"];

  rooms!: Array<RoomListModel>
  roomsForDiagram: FormGroup;
  dateToday: Date | undefined;
  yearToday: number = 2023;
  data: Array<RoomResMonthModel> = [];

  constructor(private rs: RoomService, 
              private formBuilder: FormBuilder,
              private statisticsService: StatisticsService) {

    this.roomsForDiagram = this.formBuilder.group({
      rooms: [''],
      months: [''],
      
    })

    this.chartOptions = {
      series: [
        {
          name: "Inflation",
          data: [2.3, 3.1, 4.0, 10.1, 4.0, 3.6, 3.2],
          color: "#800020"
        }
      ],
      chart: {
        height: 350,
        type: "bar"
      },
      plotOptions: {
        bar: {
          dataLabels: {
            position: "top" // top, center, bottom
          }
        }
      },
      dataLabels: {
        enabled: true,
        formatter: function(val) {
          return val + "%";
        },
        offsetY: -20,
        style: {
          fontSize: "12px",
          colors: ["#304758"]
        }
      },

      xaxis: {
        categories: [
          "Jan",
          "Feb",
          "Mar",
          "Apr",
          "May",
          "Jun",
          "Jul",
          
        ],
        position: "top",
        labels: {
          offsetY: -18
        },
        axisBorder: {
          show: false
        },
        axisTicks: {
          show: false
        },
        crosshairs: {
          fill: {
            type: "gradient",
            gradient: {
              colorFrom: "#D8E3F0",
              colorTo: "#BED1E6",
              stops: [0, 100],
              opacityFrom: 0.4,
              opacityTo: 0.5
            }
          }
        },
        tooltip: {
          enabled: true,
          offsetY: -35
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "light",
          type: "horizontal",
          shadeIntensity: 0.25,
          gradientToColors: undefined,
          inverseColors: true,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [50, 0, 100, 100]
        }
      },
      yaxis: {
        axisBorder: {
          show: false
        },
        axisTicks: {
          show: false
        },
        labels: {
          show: false,
          formatter: function(val) {
            return val + "%";
          }
        }
      },
      title: {
        text: "Szobák kihasználtsága Januárban",
        floating: false,
        offsetY: 320,
        align: "center",
        style: {
          color: "#444"
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

  onSubmit(){
    this.statisticsService.getRoomMonthStat(this.yearToday, this.roomsForDiagram.value).subscribe({
      next: res => {
        this.data = res;
        console.log(this.roomsForDiagram.value)
      },
      
      error: err => console.log(err)
  });
        
    
  }
}
