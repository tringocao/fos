import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryDishesDialogComponent } from './summary-dishes-dialog.component';

describe('SummaryDishesDialogComponent', () => {
  let component: SummaryDishesDialogComponent;
  let fixture: ComponentFixture<SummaryDishesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SummaryDishesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryDishesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
