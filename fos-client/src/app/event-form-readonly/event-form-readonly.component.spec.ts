import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventFormReadonlyComponent } from './event-form-readonly.component';

describe('EventFormReadonlyComponent', () => {
  let component: EventFormReadonlyComponent;
  let fixture: ComponentFixture<EventFormReadonlyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventFormReadonlyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventFormReadonlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
