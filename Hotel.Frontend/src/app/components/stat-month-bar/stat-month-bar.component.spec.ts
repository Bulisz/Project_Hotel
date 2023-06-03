import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatMonthBarComponent } from './stat-month-bar.component';

describe('StatMonthBarComponent', () => {
  let component: StatMonthBarComponent;
  let fixture: ComponentFixture<StatMonthBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatMonthBarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatMonthBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
