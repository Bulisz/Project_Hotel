import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, firstValueFrom } from 'rxjs';
import { RegisterModel } from '../models/register-model';
import { LoginrequestModel } from '../models/loginrequest-model';
import { TokensModel } from '../models/tokens-model';
import { UserModel } from '../models/user-model';
import { environment } from '../../environments/environment';
import { UpdateUserModel } from '../models/update-user-model';
import { UserListModel } from '../models/user-list-model';
import { EmailVerificationModel } from '../models/email-verification-model';
import { ResetPasswordModel } from '../models/reset-password-model';
import { GoogleLoginModel } from '../models/google-login-model';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  adminPageSelector = new BehaviorSubject<string | null>(null);
  BASE_URL = 'users'
  user = new BehaviorSubject<UserModel | null>(null)

  constructor(private http: HttpClient) { }

  async registerNewAccout(newAccount: RegisterModel): Promise<any> {
    return await firstValueFrom(this.http.post(`${environment.apiUrl}/${this.BASE_URL}/register`, newAccount));
  }

  async login(loginData: LoginrequestModel): Promise<TokensModel> {
    return await firstValueFrom(this.http.post<TokensModel>(`${environment.apiUrl}/${this.BASE_URL}/login`, loginData))
    .then(async lrm => {
      if(lrm.accessToken){
        localStorage.setItem('accessToken', lrm.accessToken.value);
        localStorage.setItem('refreshToken', lrm.refreshToken.value);
        await this.getCurrentUser()
      }
      return lrm
    })
  }

  async getCurrentUser(): Promise<UserModel> {
    return await firstValueFrom(this.http.get<UserModel>(`${environment.apiUrl}/${this.BASE_URL}/getcurrentuser`))
    .then(um => {
      this.user.next(um)
      return um
    })
  }

  async refresh(): Promise<TokensModel>{
    let refreshToken= localStorage.getItem('refreshToken') ? localStorage.getItem('refreshToken') : '';
    return await firstValueFrom(this.http.post<TokensModel>(`${environment.apiUrl}/${this.BASE_URL}/Refresh`, {refreshToken}))
      .then(async td =>{
        if(td.accessToken && td.refreshToken){
          localStorage.setItem('accessToken', td.accessToken.value);
          localStorage.setItem('refreshToken', td.refreshToken.value);
          await this.getCurrentUser()
        }
        return td
      })
  }

  async logout(){
    let refreshToken= localStorage.getItem('refreshToken') ? localStorage.getItem('refreshToken') : '';
    await firstValueFrom(this.http.post(`${environment.apiUrl}/${this.BASE_URL}/Logout`, {refreshToken}))
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('adminPageSelector');
    this.user.next(null)
  }

  async deleteProfile(userId: string): Promise<any> {
    return await firstValueFrom(this.http.delete(`${environment.apiUrl}/${this.BASE_URL}/${userId}`))
    .then(() => this.logout())
  }

  async updateUser(user: UpdateUserModel): Promise<UserModel> {
    return await firstValueFrom(this.http.put<UserModel>(`${environment.apiUrl}/${this.BASE_URL}/updateuser`,user))
  }

  async getUsers(): Promise<Array<UserListModel>>{
    return await firstValueFrom(this.http.get<Array<UserListModel>>(`${environment.apiUrl}/${this.BASE_URL}/GetUsers`))
  }

  async getUserById(userId: string): Promise<UserListModel> {
    return await firstValueFrom(this.http.get<UserListModel>(`${environment.apiUrl}/${this.BASE_URL}/GetUserById/${userId}`))
  }

  async updateUserAsAdmin(updateUser: UserListModel): Promise<UserListModel> {
    return await firstValueFrom(this.http.put<UserListModel>(`${environment.apiUrl}/${this.BASE_URL}/UpdateUserAsAdmin`, updateUser))
  }

  async confirmEmail(emailVerification: EmailVerificationModel): Promise<boolean> {
    return await firstValueFrom(this.http.post<boolean>(`${environment.apiUrl}/${this.BASE_URL}/VerifyEmail`, emailVerification))
  }

  async resetPassword(resetPassword: ResetPasswordModel): Promise<boolean> {
    return await firstValueFrom(this.http.post<boolean>(`${environment.apiUrl}/${this.BASE_URL}/ResetPassword`, resetPassword))
  }

  async forgotPassword(emailAddress: string): Promise<any> {
    return await firstValueFrom(this.http.post<any>(`${environment.apiUrl}/${this.BASE_URL}/ForgotPassword`, emailAddress))
  }

  async LoginWithGoogle(credentials: GoogleLoginModel): Promise<TokensModel> {
    return await firstValueFrom(this.http.post<TokensModel>(`${environment.apiUrl}/${this.BASE_URL}/LoginWithGoogle`, credentials))
    .then(async lrm => {
      if(lrm.accessToken){
        localStorage.setItem('accessToken', lrm.accessToken.value);
        localStorage.setItem('refreshToken', lrm.refreshToken.value);
        await this.getCurrentUser()
      }
      return lrm
    })
  }

}
