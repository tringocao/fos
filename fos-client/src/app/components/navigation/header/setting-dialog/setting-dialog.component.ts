import { Component, OnInit, Inject } from "@angular/core";
import { FormControl, FormGroup, FormBuilder } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { User } from "src/app/models/user";
import { RecurrenceEventService } from "src/app/services/recurrence-event/recurrence-event.service";
import { RepeateType } from "src/app/models/repeate-type";
import { RecurrenceEvent } from "src/app/models/recurrence-event";
@Component({
  selector: "app-setting-dialog",
  templateUrl: "./setting-dialog.component.html",
  styleUrls: ["./setting-dialog.component.less"]
})
export class SettingDialogComponent implements OnInit {
  repeatType: RepeateType;
  recForm: FormGroup;
  recurrenceEvent: RecurrenceEvent;
  constructor(
    public dialogRef: MatDialogRef<SettingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private recurrenceEventService: RecurrenceEventService,
    private fb: FormBuilder
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    this.recForm = this.fb.group({
      titleInput: null,
      startInput: null,
      startTimeInput: null,
      endInput: null,
      endTimeInput: null
    });
    this.recurrenceEventService.getByUserId(this.data.Id).then(e => {
      this.recurrenceEvent = e;
      debugger;
      this.recForm.get("endInput").setValue(e.EndDate);
      this.recForm.get("endTimeInput").setValue(e.EndDate.getTime);
      this.recForm.get("startTimeInput").setValue(e.StartDate.getTime);

      this.recForm.get("startInput").setValue(e.StartDate);
      this.recForm.get("titleInput").setValue(e.Title);
    });
  }
}
