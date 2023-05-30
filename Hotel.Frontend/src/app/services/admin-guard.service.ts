import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService implements CanActivate {

  constructor(public as: AccountService) {}

  canActivate(): boolean {
    if (!(this.as.user.value?.role === 'Admin')) {
      return false;
    }
    return true;
  }
}
