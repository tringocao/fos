import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDialogUserpickerComponent } from './event-dialog-userpicker.component';

describe('EventDialogUserpickerComponent', () => {
  let component: EventDialogUserpickerComponent;
  let fixture: ComponentFixture<EventDialogUserpickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDialogUserpickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDialogUserpickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
