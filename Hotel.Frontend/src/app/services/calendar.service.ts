import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DayOfMonthModel } from '../models/day-of-month-model';
import { Observable, firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CalendarService {

  BASE_URL = 'reservations'

  constructor(private http: HttpClient) { }

  getAllDaysOfMonth(year: number, month: number): Observable<Array<DayOfMonthModel>> {
    return this.http.get<Array<DayOfMonthModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetThisMonthCalendar/${year}/${month}`);
  }
}
