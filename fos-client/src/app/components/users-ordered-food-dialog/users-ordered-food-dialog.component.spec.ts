import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersOrderedFoodDialogComponent } from './users-ordered-food-dialog.component';

describe('UsersOrderedFoodDialogComponent', () => {
  let component: UsersOrderedFoodDialogComponent;
  let fixture: ComponentFixture<UsersOrderedFoodDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsersOrderedFoodDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsersOrderedFoodDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
