import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserListModel } from 'src/app/models/user-list-model';
import { AccountService } from 'src/app/services/account.service';
import { DialogService } from 'src/app/services/dialog.service';
import { UpdateUserAsAdmin } from '../update-user-as-admin/update-user-as-admin.component';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrls: ['./user-manager.component.css']
})
export class UserManagerComponent implements OnInit {
  
  @Input() userList!: Array<UserListModel>;
  @Output() userModified = new EventEmitter<string>;
  

  constructor(private dialogService: DialogService, private accountService: AccountService, private dialog: MatDialog){}
  
  ngOnInit(): void {}

  async deleteProfile(userId: string) {
    let result = await this.dialogService.confirmationDialog("Biztosan törlöd a profilt?")
    if(result === 'agree'){
      this.accountService.deleteProfile(userId)
        .then(() => this.userModified.emit('userDeleted'))
    }
  }

  async updateUser(id: string){
    let user = await this.accountService.getUserById(id);

    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '3%'},
      data: {
        id: user?.id,
        lastName: user?.lastName,
        firstName: user?.firstName,
        username: user?.userName,
        email: user?.email,
        emailConfirmed: user?.emailConfirmed,
        role: user.role}
    };

    let dialog = this.dialog.open(UpdateUserAsAdmin,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: res => {
        if(res === 'changed'){
          this.userModified.emit('userModified')
        }
      }
    })
  }

}
