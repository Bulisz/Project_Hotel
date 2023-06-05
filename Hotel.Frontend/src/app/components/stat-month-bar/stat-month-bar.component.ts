import { Component, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
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
  monthString: Array<string> = ["január", "február", "március", "április", "május", "június",
  "július", "augusztrus", "szeptember", "október", "november", "december"];

  rooms!: Array<RoomListModel>
  roomsForDiagram: FormGroup;
  dateToday: Date | undefined;
  foundationYear: number = 2022;
  yearToday: number = new Date().getFullYear();
  nextYear: number = new Date().getFullYear() + 1;
  monthToday: number = new Date().getMonth()+1;
  data: Array<RoomResMonthModel> = [];
  dataValues: Array<number> = [];
  dataRoomNumbers: Array<number> = [];
  dataRoomStrings: Array<string> = [];
  yearsForStat: Array<number> = [];
  title: string = '';
  
  constructor(private rs: RoomService, 
              private formBuilder: FormBuilder,
              private statisticsService: StatisticsService) {
                
    this.roomsForDiagram = this.formBuilder.group({
      months: [this.monthString[this.monthToday-1]],         
      year: [this.yearToday],
                  
    })  

    this.chartOptions = {
      series: [
        {
          name: "Szobakihasználtság",
          data: [0, 0, 0, 0, 0, 0, 0],
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
            position: "bottom" // top, center, bottom
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
        categories: this.dataRoomStrings,
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
        text: this.title,
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
    
    this.dateToday = new Date(Date.now());
    this.yearToday = this.dateToday.getFullYear();
    this.monthToday = this.dateToday.getMonth()+1;
    let yearsOfDogHotel = this.nextYear - this.foundationYear;
    for (let i = 0; i <= yearsOfDogHotel; i++) {
      this.yearsForStat[i] = this.foundationYear + i;
    }

    


    this.statisticsService.getRoomMonthStat(this.yearToday, this.monthToday).subscribe({
      next: res => {
        this.data = res;
        
        for (let i = 0; i < this.data.length; i++) {
          this.dataValues[i] = this.data[i].percentage;
          this.dataRoomNumbers[i] = this.data[i].id;
          this.dataRoomStrings[i] = this.data[i].name;
        }

        if (this.monthToday <= 8) {
        this.title = 'Szobák kihasználtsága ' + this.yearToday + ' ' + this.monthString[this.monthToday-1] + 'ban';
        }else{
          this.title = 'Szobák kihasználtsága ' + this.yearToday + ' ' + this.monthString[this.monthToday-1] + 'ben';
        }
        this.generateDate();

      },
      error: err => console.log(err)
    });
    
  }

  

  onSubmit(){
   
    for (let i = 0; i < this.monthString.length-1; i++) {
      if (this.monthString[i] == this.roomsForDiagram.controls['months'].value) {
        this.monthToday = i+1;
      }
    }

   
    this.yearToday = Number(this.roomsForDiagram.controls['year'].value);
   
    this.statisticsService.getRoomMonthStat(this.yearToday, this.monthToday).subscribe({
      next: res => {
        this.data = res;
        
        for (let i = 0; i < this.data.length; i++) {
          this.dataValues[i] = this.data[i].percentage;
          this.dataRoomNumbers[i] = this.data[i].id;
          this.dataRoomStrings[i] = this.data[i].name;
        }

        if (this.monthToday <= 8) {
          this.title = 'Szobák kihasználtsága ' + this.yearToday + ' ' + this.monthString[this.monthToday-1] + 'ban';
          }else{
            this.title = 'Szobák kihasználtsága ' + this.yearToday + ' ' + this.monthString[this.monthToday-1] + 'ben';
          }
        this.generateDate();

      },
      error: err => console.log(err)
    });
        
    
  }

  generateDate() {
    this.chartOptions = {
      series: [
        {
          name: "Foglaltság",
          data: this.dataValues,
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
        categories: this.dataRoomStrings,
        position: "bottom",
        labels: {
          offsetY: -5
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
          show: true
        },
        axisTicks: {
          show: true
        },
        labels: {
          show: true,
          formatter: function(val) {
            return val + "%";
          }
        },
        max: 100
      },
      title: {
        text: this.title,
        floating: false,
        offsetY: 330,
        align: "center",
        style: {
          color: "#444"
        },
        
      }
    };

  }

}
