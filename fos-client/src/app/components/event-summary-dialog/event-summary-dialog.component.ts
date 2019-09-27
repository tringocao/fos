import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
  Input
} from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { MatTableDataSource, MatSnackBar } from "@angular/material";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { ActivatedRoute } from "@angular/router";

// import * as printJs from 'printjs';
import html2canvas from "html2canvas";
import * as moment from "moment";
import "moment/locale/vi";
import { SummaryService } from "src/app/services/summary/summary.service";
import { Event } from "src/app/models/event";

import { environment } from "src/environments/environment";
import { Report } from "src/app/models/report";
import { async } from "q";
import { EventFormService } from "src/app/services/event-form/event-form.service";
import { OrderService } from "src/app/services/order/order.service";
import { Food } from "src/app/models/food";
import { UserService } from "src/app/services/user/user.service";
import { PrintService } from "src/app/services/print/print.service";

import { OverlayContainer } from "@angular/cdk/overlay";
import { SelectionModel } from "@angular/cdk/collections";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { UserNotOrderMailInfo } from "src/app/models/user-not-order-mail-info";
import { Order } from "src/app/models/order";
import { User } from "src/app/models/user";

import {OverlayContainer} from '@angular/cdk/overlay';
import { FoodReport } from 'src/app/models/food-report';
import { Comment } from 'src/app/models/comment';
import { UserOrder } from 'src/app/models/user-order';
import { User } from 'src/app/models/user';
 
 
@Component({
  selector: "app-event-summary-dialog",
  templateUrl: "./event-summary-dialog.component.html",
  styleUrls: ["./event-summary-dialog.component.less"]
})
export class EventSummaryDialogComponent implements OnInit {
  @ViewChild("personGroupView", { static: false }) userGroupTab: ElementRef;
  loading: boolean = true;

  constructor(
    private router: Router,
    private restaurantService: RestaurantService,
    private summaryService: SummaryService,
    private route: ActivatedRoute,
    private eventFormService: EventFormService,
    private orderService: OrderService,
    private userService: UserService,
    private printService: PrintService,
    private snackBar: MatSnackBar,
    private overlayContainer: OverlayContainer
  ) {
    overlayContainer.getContainerElement().classList.add("app-theme1-theme");
    console.log(router.routerState);
  }
  selection = new SelectionModel<any>(true, []);

  eventData: any;
  emailDataAvailable: boolean;
  eventDataAvailable: boolean;
  dishViewDataAvailable: boolean;
  personViewDataAvailable: boolean;
  printMode: boolean;
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);
  reOrder: boolean = false;
  usersReorder: User[];

  dishGroupViewDisplayedColumns: string[] = [
    "picture",
    "name",
    "amount",
    "price",
    "total",
    "totalComment"
  ];
  personGroupViewDisplayedColumns: string[] = [
    "user",
    "food",
    "price",
    "pay-extra",
    "comment"
  ];

  restaurant: any;
  orders: Order[];
  eventDetail: Event;
  foods: FoodReport[] = [];
  orderByDish: any[] = [];
  orderByPerson: any[] = [];
  eventId: number;

  toStandardDate(date: Date) {
    return moment(date).format("DD/MM/YYYY HH:mm");
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  printToPdf() {
    this.printService.printDocument("report", [this.eventId.toString()], {
      restaurant: this.restaurant,
      eventDetail: this.eventDetail,
      foods: this.foods,
      orderByPerson: this.orderByPerson
    });
  }

  isEmailDataAvailable() {
    return (
      this.restaurant &&
      this.restaurant.isLoaded &&
      this.personViewDataAvailable &&
      this.dishViewDataAvailable
    );
  }

  sendEmail() {
    document.getElementById("container").parentNode["style"].overflow =
      "visible";
    const page = document.getElementById("email-region");
    const options = {
      background: "white",
      height: 800,
      width: page.clientWidth,
      letterRendering: 1,
      scale: 2
    };
    console.log(this.userGroupTab);
    html2canvas(page).then(pageSource => {
      //Converting canvas to Image
      var pageData = pageSource.toDataURL("image/PNG");
      this.summaryService
        .addReport(this.eventDetail.EventId, window.location.href, pageData)
        .then(result => {
          this.toast("Report sent to email!", "Dismiss");
        });
    });
  }

  ngOnInit() {
    this.restaurant = {};
    this.eventDataAvailable = false;
    this.restaurant.isLoaded = false;
    this.printMode = false;
    this.personViewDataAvailable = false;
    this.dishViewDataAvailable = false;
    this.emailDataAvailable = false;
    this.eventData = {};

    this.dishGroupViewdataSource = this.foods;
    this.personGroupViewdataSource = this.orderByPerson;

    this.route.params.subscribe(params => {
      var id = params["id"];
      this.eventId = id;
      this.eventFormService.GetEventById(id).then((result: Event) => {
        console.log(result);
        this.eventDetail = result;
        this.eventData.eventDetail = this.eventDetail;
        this.eventDataAvailable = true;
        this.restaurantService
          .getRestaurants(
            [Number(this.eventDetail.RestaurantId)],
            Number(this.eventDetail.ServiceId),
            217
          )
          .then(result => {
            console.log(result[0]);
            this.restaurant = result[0];
            this.restaurant.address = result[0].Address;

            this.restaurantService
              .getRestaurantDetail(
                Number(this.restaurant.DeliveryId),
                Number(this.eventDetail.ServiceId)
              )
              .then(result => {
                this.restaurant.Rating = Number(result.Rating);
                this.restaurant.TotalReview = Number(result.TotalReview);
                this.restaurant.isLoaded = true;
                this.loading = false;
                this.eventData.restaurant = this.restaurant;
              });
            console.log(this.restaurant);
            // this.restaurant.RestaurantUrl = "01234";
          });
      });
      this.orderService.GetOrdersByEventId(id).then(orders => {
        console.log(orders);
        var foodList: string[] = [];
        var orderProceed = 0;
        this.orders = orders;
        orders.forEach(order => {
          this.getPersonGroupView(order, orders);

          order.FoodDetail.forEach(food => {
            this.getDishGroupView(
              food,
              foodList,
              order.FoodDetail,
              orderProceed
            );
          });
          orderProceed++;
          if (orderProceed == orders.length) {
            console.log(orderProceed);
            this.dishGroupViewdataSource = new MatTableDataSource(this.foods);
            this.dishViewDataAvailable = true;
            this.eventData.foods = this.foods;
          }
        });
      });
    });

    // this.dishGroupViewdataSource = this.orderByDish;
  }

  getPersonGroupView(order, orders) {
    var orderItem:UserOrder = {
      User: null,
      Food: '',
      Price: 0,
      PayExtra: 0,
      Comments: [],
    };
    this.userService.getUserById(order.IdUser).then((user: User) => {
      orderItem.User = user;
    }).then(() => {
      var foods = "";
      var comments:Comment[] = [];
      var total = 0;
      order.FoodDetail.forEach(food => {
        foods += food.Value.Amount + 'x ' + food.Value.Name + ', ';
        // comment += ' ' + food.Value.Comment;
        if (food.Value.Comment !== "") {
          if (comments.some(_comment => _comment.Value == food.Value.Comment)) {
            var duplicatedComment = comments.findIndex(c => c.Value == food.Value.Comment);
            comments[duplicatedComment].Amount++;
          }
          else {
            comments.push({
              Value: food.Value.Comment,
              Amount: 1,
            })
          }
        }

        total += Number(food.Value.Total);
      })
      orderItem.Food = foods;
      orderItem.Comments = comments;
      orderItem.Price = total;
      if (this.eventDetail && this.eventDetail.MaximumBudget) {
        orderItem.PayExtra = (Number(this.eventDetail.MaximumBudget) < total) ? (total - Number(this.eventDetail.MaximumBudget)) : 0;
      }
      // orderItem.comment = comment;

        this.orderByPerson.push(orderItem);
        if (this.orderByPerson.length == orders.length) {
          this.personViewDataAvailable = true;
          // this.dishViewDataAvailable = true;
          this.eventData.orderByPerson = this.orderByPerson;
          this.personViewDataAvailable = true;
          // this.eventData = {
          //   restaurant:this.restaurant,
          //   eventDetail:this.eventDetail,
          //   foods:this.foods,
          //   orderByPerson:this.orderByPerson
          // }
          // console.log(this.eventData)
          // this.emailDataAvailable = true;
        }
      });
  }

  getDishGroupView(food, foodList, foodDetail, foodProceed) {
    var _food:FoodReport = {
      FoodId: food.IdFood,
      Name: food.Value.Name,
      Price: Number(food.Value.Price),
      Picture: food.Value.Photo,
      Comments: food.Value.Comment !== "" ? [{
        Value: food.Value.Comment,
        Amount: 1
      }] : [],
      TotalComment: '',
      Amount: Number(food.Value.Amount),
      Total: 0,
      NumberOfUser: 0,
      UserIds: []
    }
    _food.Total = _food.Amount * _food.Price
    if (!foodList.includes(food.IdFood)) {
      foodList.push(food.IdFood);
      this.foods.push(_food)
    }
    else {
      var selectedFood = this.foods.findIndex(f => f.FoodId == food.IdFood);
      // console.log(selectedFood)
      this.foods[selectedFood].Amount += _food.Amount;
      this.foods[selectedFood].Total += _food.Total;
      if (food.Value.Comment !== "") {
        if (this.foods[selectedFood].Comments.some((_comment:Comment) => _comment.Value == food.Value.Comment)) {
          var duplicatedFood = this.foods[selectedFood].Comments.findIndex((c:Comment) => c.Value == food.Value.Comment);
          this.foods[selectedFood].Comments[duplicatedFood].Amount++;
        }
        else {
          this.foods[selectedFood].Comments.push({
            Value: food.Value.Comment,
            Amount: 1
          })
        }
      }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dishGroupViewdataSource.data.forEach(row =>
          this.selection.select(row)
        );
  }
      this.foods[selectedFood].TotalComment +=  _food.TotalComment;
    }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? "select" : "deselect"} all`;
    }
    return `${
      this.selection.isSelected(row) ? "deselect" : "select"
    } row ${row.position + 1}`;
  }
  reportDisheout() {
    this.reOrder = true;
    this.dishGroupViewDisplayedColumns = [
      "select",
      "picture",
      "name",
      "amount",
      "price",
      "total",
      "totalComment"
    ];
  }
  resendOrder() {
    this.selection.selected.forEach(value =>
      this.usersReorder.push(value.user)
    );
    this.openEvent();
  }
  onNoClick() {
    this.reOrder = false;
    this.dishGroupViewDisplayedColumns = [
      "picture",
      "name",
      "amount",
      "price",
      "total",
      "totalComment"
    ];
  }
  openEvent() {
    this.summaryService
      .updateEventStatus(this.eventId.toString(), "Opened")
      .then(response => {
        if (response === null) {
          this.toast("Event is opened again", "Dismiss");
          window.location.reload();
        }
        if (response != null && response.ErrorMessage != null) {
          this.toast("Open event failed", "Dismiss");
        }
      });
  }
}
