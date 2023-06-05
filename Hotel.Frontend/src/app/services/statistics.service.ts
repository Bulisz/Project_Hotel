import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RoomResMonthModel } from '../models/room-res-month-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { YearStatModel } from '../models/year-stat-model';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  BASE_URL = 'statistics'

  constructor(private http: HttpClient) { }

  getRoomMonthStat(year: number, month: number): Observable<Array<RoomResMonthModel>> {
    return this.http.get<Array<RoomResMonthModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetRoomMonthStat/${year}/${month}`);
  }

  getYearStatistics(formValue: any): Observable<Array<YearStatModel>> {
    let params  = new HttpParams();
    params = params.append('year', String(formValue.year));
   
    formValue.rooms.forEach((element: string) => {
      params = params.append('choosedRooms', String(element));
    });

  return this.http.get<Array<YearStatModel>>(`${environment.apiUrl}/${this.BASE_URL}/getYearStat`, {params: params});
}

}
