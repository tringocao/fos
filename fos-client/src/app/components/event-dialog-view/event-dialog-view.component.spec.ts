import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDialogViewComponent } from './event-dialog-view.component';

describe('EventDialogViewComponent', () => {
  let component: EventDialogViewComponent;
  let fixture: ComponentFixture<EventDialogViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDialogViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDialogViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
