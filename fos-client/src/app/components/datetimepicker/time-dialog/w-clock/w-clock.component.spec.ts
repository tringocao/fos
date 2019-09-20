import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WClockComponent } from './w-clock.component';

describe('WClockComponent', () => {
  let component: WClockComponent;
  let fixture: ComponentFixture<WClockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WClockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WClockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
