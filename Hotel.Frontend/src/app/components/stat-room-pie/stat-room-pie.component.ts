import { Component, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";

import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ApexStroke,
  ApexFill
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
 
  stroke: ApexStroke;
  fill: ApexFill;
};

@Component({
  selector: 'app-stat-room-pie',
  templateUrl: './stat-room-pie.component.html',
  styleUrls: ['./stat-room-pie.component.css']
})
export class StatRoomPieComponent {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: ChartOptions;

  constructor() {
    this.chartOptions = {
     
      series: [14, 23, 21],
      chart: {
        type: "polarArea"
      },
      stroke: {
        colors: ["#fff"]
      },
      fill: {
        opacity: 0.8,
        colors: ['#AF9E66', '#6B3923', '#9A7294'],
        image: {
          src: ['https://res.cloudinary.com/ddaprhl4e/image/upload/v1683705423/Hotel/Logos/background_j8kb3g.webp'],
        }
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200
            },
            legend: {
              position: "bottom"
            }
          }
        }
      ]
      
    };
  }
}
