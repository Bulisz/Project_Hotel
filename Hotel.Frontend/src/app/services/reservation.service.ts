import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ReservationDetailsModel } from '../models/reservation-details-model';
import { ReservationModel } from '../models/reservation-model';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  BASE_URL = 'reservations'

  constructor(private http: HttpClient) { }

  async createReservationForRoom(reservation: ReservationModel): Promise<ReservationDetailsModel> {
    return await firstValueFrom(this.http.post<ReservationDetailsModel>(`${environment.apiUrl}/${this.BASE_URL}/createReservationForRoom`,reservation))
  }
}
