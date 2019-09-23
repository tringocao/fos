import { TestBed } from '@angular/core/testing';

import { EventFormMailService } from './event-form-mail.service';

describe('EventFormMailService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EventFormMailService = TestBed.get(EventFormMailService);
    expect(service).toBeTruthy();
  });
});
