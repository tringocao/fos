import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OpenEventDialogComponent } from './open-event-dialog.component';

describe('OpenEventDialogComponent', () => {
  let component: OpenEventDialogComponent;
  let fixture: ComponentFixture<OpenEventDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OpenEventDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OpenEventDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
