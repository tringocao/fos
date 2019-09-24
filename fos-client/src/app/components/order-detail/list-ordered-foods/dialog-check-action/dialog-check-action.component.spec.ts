import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCheckActionComponent } from './dialog-check-action.component';

describe('DialogCheckActionComponent', () => {
  let component: DialogCheckActionComponent;
  let fixture: ComponentFixture<DialogCheckActionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCheckActionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCheckActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
