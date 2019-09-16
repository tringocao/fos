import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { Observable, Observer } from "rxjs";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { EventDialogComponent } from "../event-dialog/event-dialog.component";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { DeliveryInfos } from "src/app/models/delivery-infos";

interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
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
    @Inject(MAT_DIALOG_DATA) public data: RestaurantMore
  ) {}

  async ngOnInit(): Promise<void> {
    console.log("-------------------------------------");
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
      data: {restaurant:this.data.restaurant, idService:this.data.idService}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log("The dialog was closed");
    });
  }
}
