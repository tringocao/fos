import { TestBed } from '@angular/core/testing';

import { EventFormValidationService } from './event-form-validation.service';

describe('EventFormValidationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EventFormValidationService = TestBed.get(EventFormValidationService);
    expect(service).toBeTruthy();
  });
});
