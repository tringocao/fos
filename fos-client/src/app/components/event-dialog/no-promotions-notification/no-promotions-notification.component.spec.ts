import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoPromotionsNotificationComponent } from './no-promotions-notification.component';

describe('NoPromotionsNotificationComponent', () => {
  let component: NoPromotionsNotificationComponent;
  let fixture: ComponentFixture<NoPromotionsNotificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoPromotionsNotificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoPromotionsNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
