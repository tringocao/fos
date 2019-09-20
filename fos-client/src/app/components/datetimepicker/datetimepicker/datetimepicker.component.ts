import { Component, OnInit, Input } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate
} from '@angular/animations';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-datetimepicker',
  templateUrl: './datetimepicker.component.html',
  styleUrls: ['./datetimepicker.component.less'],
  animations: [
    trigger('changeState', [
      state(
        'pickTime',
        style({
          transform: 'scale(1)'
        })
      ),
      transition('* => pickTime', animate('100ms'))
    ])
  ]
})
export class DatetimepickerComponent implements OnInit {
  @Input() formGroup: FormGroup;
  @Input() dateFormControlName: string;
  @Input() timeFormControlName: string;
  constructor() { }

  triggered:boolean
  ngOnInit() {
    this.triggered = false;
  }

  onTrigger() {
    this.triggered = true;
  }

}
