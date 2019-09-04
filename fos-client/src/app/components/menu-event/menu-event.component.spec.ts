import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuEventComponent } from './menu-event.component';

describe('MenuEventComponent', () => {
  let component: MenuEventComponent;
  let fixture: ComponentFixture<MenuEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
