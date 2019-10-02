import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateCustomGroupComponent } from './create-update-custom-group.component';

describe('CreateUpdateCustomGroupComponent', () => {
  let component: CreateUpdateCustomGroupComponent;
  let fixture: ComponentFixture<CreateUpdateCustomGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateCustomGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateCustomGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
