import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
  Input
} from "@angular/core";
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialog
} from "@angular/material/dialog";
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

import { OverlayContainer, Overlay } from "@angular/cdk/overlay";
import { SelectionModel } from "@angular/cdk/collections";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { UserNotOrderMailInfo } from "src/app/models/user-not-order-mail-info";
import { Order } from "src/app/models/order";
import { User } from "src/app/models/user";

import { FoodReport } from "src/app/models/food-report";
import { Comment } from "src/app/models/comment";
import { UserOrder } from "src/app/models/user-order";
import { UserReorder } from "src/app/models/user-reorder";
import { UsersOrderedFoodDialogComponent } from "../users-ordered-food-dialog/users-ordered-food-dialog.component";
import { debug } from "util";
import { OpenEventDialogComponent } from "./open-event-dialog/open-event-dialog.component";
import { EventDialogEditComponent } from "../event-dialog-edit/event-dialog-edit.component";
import { ReminderDialogComponent } from "../reminder-dialog/reminder-dialog.component";
import { FeedbackService } from "src/app/services/feedback/feedback.service";
import { Promotion } from "src/app/models/promotion";
import { PromotionType } from "src/app/models/promotion-type";
import { EventPromotionService } from "src/app/services/event-promotion/event-promotion.service";
import { EventPromotion } from "src/app/models/event-promotion";
import { ExcelService } from "src/app/services/print/excel/excel.service";
import { ExcelModel } from "src/app/models/excel-model";
import { DataRoutingService } from "src/app/data-routing.service";
import { Subscription } from "rxjs";

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
    private overlayContainer: OverlayContainer,
    private dialog: MatDialog,
    public dialogRef: MatDialogRef<UsersOrderedFoodDialogComponent>,
    private overlay: Overlay,
    private feedbackService: FeedbackService,
    private eventPromotionService: EventPromotionService,
    private excelService: ExcelService,
    private dataRouting: DataRoutingService
  ) {
    this.getNavTitleSubscription = this.dataRouting
      .getNavTitle()
      .subscribe((appTheme: string) => (this.appTheme = appTheme));
    overlayContainer
      .getContainerElement()
      .classList.add("app-" + this.appTheme + "-theme");
  }
  ngOnDestroy() {
    // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
    this.getNavTitleSubscription.unsubscribe();
  }
  private getNavTitleSubscription: Subscription;
  appTheme: string;
  selection = new SelectionModel<FoodReport>(true, []);

  eventData: any;
  emailDataAvailable: boolean;
  promotionDataAvailable: boolean = false;
  eventDataAvailable: boolean;
  dishViewDataAvailable: boolean;
  personViewDataAvailable: boolean;
  printMode: boolean;
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);
  reOrder: boolean = false;
  usersReorder: User[] = [];
  //food: User[];

  dishGroupViewDisplayedColumns: string[] = [
    "picture",
    "name",
    "amount",
    "totalComment",
    "price",
    "total",
    "numberOfUser"
  ];
  personGroupViewDisplayedColumns: string[] = [
    "user",
    "food",
    "price",
    "pay-extra",
    "comment"
  ];

  restaurant: any;

  eventDetail: Event;
  foods: FoodReport[] = [];
  foods4Reorder: string[] = [];

  discountedFoods: number[];
  discountedPercent: number = 0;

  promotions: Promotion[];
  promotion: Promotion;
  discountedFoodIds: { [key: number]: number };
  eventPromotion: EventPromotion;

  orderByDish: any[] = [];
  orderByPerson: UserOrder[] = [];
  eventId: number;
  totalCost: number;
  baseTotalCost: number = 0;
  adjustedTotalCost: number;
  orders: Order[];
  users: User[];
  isHostUser: boolean = false;
  promoteChangeAble: boolean = true;

  toStandardDate(date: Date) {
    return moment(date).format("MM/DD/YYYY HH:mm");
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
      orderByPerson: this.orderByPerson,
      users: this.users,
      total: this.totalCost
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
    //console.log(this.userGroupTab);
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
    this.totalCost = 0;
    this.adjustedTotalCost = 0;
    this.users = [];
    this.eventDetail = new Event();

    this.route.params.subscribe(params => {
      var id = params["id"];
      this.eventId = id;
      this.eventFormService.GetEventById(id).then((result: Event) => {
        this.eventDetail = result;
        this.eventData.eventDetail = this.eventDetail;
        this.eventDataAvailable = true;

        this.eventPromotionService
          .GetByEventId(Number(id))
          .then(eventPromotion => {
            if (eventPromotion) {
              this.promotions = eventPromotion.Promotions;
              this.eventPromotion = eventPromotion;
              this.promotionDataAvailable = true;
              const discountPerItemPromotions = eventPromotion.Promotions.filter(
                p => p.PromotionType === PromotionType.DiscountPerItem
              );
              if (discountPerItemPromotions.length > 0) {
                this.promotion = discountPerItemPromotions[0];
                if (this.eventDetail.Status === "Closed") {
                  this.discountedFoodIds = this.promotion.DiscountedFoodIds;
                  this.promoteChangeAble = false;
                  this.getOrdersInfo(id);
                } else {
                  this.restaurantService
                    .getDiscountFoodIds(
                      Number(this.eventDetail.DeliveryId),
                      1,
                      this.promotion
                    )
                    .then(eventPromotion => {
                      //console.log(eventPromotion.DiscountedFoodIds);
                      this.promotion = eventPromotion;
                      const promontionIndex = this.eventPromotion.Promotions.findIndex(
                        p => p.PromotionType === PromotionType.DiscountPerItem
                      );
                      if (promontionIndex !== -1) {
                        this.eventPromotion.Promotions[
                          promontionIndex
                        ] = this.promotion;
                        this.eventPromotionService.UpdateEventPromotion(
                          this.eventPromotion
                        );
                      }
                      this.discountedFoodIds = eventPromotion.DiscountedFoodIds;
                      this.getOrdersInfo(id);
                    });
                }
              } else {
                this.getOrdersInfo(id);
              }
            } else {
              this.eventPromotion = new EventPromotion();
              this.eventPromotion.EventId = id;
              this.promotions = this.eventPromotion.Promotions;
              this.promotionDataAvailable = true;
              this.getOrdersInfo(id);
            }
            this.restaurantService
              .getRestaurants(
                [Number(this.eventDetail.RestaurantId)],
                Number(this.eventDetail.ServiceId),
                217
              )
              .then(result => {
                //console.log(result[0]);
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
                    this.eventData.restaurant = this.restaurant;
                  });
                //console.log(this.restaurant);
                // this.restaurant.RestaurantUrl = "01234";
              });
            this.isHost(result);
          });
      });
    });

    // this.dishGroupViewdataSource = this.orderByDish;
  }
  getOrdersInfo(id) {
    this.orderService.GetOrdersByEventId(id).then(orders => {
      this.orders = orders;
      //console.log(orders);
      var foodList: string[] = [];
      var orderProceed = 0;
      this.orders = orders;
      orders.forEach(order => {
        this.getPersonGroupView(order, orders);

        order.FoodDetail.forEach(food => {
          this.getDishGroupView(food, foodList, order.FoodDetail, orderProceed);
        });
        orderProceed++;
        if (orderProceed == orders.length) {
          //console.log(orderProceed);
          this.dishGroupViewdataSource = new MatTableDataSource(this.foods);
          this.dishViewDataAvailable = true;
          this.eventData.foods = this.foods;
        }
      });
      this.getUserOrderFoodAndGetTotalCost(orders);
    });
  }

  getUserOrderFoodAndGetTotalCost(orders: Order[]) {
    this.foods.forEach(item => {
      orders.forEach(order => {
        if (order.FoodDetail.some(detail => detail.IdFood === item.FoodId)) {
          item.NumberOfUser += 1;
          item.UserIds.push(order.IdUser);
        }
      });
      this.totalCost = item.Total + this.totalCost;
      this.eventData.totalCost = this.totalCost;
    });
    this.baseTotalCost = this.totalCost;
    this.adjustedTotalCost = this.totalCost;
    if (this.promotions) {
      this.adjustPrice(this.promotions);
    }
  }

  getPersonGroupView(order, orders) {
    var orderItem: UserOrder = new UserOrder();
    this.userService
      .getUserById(order.IdUser)
      .then((user: User) => {
        orderItem.User = user;
        this.users.push(user);
        this.eventData.users = this.users;
      })
      .then(() => {
        var foods = "";
        var comments: Comment[] = [];
        var total = 0;
        order.FoodDetail.forEach(food => {
          //console.log(food);
          foods += food.Value.Amount + "x " + food.Value.Name + ", ";
          // comment += ' ' + food.Value.Comment;
          if (food.Value.Comment !== "") {
            if (
              comments.some(_comment => _comment.Value == food.Value.Comment)
            ) {
              var duplicatedComment = comments.findIndex(
                c => c.Value == food.Value.Comment
              );
              comments[duplicatedComment].Amount++;
            } else {
              comments.push({
                Value: food.Value.Comment,
                Amount: 1
              });
            }
          }

          total +=
            Number(this.getDiscountedPrice(food)) * Number(food.Value.Amount);
        });
        orderItem.Food = foods;
        orderItem.Comments = comments;

        orderItem.Price = total;
        orderItem.Cost = total;
        this.getPayExtra(orderItem);
        // orderItem.comment = comment;

        this.orderByPerson.push(orderItem);
        if (this.orderByPerson.length == orders.length) {
          const orderedUsers = this.orderByPerson.filter(
            order => order.Food !== ""
          );
          this.orderByPerson = orderedUsers;

          this.personGroupViewdataSource = new MatTableDataSource(
            this.orderByPerson
          );
          // this.dishViewDataAvailable = true;
          this.eventData.orderByPerson = this.orderByPerson;
          this.personViewDataAvailable = true;
          if (this.promotions) {
            this.adjustPrice(this.promotions);
          }
        }
      });
  }

  getDishGroupView(food, foodList, foodDetail, foodProceed) {
    //console.log(food);
    var _food: FoodReport = {
      FoodId: food.IdFood,
      Name: food.Value.Name,
      Price: Number(food.Value.Price),
      Picture: food.Value.Photo,
      Comments:
        food.Value.Comment !== ""
          ? [
              {
                Value: food.Value.Comment,
                Amount: 1
              }
            ]
          : [],
      TotalComment: "",
      Amount: Number(food.Value.Amount),
      Total: 0,
      NumberOfUser: 0,
      UserIds: []
    };
    _food.Price = this.getDiscountedPrice(food);
    // _food.Price = Number(_foo);
    _food.Total = _food.Amount * _food.Price;
    if (!foodList.includes(food.IdFood)) {
      foodList.push(food.IdFood);
      this.foods.push(_food);
    } else {
      var selectedFood = this.foods.findIndex(f => f.FoodId == food.IdFood);
      this.foods[selectedFood].Amount += _food.Amount;
      this.foods[selectedFood].Total += _food.Total;
      if (food.Value.Comment !== "") {
        if (
          this.foods[selectedFood].Comments.some(
            (_comment: Comment) => _comment.Value == food.Value.Comment
          )
        ) {
          var duplicatedFood = this.foods[selectedFood].Comments.findIndex(
            (c: Comment) => c.Value == food.Value.Comment
          );
          this.foods[selectedFood].Comments[duplicatedFood].Amount++;
        } else {
          this.foods[selectedFood].Comments.push({
            Value: food.Value.Comment,
            Amount: 1
          });
        }
      }
      this.foods[selectedFood].TotalComment += _food.TotalComment;
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

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dishGroupViewdataSource.data.length;
    return numSelected === numRows;
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
    this.dishGroupViewDisplayedColumns = [
      "select",
      "picture",
      "name",
      "amount",
      "totalComment",
      "price",
      "total",
      "numberOfUser"
    ];
    this.reOrder = true;
  }
  openDialog(): void {
    this.resendOrder();
    if (this.eventDetail.Status == "Closed") {
      const dialogRef = this.dialog.open(OpenEventDialogComponent, {
        scrollStrategy: this.overlay.scrollStrategies.noop(),
        autoFocus: false,
        maxWidth: "80%",
        data: this.eventDetail.Name
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result != undefined) {
          this.reOpen();
        }
      });
    }
  }
  resendOrder() {
    this.selection.selected.forEach(value => {
      value.UserIds.forEach(id => {
        this.usersReorder.push(this.users.filter(u => u.Id == id)[0]);
      });
      this.foods4Reorder.push(value.Name);
    });
    this.usersReorder = Array.from(new Set(this.usersReorder));
    this.sendEmailReorder();
  }
  onNoClick() {
    this.reOrder = false;

    this.dishGroupViewDisplayedColumns = [
      "picture",
      "name",
      "amount",
      "totalComment",
      "price",
      "total",
      "numberOfUser"
    ];
  }

  sendEmailFeedback() {
    this.feedbackService
      .sendFeedbackEmail(this.eventDetail.EventId)
      .then(result => {
        this.toast("Feedback Email Sent!", "Dismiss");
      })
      .catch(error => this.toast(error, "Dismiss"));
    this.toast("Feedback Email Sent!", "Dismiss");
  }
  sendEmailReorder() {
    const info: UserReorder[] = [];
    this.usersReorder.forEach(user => {
      const element = new UserReorder();
      element.EventRestaurant = this.eventDetail.Restaurant;
      element.EventTitle = this.eventDetail.Name;
      element.UserMail = user.Mail;
      element.OrderId = this.orders.filter(o => o.IdUser === user.Id)[0].Id;
      element.FoodName = this.foods4Reorder;
      element.UserName = user.DisplayName;
      info.push(element);
    });
    this.orderService
      .SendEmailToReOrderedUser(info)
      .then(response => {
        if (response === null) {
          this.toast("Reorder success", "Dismiss");
        }
      })
      .catch(error => this.toast(error, "Dismiss"));
  }
  closeEvent() {
    const dialogRef = this.dialog.open(ReminderDialogComponent, {
      maxHeight: "98vh",
      minWidth: "50%",
      data: {
        event: this.eventDetail,
        header: "Close Event",
        isClosedEvent: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      //console.log("The dialog was closed");
    });
  }
  showUsers(userIds: string[], foodName: string) {
    const listUserOrderFood = this.users.filter(user =>
      userIds.includes(user.Id)
    );
    //console.log("data: ", userIds, listUserOrderFood, foodName);
    const dialogRef = this.dialog.open(UsersOrderedFoodDialogComponent, {
      data: {
        users: listUserOrderFood,
        food: foodName,
        eventDetail: this.eventDetail,
        isHostUser: this.isHostUser
      },
      maxHeight: "98vh",
      minWidth: "50%"
    });
    dialogRef.afterClosed().subscribe(result => {
      //console.log(result);
    });
  }
  closeDialog() {
    this.dialogRef.close();
  }
  isHost(event: Event) {
    this.userService.getCurrentUser().then(user => {
      this.isHostUser = user.Id == event.HostId;
      if (this.isHostUser) {
        this.personGroupViewDisplayedColumns = [
          "user",
          "food",
          "price",
          "pay-extra",
          "comment",
          "editMakeOrder"
        ];
      }
      this.loading = false;
    });
  }

  reOpen() {
    this.summaryService
      .updateEventStatus(this.eventId.toString(), "Reopened")
      .then(response => {
        if (response === null) {
          this.toast("Event is reopened again", "Dismiss");
          window.location.reload();
        }
        if (response != null && response.ErrorMessage != null) {
          this.toast("Reopen event failed", "Dismiss");
        }
      });
  }
  reOpenEvent() {
    const dialogRef = this.dialog.open(OpenEventDialogComponent, {
      scrollStrategy: this.overlay.scrollStrategies.noop(),
      autoFocus: false,
      maxWidth: "80%",
      data: this.eventDetail.Name
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.reOpen();
      }
    });
  }
  getDiscountedPrice(food: FoodDetailJson): number {
    if (
      this.promotion &&
      this.discountedFoodIds &&
      this.discountedFoodIds[food.IdFood]
    ) {
      //console.log(
      //Number(food.Value.Price) + this.discountedFoodIds[food.IdFood]
      //);
      return Number(food.Value.Price) + this.discountedFoodIds[food.IdFood];
    }
    return Number(food.Value.Price);
  }
  getOriginalPrice(report: FoodReport) {
    if (this.discountedFoodIds && this.discountedFoodIds[report.FoodId]) {
      return report.Price - this.discountedFoodIds[report.FoodId];
    }
    return report.Price;
  }

  getPayExtra(orderItem) {
    if (this.eventDetail && this.eventDetail.MaximumBudget) {
      orderItem.PayExtra =
        Number(this.eventDetail.MaximumBudget) < orderItem.Cost
          ? orderItem.Cost - Number(this.eventDetail.MaximumBudget)
          : 0;
    }
  }
  adjustPrice(promotions: Promotion[]) {
    this.eventPromotion.Promotions = promotions;
    if (this.baseTotalCost > 0) {
      this.adjustedTotalCost = this.baseTotalCost;
      this.orderByPerson.forEach(order => {
        order.Cost = order.Price;
        this.getPayExtra(order);
      });
      promotions.forEach((promotion: Promotion) => {
        if (
          !promotion.IsPercent &&
          promotion.PromotionType !== PromotionType.ShipFee &&
          promotion.PromotionType !== PromotionType.DiscountPerItem
        ) {
          this.adjustedTotalCost = this.adjustedTotalCost - promotion.Value;
          this.orderByPerson.forEach(order => {
            order.Cost =
              order.Cost - promotion.Value / this.orderByPerson.length;
            this.getPayExtra(order);
          });
        } else if (promotion.IsPercent) {
          if (promotion.Value > 0) {
            if (promotion.PromotionType === PromotionType.DiscountAll) {
              this.adjustedTotalCost =
                this.adjustedTotalCost -
                (this.adjustedTotalCost / 100) * promotion.Value;
              this.orderByPerson.forEach(order => {
                order.Cost = order.Cost - (order.Cost / 100) * promotion.Value;
                this.getPayExtra(order);
              });
            }
          }
        } else if (promotion.PromotionType === PromotionType.ShipFee) {
          this.adjustedTotalCost = this.adjustedTotalCost + promotion.Value;
          this.orderByPerson.forEach(order => {
            order.Cost =
              order.Cost + promotion.Value / this.orderByPerson.length;
            this.getPayExtra(order);
          });
        }
      });
      this.totalCost = this.adjustedTotalCost;
    }
  }
  exportExcel() {
    // restaurant: this.restaurant,
    //   eventDetail: this.eventDetail,
    //   foods: this.foods,
    //   orderByPerson: this.orderByPerson,
    //   users: this.users

    const excelModel: ExcelModel = {
      Event: this.eventDetail,
      FoodReport: this.foods,
      RestaurantExcel: this.restaurant,
      UserOrder: this.orderByPerson,
      User: this.users,
      Total: this.totalCost
    };
    this.excelService.CreateCSV(excelModel).then(value => {
      if (value === true) {
        window.open(
          environment.apiUrl +
            "api/Excel/DownloadCSV?eventId=" +
            this.eventDetail.EventId.toString()
        );
      }
    });
  }
}
