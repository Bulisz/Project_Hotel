import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NonStandardEquipmentModel } from 'src/app/models/non-standard-equipment-model';
import { AvailableRoomsModel } from 'src/app/models/available-rooms-model';
import { RoomListModel } from 'src/app/models/room-list.model';
import { RoomService } from 'src/app/services/room.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit{
  
  allRooms: Array<RoomListModel> | null = null;
  searchedRooms = new BehaviorSubject<Array<RoomListModel> | null>(null)
  nonStandardEquipments: Array<NonStandardEquipmentModel> =[];
  roomSelector: FormGroup;
  equipmentsControllers: FormArray | undefined;
  
  constructor (private formBuilder: FormBuilder, private roomService: RoomService, private router: Router) {
    this.roomSelector = this.formBuilder.group({
      guestNumber: (null),
      dogNumber:  (null),
      nonStandardEquipments: new FormArray([]),
      bookingFrom: (null),
      bookingTo: (null)
    })
  }

  ngOnInit(): void {

    this.searchedRooms.subscribe({
      next: (res) => this.allRooms = res
    })
    this.roomService.fetchNonStandardEquipmentData().subscribe({
      next: (response) => {
        this.nonStandardEquipments = response;

        this.nonStandardEquipments.forEach(e => {
          this.equipmentFormArray.push( 
            new FormControl(false)
          )
        }
          )
        this.equipmentsControllers = this.roomSelector.controls['nonStandardEquipments'] as FormArray;

      },
      error: (error) => {
        console.log(error);
      }
    })

    this.loadRooms();
  }


  get equipmentFormArray () {
    return this.roomSelector.controls['nonStandardEquipments'] as FormArray
  }

  loadRooms() {
            
    this.roomService.getAllRooms().subscribe( {

      next: (response: RoomListModel[]) =>{ this.searchedRooms.next(response)  
      
    
      },
      error: (error) => console.log(error),
    });
  }

  
  goToDetails(id: number) {
    this.router.navigate(['room-details', id]);
  }

  onSubmit() {
    const selectedList: number[] = [];
    this.equipmentFormArray.controls.forEach((element, i) => {
      if (element.value === true) {
        selectedList.push(this.nonStandardEquipments[i].id)
      }

    });
    const formValue = this.roomSelector.getRawValue()
    const parsoltFormValue = {...formValue, nonStandardEquipments: selectedList};
    
    this.roomService.getRoomOptions(parsoltFormValue).subscribe({
      next: (response: RoomListModel[]) =>{ this.searchedRooms.next(response)
        console.log(this.searchedRooms.value)
      
        },
        error: (error) => console.log(error),
      });

  }


  
}
