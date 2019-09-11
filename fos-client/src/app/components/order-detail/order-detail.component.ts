import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { OrderService } from 'src/app/services/order/order.service';
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
  idOrder: string;
  data: RestaurantMore;
  resDetail: RestaurantDetail;
  isDataAvailable:boolean = false;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private restaurantService: RestaurantService
  ) {}

  ngOnInit() {
    this.idOrder = this.route.snapshot.paramMap.get("id");
    this.orderService.GetOrder(this.idOrder).then(o => {
          this.restaurantService.getRestaurants([o.IdDelivery])
          .then(r => {
            this.data.restaurant = r[0];
            this.restaurantService.getRestaurantDetail(o.IdDelivery).then(rd => {
              this.data.detail = rd;
              this.isDataAvailable = true;
            });
          });
      });

  }
  getOrder(): void {}
}
