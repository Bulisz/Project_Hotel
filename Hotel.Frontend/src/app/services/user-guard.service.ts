import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { CanActivate } from '@angular/router';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserGuardService implements CanActivate {

  currentUser: UserModel | null = null;

  constructor(public as: AccountService) {}

  async canActivate(): Promise<boolean> {

    await this.as.getCurrentUser()
      .then(res => this.currentUser = res)

    if (!(this.currentUser)) {
      return false;
    }
    return true;
  }
}
