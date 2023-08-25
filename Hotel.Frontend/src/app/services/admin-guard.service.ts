import { Injectable } from '@angular/core';

import { AccountService } from './account.service';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService  {

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
