import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDialogConfirmComponent } from './event-dialog-confirm.component';

describe('EventDialogConfirmComponent', () => {
  let component: EventDialogConfirmComponent;
  let fixture: ComponentFixture<EventDialogConfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDialogConfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDialogConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
