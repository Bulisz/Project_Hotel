import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { RoomListModel } from 'src/app/models/room-list.model';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit{
  
  allRooms: Array<RoomListModel> = [];

  roomSelector: FormGroup;
  
  constructor (private formBuilder: FormBuilder, private roomService: RoomService, private router: Router) {
    this.roomSelector = this.formBuilder.group({
      guestNumber: (null),
      dogNumber: (null),
      arrival: (null),
      leave: (null)
    })
  }

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms() {
            
    this.roomService.getAllRooms().subscribe( {

      next: (response: RoomListModel[]) =>{ this.allRooms = response
      console.log(this.allRooms)
    
      },
      error: (error) => console.log(error),
    });
  }

  
  goToDetails(id: number) {
    this.router.navigate(['room-details', id]);
  }

  onSubmit() {}
}
