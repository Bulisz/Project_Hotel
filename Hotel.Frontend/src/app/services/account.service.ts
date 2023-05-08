import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { RegisterModel } from '../models/register-model';

const BASE_URL = 'https://localhost:5001/hotel/Users'

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  async registerNewAccout(newAccount: RegisterModel): Promise<any> {
    return await firstValueFrom(this.http.post(`${BASE_URL}/Register`, newAccount));
  }
}
