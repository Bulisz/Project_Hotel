import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EventDetailsModel } from 'src/app/models/event-details-model';
import { UserModel } from 'src/app/models/user-model';
import { EventService } from 'src/app/services/event.service';
import { CreateEventComponent } from '../create-event/create-event.component';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';

import { BehaviorSubject, async } from 'rxjs';
import { ConfirmationComponent } from '../confirmation/confirmation.component';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  currentUser: UserModel | null = null;
  allEvents: Array<EventDetailsModel> | null = null;


  constructor (private as: AccountService, 
              private eventService: EventService, 
              private dialog: MatDialog, 
              private router: Router) {}

  async ngOnInit() {

    this.as.user.subscribe({
      next: res => this.currentUser = res
    })

    await this.loadEvents();
  }

  loadEvents() {

    this.eventService.getAllEvents().subscribe( {

      next: (data: EventDetailsModel[]) => this.allEvents = data,
      error: (error) => console.log(error),
    });
  }

  addNewPost(){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: { userName: this.currentUser?.userName, role: this.currentUser?.role }
    };
    let dialog = this.dialog.open(CreateEventComponent,dialogBoxSettings);

    dialog.afterClosed().subscribe({
      next: () => this.loadEvents()
    })
  }

  async deleteEvent(id: number){
    let dialogBoxSettings = {
      width: '400px',
      margin: '0 auto',
      disableClose: true,
      hasBackdrop: true,
      position: {top: '10%'},
      data: {message: "Biztos törlöd ezt a programot?"}
    };
    
    let dialogRef = this.dialog.open(ConfirmationComponent, dialogBoxSettings)
    dialogRef.afterClosed().subscribe({
      next: (res) => {
        if (res === "agree"){
          this.eventService.deleteEvent(id)
         .then(() => this.loadEvents())
         .catch((err) => console.log(err))
        }
      }
    })


    
  }
}
