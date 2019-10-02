import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomGroupPageComponent } from './custom-group-page.component';

describe('CustomGroupPageComponent', () => {
  let component: CustomGroupPageComponent;
  let fixture: ComponentFixture<CustomGroupPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomGroupPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomGroupPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
