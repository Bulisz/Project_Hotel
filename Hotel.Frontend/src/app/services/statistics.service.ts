import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RoomResMonthModel } from '../models/room-res-month-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  BASE_URL = 'statistics'

  constructor(private http: HttpClient) { }

  getRoomMonthStat(year: number, month: number): Observable<Array<RoomResMonthModel>> {
    return this.http.get<Array<RoomResMonthModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetRoomMonthStat/${year}/${month}`);
  }
}
