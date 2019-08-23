import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RestaurantsPageComponent } from './restaurants-page.component';

describe('RestaurantsPageComponent', () => {
  let component: RestaurantsPageComponent;
  let fixture: ComponentFixture<RestaurantsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RestaurantsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RestaurantsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
