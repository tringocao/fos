import { Component, Inject, Input } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { DialogComponent } from "../dialog/dialog.component";
import { NoopScrollStrategy, Overlay } from "@angular/cdk/overlay";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { Observable } from "rxjs";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { DeliveryInfos } from "src/app/models/delivery-infos";

@Component({
  selector: "app-menu",
  templateUrl: "./menu.component.html",
  styleUrls: ["./menu.component.less"]
})
export class MenuComponent {
  overlay: Overlay;
  @Input("restaurant") restaurant: DeliveryInfos;
  @Input() idService: number;
  resDetail: RestaurantDetail;

  constructor(
    public dialog: MatDialog,
    overlay: Overlay,
    private restaurantService: RestaurantService
  ) {
    this.overlay = overlay;
    let restaurantItem: RestaurantDetail = {
      Rating: 0,
      TotalReview: 0,
      PromotionLists: []
    };

    this.resDetail = restaurantItem;
  }

  openDialog(): void {
    this.restaurantService
      .getRestaurantDetail(Number(this.restaurant.DeliveryId), this.idService)
      .then(result => {
        this.resDetail.Rating = Number(result.Rating);
        this.resDetail.TotalReview = Number(result.TotalReview);
        const dialogRef = this.dialog.open(DialogComponent, {
          scrollStrategy: this.overlay.scrollStrategies.noop(),
          autoFocus: false,
          maxHeight: "100vh",
          width: "80%",
          data: {
            restaurant: this.restaurant,
            detail: this.resDetail,
            idService: this.idService
          }
        });

        dialogRef.afterClosed().subscribe(result => {
          //console.log("The dialog was closed");
        });
      });
  }
}
