import { TestBed } from '@angular/core/testing';
import { BusinessStore } from './business-store';

describe('BusinessStore', () => {
  let service: BusinessStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BusinessStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
