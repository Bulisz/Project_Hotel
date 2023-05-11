import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { RegisterModel } from '../models/register-model';
import { LoginrequestModel } from '../models/loginrequest-model';
import { LoginresponseModel } from '../models/loginresponse-model';
import { UserModel } from '../models/user-model';
import { environment } from '../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class AccountService {

  BASE_URL = 'users'
  user = new BehaviorSubject<UserModel | null>(null)

  constructor(private http: HttpClient) { }

  async registerNewAccout(newAccount: RegisterModel): Promise<any> {
    return await firstValueFrom(this.http.post(`${environment.apiUrl}/${this.BASE_URL}/register`, newAccount));
  }

  async login(loginData: LoginrequestModel): Promise<any> {
    return await firstValueFrom(this.http.post<LoginresponseModel>(`${environment.apiUrl}/${this.BASE_URL}/login`, loginData))
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
