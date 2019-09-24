import { Component, OnInit, Inject } from "@angular/core";
import { FormControl, FormGroup, FormBuilder } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { User } from "src/app/models/user";
import { RecurrenceEventService } from "src/app/services/recurrence-event/recurrence-event.service";
import { RepeateType } from "src/app/models/repeate-type";
import { RecurrenceEvent } from "src/app/models/recurrence-event";
import { OverlayContainer } from "@angular/cdk/overlay";

@Component({
  selector: "app-setting-dialog",
  templateUrl: "./setting-dialog.component.html",
  styleUrls: ["./setting-dialog.component.less"]
})
export class SettingDialogComponent implements OnInit {
  keys = Object.keys;
  symbols = RepeateType;
  recForm: FormGroup;
  recurrenceEvent: RecurrenceEvent;
  startDate: Date;
  endDate: Date;
  startTime: string;
  endTime: string;
  nameRepeatType: string[] = [];
  constructor(
    public dialogRef: MatDialogRef<SettingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private recurrenceEventService: RecurrenceEventService,
    private fb: FormBuilder,
    overlayContainer: OverlayContainer
  ) {
    overlayContainer.getContainerElement().classList.add("app-theme1-theme");
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    this.recForm = this.fb.group({
      titleInput: null,
      startInput: null,
      startTimeInput: null,
      endInput: null,
      endTimeInput: null,
      selectRepeatType: null
    });
    this.recurrenceEventService.getByUserId(this.data.Id).then(e => {
      this.recurrenceEvent = e;
      this.recForm.get("endInput").setValue(e.EndDate);
      this.recForm.get("startInput").setValue(e.StartDate);

      this.startDate = new Date(e.StartDate);
      this.endDate = new Date(e.EndDate);
      var hour = this.endDate.getHours() < 10 ? "0" : "";
      var minus = this.endDate.getMinutes() < 10 ? "0" : "";
      this.recForm
        .get("endTimeInput")
        .setValue(
          hour +
            this.endDate.getHours() +
            ":" +
            minus +
            this.endDate.getMinutes()
        );
      var hour = this.startDate.getHours() < 10 ? "0" : "";
      var minus = this.startDate.getMinutes() < 10 ? "0" : "";
      this.recForm
        .get("startTimeInput")
        .setValue(
          hour +
            this.startDate.getHours() +
            ":" +
            minus +
            this.startDate.getMinutes()
        );
      this.recForm.get("titleInput").setValue(e.Title);
      this.recForm
        .get("selectRepeatType")
        .setValue(RepeateType[e.TypeRepeat].toString());
      for (var enumMember in RepeateType) {
        var isValueProperty = parseInt(enumMember, 10) >= 0;
        if (isValueProperty) {
          this.nameRepeatType.push(RepeateType[enumMember].toString());
        }
      }
    });
  }
  saveAll() {
    if (this.recurrenceEvent.Id == 0) {
      this.recurrenceEvent.Id = null;
      this.recurrenceEvent.UserMail = this.data.Mail;
      this.recurrenceEventService.addRecurrenceEvent(this.recurrenceEvent);
    } else {
      this.recurrenceEvent.UserMail = this.data.Mail;
      this.recurrenceEventService.updateRecurrenceEvent(this.recurrenceEvent);
    }
    this.onNoClick();
  }
  getAllValue() {
    this.startTime = this.recForm.get("startTimeInput").value;
    this.startDate = new Date(this.recForm.get("startInput").value);
    var temp = this.startTime.split(":", 2);
    this.startDate.setHours(Number(temp[0]));
    this.startDate.setMinutes(Number(temp[1]));
    this.recurrenceEvent.StartDate = this.startDate;

    this.endTime = this.recForm.get("endTimeInput").value;
    this.endDate = new Date(this.recForm.get("endInput").value);
    var temp = this.endTime.split(":", 2);
    this.endDate.setHours(Number(temp[0]));
    this.endDate.setMinutes(Number(temp[1]));
    this.recurrenceEvent.EndDate = this.endDate;

    this.recurrenceEvent.Title = this.recForm.get("titleInput").value;
    this.recurrenceEvent.TypeRepeat = this.stringToEnum<
      typeof RepeateType,
      RepeateType
    >(RepeateType, this.recForm.get("selectRepeatType").value);
    this.saveAll();
  }
  stringToEnum<ET, T>(enumObj: ET, str: keyof ET): T {
    return enumObj[<string>str];
  }
  removeRecurrenceEvent() {
    if (this.recurrenceEvent.Id != 0) {
      this.recurrenceEventService.removeRecurrenceEvent(
        this.recurrenceEvent.Id
      );
    }
    this.dialogRef.close();
  }
}
