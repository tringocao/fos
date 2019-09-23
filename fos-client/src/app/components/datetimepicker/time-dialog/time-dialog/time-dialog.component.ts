import { Component, OnInit, Inject, Input } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { ITime } from "../w-clock/w-clock.component";
import { Utils } from "../utils";

@Component({
  selector: "app-time-dialog",
  templateUrl: "./time-dialog.component.html",
  styleUrls: ["./time-dialog.component.less"]
})
export class TimeDialogComponent implements OnInit {
  userTime: ITime;

  constructor(
    private dialogRef: MatDialogRef<TimeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.userTime = this.data.time;
  }
  private exportTime = { hour: 7, minute: 15, format: 24 };

  ngOnInit() {}

  public formatHour(format, hour): string {
    return Utils.formatHour(format, hour);
  }

  public formatMinute(minute): string {
    return Utils.formatMinute(minute);
  }

  submit($event) {
    var time = $event;
    if (time.hour.length !== 2) {
      time.hour = this.formatHour(24, $event.hour);
    }
    if (time.minute.length !== 2) {
      time.minute = this.formatMinute($event.minute);
    }

    this.dialogRef.close(time);
  }
}
