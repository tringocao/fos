import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowActionComponent } from './show-action.component';

describe('ShowActionComponent', () => {
  let component: ShowActionComponent;
  let fixture: ComponentFixture<ShowActionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowActionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
