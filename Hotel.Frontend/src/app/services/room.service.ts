import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import { RoomListModel } from '../models/room-list.model';
import { RoomDetailsModel } from '../models/room-details-model';

const BASE_URL = 'https://localhost:5001/hotel/rooms'

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private http: HttpClient) { }

  getAllRooms(): Observable<Array<RoomListModel>> {
    return this.http.get<Array<RoomListModel>>(`${BASE_URL}/getlistofrooms`);
  }

  async getRoomById(id: number): Promise<RoomDetailsModel> {
    return await firstValueFrom(this.http.get<RoomDetailsModel>(`${BASE_URL}/GetRoomById/${id}`))
  }
}
