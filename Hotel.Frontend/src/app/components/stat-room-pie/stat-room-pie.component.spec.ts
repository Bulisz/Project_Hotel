import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatRoomPieComponent } from './stat-room-pie.component';

describe('StatRoomPieComponent', () => {
  let component: StatRoomPieComponent;
  let fixture: ComponentFixture<StatRoomPieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatRoomPieComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatRoomPieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
