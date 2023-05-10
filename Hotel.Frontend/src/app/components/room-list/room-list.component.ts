import { Component, OnInit } from '@angular/core';
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
  
  constructor (private roomService: RoomService, private router: Router) {}

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

}
