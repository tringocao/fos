import { Component, OnInit, Inject } from "@angular/core";
import { FormControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { User } from "src/app/models/user";
@Component({
  selector: "app-setting-dialog",
  templateUrl: "./setting-dialog.component.html",
  styleUrls: ["./setting-dialog.component.less"]
})
export class SettingDialogComponent implements OnInit {
  date = new FormControl(new Date());
  serializedDate = new FormControl(new Date().toISOString());
  constructor(
    public dialogRef: MatDialogRef<SettingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {}
}
