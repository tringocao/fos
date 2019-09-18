import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventSummaryPrintComponent } from './event-summary-print.component';

describe('EventSummaryPrintComponent', () => {
  let component: EventSummaryPrintComponent;
  let fixture: ComponentFixture<EventSummaryPrintComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventSummaryPrintComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventSummaryPrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
