import { Component, OnInit, Input } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate
} from '@angular/animations';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { TimeDialogComponent } from '../time-dialog/time-dialog/time-dialog.component';
import { ITime } from '../time-dialog/w-clock/w-clock.component';
import moment from 'moment';

@Component({
  selector: 'app-datetimepicker',
  templateUrl: './datetimepicker.component.html',
  styleUrls: ['./datetimepicker.component.less'],
  animations: [
    trigger('changeState', [
      state(
        'pickTime',
        style({
          transform: 'scaleY(1)'
        })
      ),
      transition('* => pickTime', animate('100ms'))
    ])
  ]
})
export class DatetimepickerComponent implements OnInit {
  @Input() formGroup: FormGroup;
  @Input() formLable: string;
  @Input() dateFormControlName: string;
  @Input() timeFormControlName: string;
  constructor(private dialog:MatDialog) { }

  date: string;
  time: string;
  itime: ITime;

  triggered:boolean
  ngOnInit() {
    this.triggered = false;
  }

  public HasError = (controlName: string, errorName: string) => {
    // debugger;
    return this.formGroup.controls[controlName].hasError(errorName);
  };

  openDialog(): void {
    const dialogRef = this.dialog.open(TimeDialogComponent, {
      width: '400px',
      data: {
        time: this.itime
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      this.time = result ? result : '00:00';
      if (result) {
        this.itime = result;
        this.formGroup.get(this.timeFormControlName).setValue(result.hour + ':' + result.minute);
      }
    });
  }

  onTrigger(state:boolean) {
    this.triggered = state;
  }

  onTimeInputBlur()  {
    
  }

}
