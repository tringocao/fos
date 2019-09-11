import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { OrderService } from "src/app/services/order/order.service";
import { User } from "src/app/models/user";
import { Order } from "src/app/models/order";
import { UserService } from "src/app/services/user/user.service";
import { Food } from "src/app/models/food";
import { ListOrderedFoodsComponent } from "./list-ordered-foods/list-ordered-foods.component";
interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
}
interface FoodCheck {
  food: Food;
  checked: boolean;
}
@Component({
  selector: "app-order-detail",
  templateUrl: "./order-detail.component.html",
  styleUrls: ["./order-detail.component.less"]
})
export class OrderDetailComponent implements OnInit {
  idOrder: string;
  data: RestaurantMore;
  user: User;
  order: Order;
  resDetail: RestaurantDetail;
  isDataAvailable: boolean = false;
  totalBudget: Number;
  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private restaurantService: RestaurantService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.data = { restaurant: null, detail: null };
    this.idOrder = this.route.snapshot.paramMap.get("id");
    this.orderService.GetOrder(this.idOrder).then(order => {
      this.order = order;
      this.restaurantService
        .getRestaurants([order.IdRestaurant])
        .then(restaurant => {
          this.data.restaurant = restaurant[0];
          this.restaurantService
            .getRestaurantDetail(order.IdDelivery)
            .then(restaurantd => {
              this.data.detail = restaurantd;
              this.userService.getUserById(order.IdUser).then(user => {
                this.user = user;
                this.totalBudget = 350000;
                this.isDataAvailable = true;
              });
            });
        });
    });
  }
  @ViewChild(ListOrderedFoodsComponent, { static: false })
  foodorderlist: ListOrderedFoodsComponent;
  GetFoodFromMenu(food: FoodCheck): void {
    if (food.checked) {
      this.foodorderlist.AddFoodDetail(
        this.foodorderlist.MapFoodCheck2FoodDetail(food.food)
      );
    } else {
      this.foodorderlist.DeleteFoodDetail(food.food);
    }
  }
  Save(){
    this.order.FoodDetail = this.foodorderlist.getAllFoodDetail();
    this.orderService.SetOrder(this.order);
  }
}
