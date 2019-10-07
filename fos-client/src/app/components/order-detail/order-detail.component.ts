import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
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
import { FoodComponent } from "../dialog/food/food.component";
import { environment } from "src/environments/environment";
import { promise } from "protractor";
import { EventPromotionService } from "src/app/services/event-promotion/event-promotion.service";
import { EventPromotion } from "src/app/models/event-promotion";
import { Promotion } from "src/app/models/promotion";
import { PromotionType } from "src/app/models/promotion-type";
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
  orderUser: User;
  hostUser: User;
  order: Order;
  event: Event;
  loading = true;
  checkedData: FoodDetailJson[];
  resDetail: RestaurantDetail;
  isDataAvailable: boolean = false;
  totalBudget: Number;
  idService: number;
  isWildParticipant: boolean;
  nameEvent: string;
  eventPromotion: EventPromotion;
  promotions: Promotion[] = [];
  discountPerItem: Promotion;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private restaurantService: RestaurantService,
    private userService: UserService,
    private eventFormService: EventFormService,
    private _snackBar: MatSnackBar,
    private router: Router,
    private eventPromotionService: EventPromotionService
  ) {}

  async ngOnInit() {
    this.data = { restaurant: null, detail: null, idService: 1 };
    this.idOrder = this.route.snapshot.paramMap.get("id");

    this.isWildParticipant = false;
    // check if wild guest order
    if (this.idOrder.includes("ffa")) {
      var eventId = this.idOrder.slice(3);
      this.userService.getCurrentUser().then(user => {
        this.orderService
          .GetOrderIdOfUserInEvent(eventId, user.Id)
          .then(result => {
            if (result && result.length > 0) {
              window.location.href =
                environment.baseUrl + "make-order/" + result;
            }
          });
      });
      this.isWildParticipant = true;
      this.eventFormService.GetEventById(eventId).then(event => {
        if (event && event.EventType === 'Open') {
          this.event = event;
          //console.log(this.event);
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
                    .getCurrentUser()
                    .then(user => {
                      this.orderUser = user;
                    })
                    .then(() => {
                      this.order = {
                        Id: "1",
                        OrderDate: new Date(),
                        IdUser: this.orderUser.Id,
                        IdEvent: this.event.EventId,
                        IdRestaurant: Number(this.event.RestaurantId),
                        IdDelivery: Number(this.event.DeliveryId),
                        FoodDetail: [],
                        OrderStatus: 0,
                        Email: ""
                      };
                      this.checkedData = this.order.FoodDetail;
  
                      this.totalBudget = Number(event.MaximumBudget);
                      this.userService.getUserById(event.HostId).then(user => {
                        this.hostUser = user;
                        if (
                          this.event.Status == "Closed" &&
                          this.hostUser.Id != this.orderUser.Id
                        ) {
                          this.isOrder = false;
                        }
                        this.loading = false;
                        this.isDataAvailable = true;
  
                        this.getDbPromotions(this.event.EventId, this.order);
                      });
                      this.nameEvent = event.Name;
                    });
                });
            });
        }
      });
    } else {
      await this.orderService.GetOrder(this.idOrder).then(value => {
        if (value.OrderStatus === 2) {
          this.router.navigateByUrl("not-participant/" + this.idOrder);
        }
      });
      this.getOrderInfor(this.idOrder);
    }
  }
  getOrderInfor(idOrder: string) {
    return this.orderService.GetOrder(this.idOrder).then(order => {
      this.order = order;
      this.checkedData = order.FoodDetail;
      this.GetEventById(order);
      this.userService.getUserById(order.IdUser).then(user => {
        this.orderUser = user;
      });
    });
  }

  getRestaurant(IdRestaurant: Array<number>, order: Order) {
    return this.restaurantService
      .getRestaurants(IdRestaurant, Number(this.event.ServiceId), 217)
      .then(restaurant => {
        this.data.restaurant = restaurant[0];
        this.getRestaurantDetail(order);
      });
  }
  getRestaurantDetail(order: Order) {
    return this.restaurantService
      .getRestaurantDetail(order.IdDelivery, Number(this.event.ServiceId))
      .then(restaurantd => {
        this.data.detail = restaurantd;
        this.totalBudget = Number(this.event.MaximumBudget);
        this.userService.getCurrentUser().then(user => {
          if (this.event.Status == "Closed" && this.hostUser.Id != user.Id) {
            this.isOrder = false;
          }
          this.loading = false;
          if (this.order != null) {
            this.isDataAvailable = true;
          }
        });
      });
  }
  getUserById(IdUser: string, order: Order) {
    return this.userService.getUserById(IdUser).then(user => {
      this.hostUser = user;

      this.getRestaurant([order.IdRestaurant], order);
    });
  }
  GetEventById(order: Order) {
    return this.eventFormService.GetEventById(order.IdEvent).then(event => {
      this.event = event;
      this.nameEvent = event.Name;
      this.getDbPromotions(this.event.EventId, order);
      this.getUserById(this.event.HostId, order);
    });
  }
  isClosed(dateParameter: Date) {
    return new Date().getTime() > dateParameter.getTime();
  }
  @ViewChild(ListOrderedFoodsComponent, { static: false })
  foodorderlist: ListOrderedFoodsComponent;
  @ViewChild(FoodComponent, { static: false })
  foodlist: FoodComponent;
  getFoodFromMenu(food: FoodCheck): void {
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
  save() {
    this.order.FoodDetail = this.foodorderlist.getAllFoodDetail();
    if (this.order.FoodDetail.length > 0) {
      this.order.OrderStatus = 1;
    } else {
      this.order.OrderStatus = 0;
    }
    this.orderService
      .SetOrder(this.order, this.isWildParticipant)
      .then(result => {
        this.toast("Save!", "Dismiss");
        if (this.idOrder.includes("ffa")) {
          window.close();
        }
      })
      .catch(error => this.toast(error, "Dismiss"));
  }
  deleteFoodFromMenu($event: FoodDetailJson) {
    this.foodlist.unChecked(this.foodlist.MapFoodDetail2Food($event));
  }
  getDbPromotions(eventId: string, order: Order) {
    this.eventPromotionService.GetByEventId(Number(eventId)).then(promotion => {
      this.eventPromotion = promotion;
      //console.log(this.eventPromotion);
      this.promotions = this.eventPromotion.Promotions;
      this.discountPerItem = this.promotions
        .filter(p => p.PromotionType == PromotionType.DiscountPerItem)
        .pop();
      if (this.discountPerItem == null) {
        this.order = order;
        if (this.loading == false) this.isDataAvailable = true;
      } else {
        this.restaurantService
          .getDiscountFoodIds(
            Number(this.event.DeliveryId),
            1,
            this.discountPerItem
          )
          .then(p => {
            this.discountPerItem = p;
            this.order = order;
            if (this.loading == false) this.isDataAvailable = true;
          });
      }
    });
  }
}
