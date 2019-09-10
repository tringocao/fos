import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventSummaryDialogComponent } from './event-summary-dialog.component';

describe('EventSummaryDialogComponent', () => {
  let component: EventSummaryDialogComponent;
  let fixture: ComponentFixture<EventSummaryDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventSummaryDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
