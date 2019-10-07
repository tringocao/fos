import { Component, OnInit } from "@angular/core";
import { FeedbackService } from "src/app/services/feedback/feedback.service";
import { ActivatedRoute } from "@angular/router";
import { Restaurant } from "src/app/models/restaurant";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { OrderService } from "src/app/services/order/order.service";
import { Order } from "src/app/models/order";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { EventFormService } from "src/app/services/event-form/event-form.service";
import { Event } from "src/app/models/event";
import { MatTableDataSource, MatSnackBar } from "@angular/material";
import { StarRatingColor } from "../../star-rating/star-rating.component";
import { FeedBack } from "src/app/models/feed-back";
import { UserRating } from "src/app/models/user-rating";
import { FeedbackDetail } from "src/app/models/feedback-detail";
import { UserFeedback } from "src/app/models/user-feedback";

@Component({
  selector: "app-feedback",
  templateUrl: "./feedback.component.html",
  styleUrls: ["./feedback.component.less"]
})
export class FeedbackComponent implements OnInit {
  constructor(
    private feedbackService: FeedbackService,
    private route: ActivatedRoute,
    private restaurantService: RestaurantService,
    private orderService: OrderService,
    private eventFormService: EventFormService,
    private snackBar: MatSnackBar
  ) {}
  restaurant: DeliveryInfos;
  orderId: string;
  order: Order;
  orderDetail: any[];
  event: Event;
  dishViewdataSource: any = new MatTableDataSource([]);
  eventDataAvailable = false;
  loading = true;
  feedback: FeedBack = new FeedBack();

  rating = 3;
  starCount = 5;
  starColor: StarRatingColor = StarRatingColor.accent;
  starColorP: StarRatingColor = StarRatingColor.primary;
  starColorW: StarRatingColor = StarRatingColor.warn;

  dishViewDisplayedColumns: string[] = [
    "picture",
    "name",
    "price",
    "totalComment"
  ];

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.orderId = params["id"];
      this.GetOrderInfo(this.orderId);
    });
  }

  OnRatingChanged(rating) {
    //console.log(rating);
    this.rating = rating;
  }

  OnCommentChange(event, foodId) {
    //console.log(foodId);
  }

  GetOrderInfo(orderId: string) {
    return this.orderService.GetOrder(orderId).then((order: Order) => {
      this.order = order;
      this.orderDetail = order.FoodDetail.map((foodDetail: any) => {
        foodDetail.Comment = "";
        return foodDetail;
      });
      this.dishViewdataSource = this.orderDetail;
      //console.log(this.orderDetail);
      this.GetEventById(this.order.IdEvent);
    });
  }

  GetEventById(eventId: string) {
    return this.eventFormService.GetEventById(eventId).then((event: Event) => {
      this.event = event;
      //console.log(this.event)
      this.loading = false;
      this.feedback.DeliveryId = this.event.DeliveryId;
      this.feedbackService
        .getFeedbackById(this.event.DeliveryId)
        .then(result => {
          if (result !== null) {
            const ratingIndex = result.Ratings.findIndex(
              rating => rating.UserId === this.order.IdUser
            );
            if (ratingIndex !== -1) {
              this.feedback.Ratings[0] = result.Ratings[ratingIndex];
              this.rating = this.feedback.Ratings[0].Rating;
            }
            result.FoodFeedbacks.forEach(foodFeedback => {
              const foodIndex = this.orderDetail.findIndex(
                food => food.IdFood === foodFeedback.FoodId
              );
              if (foodIndex !== -1) {
                const feedBackIndex = foodFeedback.UserFeedBacks.findIndex(
                  fb => fb.UserId === this.order.IdUser
                );
                if (feedBackIndex !== -1) {
                  this.orderDetail[foodIndex].Comment =
                    foodFeedback.UserFeedBacks[feedBackIndex].Comment;
                  //console.log(this.orderDetail[foodIndex]);
                }
              }
            });
            this.dishViewdataSource = new MatTableDataSource(this.orderDetail);
          }

          this.eventDataAvailable = true;
        });
    });
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  Submit() {
    const ratingIndex = this.feedback.Ratings.findIndex(
      rating => rating.UserId === this.order.IdUser
    );
    if (ratingIndex === -1) {
      const userRating = new UserRating();
      userRating.UserId = this.order.IdUser;
      userRating.Rating = this.rating;

      this.feedback.Ratings.push(userRating);
    } else {
      this.feedback.Ratings[ratingIndex].Rating = this.rating;
    }

    this.feedback.FoodFeedbacks = this.orderDetail.map(food => {
      const feedbackDetail = new FeedbackDetail();
      const userFeedback = new UserFeedback();
      feedbackDetail.FoodId = food.IdFood;
      userFeedback.Comment = food.Comment;
      userFeedback.UserId = this.order.IdUser;
      feedbackDetail.UserFeedBacks.push(userFeedback);
      //console.log(feedbackDetail);
      return feedbackDetail;
    });
    //console.log(this.feedback);

    this.feedbackService.feedBackEvent(this.feedback).then(result => {
      this.toast('Feedback Submitted!', 'Dismiss');
    });
  }

  numberWithCommas(x: Number) {
    if (x < 0) return 0;
    if (x != undefined) {
      var parts = x.toString().split(".");
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      return parts.join(".");
    }
  }
}
