import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { OrderService } from "src/app/services/order/order.service";
import { User } from "src/app/models/user";
import { Event } from "src/app/models/event";
import { Order } from "src/app/models/order";
import { UserService } from "src/app/services/user/user.service";
import { Food } from "src/app/models/food";
import { ListOrderedFoodsComponent } from "./list-ordered-foods/list-ordered-foods.component";
import { EventFormService } from "src/app/services/event-form/event-form.service";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { MatSnackBar } from "@angular/material";
interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
  idService: number;
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
  isOrder: boolean = true;
  data: RestaurantMore;
  user: User;
  order: Order;
  event: Event;
  loading = true;
  checkedData: FoodDetailJson[];
  resDetail: RestaurantDetail;
  isDataAvailable: boolean = false;
  totalBudget: Number;
  idService: number;
  isWildParticipant: boolean;
  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private restaurantService: RestaurantService,
    private userService: UserService,
    private eventFormService: EventFormService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.data = { restaurant: null, detail: null, idService: 1 };
    this.idOrder = this.route.snapshot.paramMap.get("id");
    this.isWildParticipant = false;
    // check if wild guest order
    if (this.idOrder.includes("ffa")) {
      var eventId = this.idOrder.slice(3);
      this.isWildParticipant = true;
      this.eventFormService.GetEventById(eventId).then(event => {
        this.event = event;
        console.log(this.event);
        this.restaurantService
          .getRestaurants(
            [Number(this.event.RestaurantId)],
            Number(this.event.ServiceId),
            217
          )
          .then(restaurant => {
            this.data.restaurant = restaurant[0];
            this.restaurantService
              .getRestaurantDetail(
                Number(event.DeliveryId),
                Number(this.event.ServiceId)
              )
              .then(restaurantd => {
                this.data.detail = restaurantd;
                this.userService
                  .getCurrentUserId()
                  .then(user => {
                    this.user = user;
                  })
                  .then(() => {
                    this.order = {
                      Id: "1",
                      OrderDate: new Date(),
                      IdUser: this.user.Id,
                      IdEvent: this.event.EventId,
                      IdRestaurant: Number(this.event.RestaurantId),
                      IdDelivery: Number(this.event.DeliveryId),
                      FoodDetail: [],
                      IsOrdered: false,
                      Email: '',
                    };
                    this.checkedData = this.order.FoodDetail;
                    if (this.isClosed(new Date(event.CloseTime))) {
                      this.isOrder = false;
                    }
                    this.isDataAvailable = true;
                    this.loading = false;
                    this.totalBudget = Number(event.MaximumBudget);
                  });
              });
          });
      });
    } else {
      this.getOrderInfor(this.idOrder);
    }
  }
  getOrderInfor(idOrder: string) {
    return this.orderService.GetOrder(this.idOrder).then(order => {
      this.order = order;
      this.checkedData = order.FoodDetail;
      this.GetEventById(this.order.IdEvent);
    });
  }

  getRestaurant(IdRestaurant: Array<number>) {
    return this.restaurantService
      .getRestaurants(IdRestaurant, Number(this.event.ServiceId), 217)
      .then(restaurant => {
        this.data.restaurant = restaurant[0];
        this.getRestaurantDetail(this.order.IdDelivery);
      });
  }
  getRestaurantDetail(IdDelivery: number) {
    return this.restaurantService
      .getRestaurantDetail(IdDelivery, Number(this.event.ServiceId))
      .then(restaurantd => {
        this.data.detail = restaurantd;
        this.isDataAvailable = true;
        this.loading = false;
        this.totalBudget = Number(this.event.MaximumBudget);
      });
  }
  getUserById(IdUser: string) {
    return this.userService.getUserById(IdUser).then(user => {
      this.user = user;
      this.getRestaurant([this.order.IdRestaurant]);
    });
  }
  GetEventById(IdEvent: string) {
    return this.eventFormService.GetEventById(IdEvent).then(event => {
      this.event = event;
      if (this.isClosed(new Date(event.CloseTime))) {
        this.isOrder = false;
      }
      this.getUserById(this.event.HostId);
    });
  }
  isClosed(dateParameter: Date) {
    return new Date().getTime() > dateParameter.getTime();
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
  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }
  Save() {
    this.order.FoodDetail = this.foodorderlist.getAllFoodDetail();
    this.orderService
      .SetOrder(this.order, this.isWildParticipant)
      .then(result => {
        this.toast("Save!", "Dismiss");
      });
  }
}
