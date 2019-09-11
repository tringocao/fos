import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListOrderedFoodsComponent } from './list-ordered-foods.component';

describe('ListOrderedFoodsComponent', () => {
  let component: ListOrderedFoodsComponent;
  let fixture: ComponentFixture<ListOrderedFoodsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListOrderedFoodsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListOrderedFoodsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
