import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
  selector: "app-no-promotions-notification",
  templateUrl: "./no-promotions-notification.component.html",
  styleUrls: ["./no-promotions-notification.component.less"]
})
export class NoPromotionsNotificationComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<NoPromotionsNotificationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) {}

  ngOnInit() {}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
