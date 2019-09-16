import { TestBed } from '@angular/core/testing';

import { ExternalFoodService } from './external-food.service';

describe('ExternalFoodService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ExternalFoodService = TestBed.get(ExternalFoodService);
    expect(service).toBeTruthy();
  });
});
