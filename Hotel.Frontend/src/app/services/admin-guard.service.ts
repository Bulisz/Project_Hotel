import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AccountService } from './account.service';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService implements CanActivate {

  currentUser: UserModel | null = null;

  constructor(public as: AccountService) {}

  async canActivate(): Promise<boolean> {

    await this.as.getCurrentUser()
      .then(res => this.currentUser = res)

    if (!(this.currentUser?.role === 'Admin')) {
      return false;
    }
    return true;
  }
}
