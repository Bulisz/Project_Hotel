import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateEventModel } from '../models/create-event-model';
import { environment } from 'src/environments/environment';
import { EventDetailsModel } from '../models/event-details-model';
import {  Observable, firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  BASE_URL = 'events'

  constructor(private http: HttpClient) { }

  async createEvent(event: CreateEventModel): Promise<EventDetailsModel> {

    const formData: FormData = new FormData();
    formData.append('title', event.title);
    formData.append('text', event.text);
    formData.append('schedule', event.schedule);
    formData.append('image', event.image);

    return await firstValueFrom(this.http.post<EventDetailsModel>(`${environment.apiUrl}/${this.BASE_URL}/CreateEvent`,formData))
  }

  getAllEvents(): Observable<Array<EventDetailsModel>> {
    return this.http.get<Array<EventDetailsModel>>(`${environment.apiUrl}/${this.BASE_URL}/getlistofevents`);
  }
}
