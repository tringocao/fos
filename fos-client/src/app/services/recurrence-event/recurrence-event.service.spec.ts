import { TestBed } from '@angular/core/testing';

import { RecurrenceEventService } from './recurrence-event.service';

describe('RecurrenceEventService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RecurrenceEventService = TestBed.get(RecurrenceEventService);
    expect(service).toBeTruthy();
  });
});
