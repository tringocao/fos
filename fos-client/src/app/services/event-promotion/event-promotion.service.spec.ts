import { TestBed } from '@angular/core/testing';

import { EventPromotionService } from './event-promotion.service';

describe('EventPromotionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EventPromotionService = TestBed.get(EventPromotionService);
    expect(service).toBeTruthy();
  });
});
