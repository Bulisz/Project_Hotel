import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoomListModel } from '../models/room-list.model';

const BASE_URL = 'https://localhost:5001/hotel'

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private http: HttpClient) { }

  getAllRooms(): Observable<Array<RoomListModel>> {
    return this.http.get<Array<RoomListModel>>(`${BASE_URL}/rooms/getlistofrooms`);
  }
}
