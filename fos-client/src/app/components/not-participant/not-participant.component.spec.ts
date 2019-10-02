import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotParticipantComponent } from './not-participant.component';

describe('NotParticipantComponent', () => {
  let component: NotParticipantComponent;
  let fixture: ComponentFixture<NotParticipantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotParticipantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotParticipantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
