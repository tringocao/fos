import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PromotionsChipComponent } from './promotions-chip.component';

describe('PromotionsChipComponent', () => {
  let component: PromotionsChipComponent;
  let fixture: ComponentFixture<PromotionsChipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PromotionsChipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PromotionsChipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
