import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EquipmentModel } from '../models/equipment-model';
import { environment } from 'src/environments/environment';
import {  firstValueFrom } from 'rxjs';
import { CreateEquipmentModel } from '../models/create-equipment-model';
import { EquipmentRoomModel } from '../models/equipment-room-model';

@Injectable({
  providedIn: 'root'
})
export class EquipmentService {

  BASE_URL = 'equipments'

  constructor(private http: HttpClient) { }

  async getNonStandardEquipments(): Promise<Array<EquipmentModel>> {
    return firstValueFrom(this.http.get<Array<EquipmentModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetNonStandardEquipments`));
  }

  async getStandardEquipments(): Promise<Array<EquipmentModel>> {
    return firstValueFrom(this.http.get<Array<EquipmentModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetStandardEquipments`));
  }

  async createEquipment(equipment: CreateEquipmentModel): Promise<EquipmentModel> {
    return firstValueFrom(this.http.post<EquipmentModel>(`${environment.apiUrl}/${this.BASE_URL}/CreatedEquipment`,equipment));
  }

  async deleteEquipment(id: number): Promise<any> {
    return firstValueFrom(this.http.delete(`${environment.apiUrl}/${this.BASE_URL}/DeleteEquipment/${id}`));
  }

  async addEquipmentToRoom(model: EquipmentRoomModel): Promise<any> {
    return firstValueFrom(this.http.put(`${environment.apiUrl}/${this.BASE_URL}/AddEquipmentToRoom`,model));
  }

  async removeEquipmentFromRoom(model: EquipmentRoomModel): Promise<any> {
    return firstValueFrom(this.http.put(`${environment.apiUrl}/${this.BASE_URL}/RemoveEquipmentFromRoom`,model));
  }
}
