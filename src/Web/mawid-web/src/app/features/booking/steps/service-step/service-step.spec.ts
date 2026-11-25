import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServiceStep } from './service-step';

describe('ServiceStep', () => {
  let component: ServiceStep;
  let fixture: ComponentFixture<ServiceStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceStep],
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
