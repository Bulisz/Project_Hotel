import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class OperatorGuardService implements CanActivate {

  constructor(public as: AccountService) {}

  canActivate(): boolean {
    if (!(this.as.user.value?.role === 'Operator')) {
      return false;
    }
    return true;
  }
}
