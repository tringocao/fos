import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PromotionsChipListComponent } from './promotions-chip-list.component';

describe('PromotionsChipListComponent', () => {
  let component: PromotionsChipListComponent;
  let fixture: ComponentFixture<PromotionsChipListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PromotionsChipListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PromotionsChipListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
