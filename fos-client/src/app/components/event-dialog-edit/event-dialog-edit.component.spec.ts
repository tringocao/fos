import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDialogEditComponent } from './event-dialog-edit.component';

describe('EventDialogEditComponent', () => {
  let component: EventDialogEditComponent;
  let fixture: ComponentFixture<EventDialogEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDialogEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDialogEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
