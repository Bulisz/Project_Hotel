import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import { RoomListModel } from '../models/room-list.model';
import { RoomDetailsModel } from '../models/room-details-model';
import { environment } from '../../environments/environment';
import { NonStandardEquipmentModel } from '../models/non-standard-equipment-model';


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


  fetchNonStandardEquipmentData(): Observable<Array<NonStandardEquipmentModel>> {
    return this.http.get<Array<NonStandardEquipmentModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetNonStandardEquipments`);
  }

  getRoomOptions(parsoltFormValue: any): Observable<Array<RoomListModel>> {
      let params  = new HttpParams();
      params = params.append('guest', String(parsoltFormValue.guestNumber));
      params = params.append('dog', String(parsoltFormValue.dogNumber));
      params = params.append('bookingFrom', String(parsoltFormValue.bookingFrom));
      params = params.append('bookingTo', String(parsoltFormValue.bookingTo));
      parsoltFormValue.nonStandardEquipments.forEach((element: string) => {
        params = params.append('choosedEquipments', String(element));
      });
     
      
    return this.http.get<Array<RoomListModel>>(`${environment.apiUrl}/${this.BASE_URL}/getAvailableRooms`, {params: params});
  }

}
