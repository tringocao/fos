import { TestBed } from '@angular/core/testing';

import { CustomGroupService } from './custom-group.service';

describe('CustomGroupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomGroupService = TestBed.get(CustomGroupService);
    expect(service).toBeTruthy();
  });
});
