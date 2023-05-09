import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { RegisterModel } from '../models/register-model';
import { LoginrequestModel } from '../models/loginrequest-model';
import { LoginresponseModel } from '../models/loginresponse-model';
import { UserModel } from '../models/user-model';

const BASE_URL = 'https://localhost:5001/hotel/Users'

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  user = new BehaviorSubject<UserModel | null>(null)

  constructor(private http: HttpClient) { }

  async registerNewAccout(newAccount: RegisterModel): Promise<any> {
    return await firstValueFrom(this.http.post(`${BASE_URL}/Register`, newAccount));
  }

  async login(loginData: LoginrequestModel): Promise<any> {
    return await firstValueFrom(this.http.post<LoginresponseModel>(`${BASE_URL}/Login`, loginData))
      .then((res) => {
        if(res.token){
          localStorage.setItem('accessToken', res.token);
          this.user.next({id: res.id, role: res.role , userName: res.userName})
        }
      })
  }

  logout(){
    localStorage.removeItem('accessToken');
    this.user.next(null)
  }

}
