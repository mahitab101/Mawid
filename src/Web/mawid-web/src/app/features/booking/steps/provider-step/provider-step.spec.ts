import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProviderStep } from './provider-step';

describe('ProviderStep', () => {
  let component: ProviderStep;
  let fixture: ComponentFixture<ProviderStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProviderStep],
    }).compileComponents();

    fixture = TestBed.createComponent(ProviderStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
