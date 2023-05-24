import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NonStandardEquipmentModel } from 'src/app/models/non-standard-equipment-model';
import { RoomListModel } from 'src/app/models/room-list.model';
import { RoomService } from 'src/app/services/room.service';
import { BehaviorSubject } from 'rxjs';
import { validationHandler } from 'src/utils/validationHandler';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit{

  allRooms: Array<RoomListModel> | null = null;
  searchedRooms = new BehaviorSubject<Array<RoomListModel> | null>(null);
  nonStandardEquipments: Array<NonStandardEquipmentModel> =[];
  roomSelector: FormGroup;
  equipmentsControllers: FormArray | undefined;

  constructor (private formBuilder: FormBuilder, private roomService: RoomService, private router: Router) {
    this.roomSelector = this.formBuilder.group({
      numberOfBeds: ['', [Validators.required,  Validators.min(1), Validators.max(20)]],
      maxNumberOfDogs:  ['', [Validators.required,  Validators.min(1), Validators.max(10)]],
      nonStandardEquipments: new FormArray([]),
      bookingFrom: [null, Validators.required],
      bookingTo: [null, Validators.required]
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

      next: (response: RoomListModel[]) =>{ this.searchedRooms.next(response)},
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
        },
        error: (error) => validationHandler(error, this.roomSelector),
      });

  }

  get numberOfBeds(): AbstractControl | null {
    return this.roomSelector.get('numberOfBeds')
  }

  get maxNumberOfDogs(): AbstractControl | null {
    return this.roomSelector.get('maxNumberOfDogs')
  }

  get bookingFrom(): AbstractControl | null {
    return this.roomSelector.get('bookingFrom')
  }

  get bookingTo(): AbstractControl | null {
    return this.roomSelector.get('bookingTo')
  }

  changeMe(){
    this.roomSelector.get('bookingFrom')?.setErrors(null)
    this.roomSelector.get('bookingTo')?.setErrors(null)
  }
}
