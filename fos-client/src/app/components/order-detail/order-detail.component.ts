import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
}
@Component({
  selector: "app-order-detail",
  templateUrl: "./order-detail.component.html",
  styleUrls: ["./order-detail.component.less"]
})
export class OrderDetailComponent implements OnInit {
  idOrder: number;
  idEvent: number;
  idRestaurant: number;
  data: RestaurantMore;
  resDetail: RestaurantDetail;

  constructor(
    private route: ActivatedRoute,
    private restaurantService: RestaurantService
  ) {}

  ngOnInit() {
    this.idOrder = Number(this.route.snapshot.paramMap.get("idOrder"));
    this.idEvent = Number(this.route.snapshot.paramMap.get("idEvent"));
    this.idRestaurant = Number(
      this.route.snapshot.paramMap.get("idRestaurant")
    );
    this.restaurantService.getRestaurants([this.idRestaurant]).then(result => {
      if (result.length > 0) {
        this.data.restaurant = result[0];
        this.restaurantService
          .getRestaurantDetail(Number(this.data.restaurant.DeliveryId))
          .then(result => {
            this.data.detail = result;
          });
      }
    });
  }
  getOrder(): void {}
}
