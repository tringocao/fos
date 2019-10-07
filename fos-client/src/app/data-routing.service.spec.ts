import { TestBed } from '@angular/core/testing';

import { DataRoutingService } from './data-routing.service';

describe('DataRoutingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataRoutingService = TestBed.get(DataRoutingService);
    expect(service).toBeTruthy();
  });
});
