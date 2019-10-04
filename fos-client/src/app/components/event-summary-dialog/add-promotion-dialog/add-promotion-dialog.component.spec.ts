import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPromotionDialogComponent } from './add-promotion-dialog.component';

describe('AddPromotionDialogComponent', () => {
  let component: AddPromotionDialogComponent;
  let fixture: ComponentFixture<AddPromotionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddPromotionDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddPromotionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
