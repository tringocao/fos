import { Component, OnInit, Inject } from "@angular/core";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { OverlayContainer } from "@angular/cdk/overlay";
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';
@Component({
  selector: "app-dialog-check-action",
  templateUrl: "./dialog-check-action.component.html",
  styleUrls: ["./dialog-check-action.component.less"]
})
export class DialogCheckActionComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<DialogCheckActionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FoodDetailJson,
    overlayContainer: OverlayContainer
    ,
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
  ngOnInit() {}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
