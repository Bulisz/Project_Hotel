import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatYearRoomsComponent } from './stat-year-rooms.component';

describe('StatYearRoomsComponent', () => {
  let component: StatYearRoomsComponent;
  let fixture: ComponentFixture<StatYearRoomsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatYearRoomsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatYearRoomsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
