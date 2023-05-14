import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import { RoomListModel } from '../models/room-list.model';
import { RoomDetailsModel } from '../models/room-details-model';
import { environment } from '../../environments/environment';


const BASE_URL = 'https://localhost:5001/hotel/rooms'

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  BASE_URL = 'rooms'

  constructor(private http: HttpClient) { }

  getAllRooms(): Observable<Array<RoomListModel>> {
    return this.http.get<Array<RoomListModel>>(`${environment.apiUrl}/${this.BASE_URL}/getlistofrooms`);
  }

  async getRoomById(id: number): Promise<RoomDetailsModel> {
    return await firstValueFrom(this.http.get<RoomDetailsModel>(`${environment.apiUrl}/${this.BASE_URL}/getroombyid/${id}`))
  }

  getRoomOptions(guestNumber: number, dogNumber: number, bookingFrom: Date, bookingTo: Date): Observable<Array<RoomListModel>> {
      let params  = new HttpParams();
      params = params.append('guest', String(guestNumber));
      params = params.append('dog', String(dogNumber));
      params = params.append('bookingFrom', String(bookingFrom));
      params = params.append('bookingTo', String(bookingTo));
      console.log(params)
    return this.http.get<Array<RoomListModel>>(`${BASE_URL}/getAvailableRooms`, {params: params});
  }

}
