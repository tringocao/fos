import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustPriceComponent } from './adjust-price.component';

describe('AdjustPriceComponent', () => {
  let component: AdjustPriceComponent;
  let fixture: ComponentFixture<AdjustPriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdjustPriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdjustPriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
