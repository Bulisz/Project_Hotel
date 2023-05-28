import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationComponent } from '../components/confirmation/confirmation.component';
import {  firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  async confirmationDialog(message: string): Promise<string> {
    let result = 'cancel'
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: { top: '10%' },
      data: { message }
    };
    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings);
    await firstValueFrom(dialogRef.afterClosed())
      .then(res => result = res)
    return result
  }
}
