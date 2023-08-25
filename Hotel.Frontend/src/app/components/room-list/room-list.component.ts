import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EquipmentModel } from 'src/app/models/equipment-model';
import { RoomListModel } from 'src/app/models/room-list.model';
import { RoomService } from 'src/app/services/room.service';
import { BehaviorSubject } from 'rxjs';
import { validationHandler } from 'src/utils/validationHandler';
import { EquipmentService } from 'src/app/services/equipment.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit{

  allRooms: Array<RoomListModel> | null = null;
  searchedRooms = new BehaviorSubject<Array<RoomListModel> | null>(null);
  nonStandardEquipments: Array<EquipmentModel> =[];
  roomSelector: FormGroup;
  equipmentsControllers: FormArray | undefined;

  constructor (private formBuilder: FormBuilder,
              private roomService: RoomService,
              private router: Router,
              private equipmentService: EquipmentService,
              private cookieService: CookieService) {
    this.roomSelector = this.formBuilder.group({
      numberOfBeds: ['', [Validators.required,  Validators.min(1), Validators.max(20)]],
      maxNumberOfDogs:  ['', [Validators.required,  Validators.min(1), Validators.max(10)]],
      nonStandardEquipments: new FormArray([]),
      bookingFrom: [null, Validators.required],
      bookingTo: [null, Validators.required]
    })
  }

  async ngOnInit() {
    this.searchedRooms.subscribe({
      next: (res) => this.allRooms = res
    })

    await this.equipmentService.getNonStandardEquipments()
      .then(res => {
        this.nonStandardEquipments = res;
        let nonStandardEquipmentsCookieContent: number[] = []
        if(this.cookieService.check('nonStandardEquipments')){
            nonStandardEquipmentsCookieContent = this.cookieService.get('nonStandardEquipments').split(',').map(x=>+x)
        }
        this.nonStandardEquipments.forEach(e => {
          if(nonStandardEquipmentsCookieContent.includes(e.id)){
            this.equipmentFormArray.push(
              new FormControl(true)
            )
          } else {
            this.equipmentFormArray.push(
              new FormControl(false)
            )
          }
        })
        this.equipmentsControllers = this.roomSelector.controls['nonStandardEquipments'] as FormArray;
      })

    if(this.cookieService.check('numberOfBeds')){
      this.roomSelector.get('numberOfBeds')?.setValue(this.cookieService.get('numberOfBeds'))
      this.roomSelector.get('maxNumberOfDogs')?.setValue(this.cookieService.get('maxNumberOfDogs'))
      this.roomSelector.get('bookingFrom')?.setValue(this.cookieService.get('bookingFrom'))
      this.roomSelector.get('bookingTo')?.setValue(this.cookieService.get('bookingTo'))
      this.onSubmit()
    } else {
      await this.loadRooms();
    }
  }

  get equipmentFormArray () {
    return this.roomSelector.controls['nonStandardEquipments'] as FormArray
  }

  async loadRooms() {
    await this.roomService.getAllRooms()
      .then(res => this.searchedRooms.next(res))
      .catch(err => console.log(err))
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

    this.cookieService.set('numberOfBeds', this.numberOfBeds?.value)
    this.cookieService.set('maxNumberOfDogs', this.maxNumberOfDogs?.value)
    this.cookieService.set('bookingFrom', this.bookingFrom?.value)
    this.cookieService.set('bookingTo', this.bookingTo?.value)
    this.cookieService.set('nonStandardEquipments', selectedList.join(','))

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
    if(this.roomSelector.get('bookingFrom')?.value  && this.roomSelector.get('bookingTo')?.value){
      this.roomSelector.get('bookingFrom')?.setErrors(null)
      this.roomSelector.get('bookingTo')?.setErrors(null)
    }
  }
}
