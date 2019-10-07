import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { Observable, Observer, Subscription } from "rxjs";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { EventDialogComponent } from "../event-dialog/event-dialog.component";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { OverlayContainer } from "@angular/cdk/overlay";
import { DataRoutingService } from 'src/app/data-routing.service';

interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
  idService: number;
}
interface MoreInfo {
  restaurant: DeliveryInfos;
  idService: number;
}

@Component({
  selector: "app-dialog",
  templateUrl: "./dialog.component.html",
  styleUrls: ["./dialog.component.less"]
})
export class DialogComponent implements OnInit {
  load = true;

  userId: string;

  constructor(
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RestaurantMore,
    overlayContainer: OverlayContainer,
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

  async ngOnInit(): Promise<void> {
    //console.log("-------------------------------------");
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(EventDialogComponent, {
      // scrollStrategy: this.overlay.scrollStrategies.noop(),
      // autoFocus: false,
      maxHeight: "98vh",
      width: "80%",
      data: { restaurant: this.data.restaurant, idService: this.data.idService }
    });

    dialogRef.afterClosed().subscribe(result => {
      //console.log("The dialog was closed");
    });
  }
}
