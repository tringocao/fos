import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { TimeDialogComponent } from '../time-dialog/time-dialog/time-dialog.component';
import { ITime } from '../time-dialog/w-clock/w-clock.component';
import moment from 'moment';
import { Utils } from '../time-dialog/utils';
import { OverlayContainer } from '@angular/cdk/overlay';
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscribable, Subscription } from 'rxjs';

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
  @Input() userTime:any;
  @Input() formGroup: FormGroup;
  @Input() formLable: string;
  @Input() dateFormControlName: string;
  @Input() timeFormControlName: string;
  @Output() onDateTimeChange: EventEmitter<any> = new EventEmitter();
  constructor(private dialog:MatDialog, private overlayContainer: OverlayContainer ,
       private dataRouting: DataRoutingService
    ) {
         this.getNavTitleSubscription = this.dataRouting.getNavTitle()
      .subscribe((appTheme: string) => this.appTheme = appTheme);
      overlayContainer.getContainerElement().classList.add("app-"+this.appTheme+"-theme");
    }
    ngOnDestroy() {
      // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
      this.getNavTitleSubscription.unsubscribe();
    }
    private getNavTitleSubscription: Subscription;
    appTheme: string;

  date: string;
  time: string;
  itime: ITime;

  triggered:boolean
  ngOnInit() {
    this.triggered = false;
  }

  public HasError = (controlName: string, errorName: string) => {
    return this.formGroup.controls[controlName].hasError(errorName);
  }

  openDialog(): void {
    //console.log(this.itime)
    //console.log(this.userTime)
    if (!this.itime) {
      if (this.userTime) {
        this.itime = {
          hour: moment(this.userTime).format('HH'),
          minute: moment(this.userTime).format('mm'),
          meriden: "AM",
          format: 24,
        }
      }
    }
    const dialogRef = this.dialog.open(TimeDialogComponent, {
      width: '400px',
      data: {
        time: this.itime
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.time = result ? result : '00:00';
      if (result) {
        this.itime = result;
        this.formGroup.get(this.timeFormControlName).setValue(this.formatHour(24, result.hour) + ':' + this.formatMinute(result.minute));
        this.onDateTimeChange.emit();
      }
    });
  }

  public formatHour(format, hour): string {
    return Utils.formatHour(format, hour);
  }

  public formatMinute(minute): string {
    return Utils.formatMinute(minute);
  }

  onTrigger(state:boolean) {
    this.triggered = state;
  }

  onInputChange($event) {
    this.onDateTimeChange.emit();
  }

  onTimeInputBlur()  {
    
  }

}
