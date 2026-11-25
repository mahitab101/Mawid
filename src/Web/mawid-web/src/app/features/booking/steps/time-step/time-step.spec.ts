import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TimeStep } from './time-step';

describe('TimeStep', () => {
  let component: TimeStep;
  let fixture: ComponentFixture<TimeStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TimeStep],
    }).compileComponents();

    fixture = TestBed.createComponent(TimeStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
