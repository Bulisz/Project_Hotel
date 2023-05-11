import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import { RoomListModel } from '../models/room-list.model';
import { RoomDetailsModel } from '../models/room-details-model';
import { environment } from '../../environments/environment';

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
}
