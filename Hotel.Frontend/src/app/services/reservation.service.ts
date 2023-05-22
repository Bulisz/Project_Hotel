import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ReservationDetailsModel } from '../models/reservation-details-model';
import { ReservationModel } from '../models/reservation-model';
import { PostModel } from '../models/post-model';
import { PostDetailsModel } from '../models/post-details-model';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  BASE_URL = 'reservations'

  constructor(private http: HttpClient) { }

  async createPost(post: PostModel): Promise<PostModel> {
    return await firstValueFrom(this.http.post<PostModel>(`${environment.apiUrl}/${this.BASE_URL}/createPost`,post))
  }

  async getAllPosts(): Promise<Array<PostDetailsModel>> {
    return await firstValueFrom(this.http.get<Array<PostDetailsModel>>(`${environment.apiUrl}/${this.BASE_URL}/getAllPosts`))
  }

  async createReservationForRoom(reservation: ReservationModel): Promise<ReservationDetailsModel> {
    return await firstValueFrom(this.http.post<ReservationDetailsModel>(`${environment.apiUrl}/${this.BASE_URL}/createReservationForRoom`,reservation))
  }
}
