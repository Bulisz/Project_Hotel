import { Injectable } from '@angular/core';
import { AccountService } from './account.service';

import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserGuardService  {

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
