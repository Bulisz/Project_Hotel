import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserGuardService implements CanActivate {

  constructor(public as: AccountService) {}

  canActivate(): boolean {
    if (!(this.as.user.value)) {
      return false;
    }
    return true;
  }
}
