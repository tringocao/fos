import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMakeOrderComponent } from './edit-make-order.component';

describe('EditMakeOrderComponent', () => {
  let component: EditMakeOrderComponent;
  let fixture: ComponentFixture<EditMakeOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMakeOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMakeOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
