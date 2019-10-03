import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
  Input
} from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialog
} from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatTableDataSource, MatSnackBar } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { ActivatedRoute } from '@angular/router';

// import * as printJs from 'printjs';
import html2canvas from 'html2canvas';
import * as moment from 'moment';
import 'moment/locale/vi';
import { SummaryService } from 'src/app/services/summary/summary.service';
import { Event } from 'src/app/models/event';

import { environment } from 'src/environments/environment';
import { Report } from 'src/app/models/report';
import { async } from 'q';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { OrderService } from 'src/app/services/order/order.service';
import { Food } from 'src/app/models/food';
import { UserService } from 'src/app/services/user/user.service';
import { PrintService } from 'src/app/services/print/print.service';

import { OverlayContainer, Overlay } from '@angular/cdk/overlay';
import { SelectionModel } from '@angular/cdk/collections';
import { FoodDetailJson } from 'src/app/models/food-detail-json';
import { UserNotOrderMailInfo } from 'src/app/models/user-not-order-mail-info';
import { Order } from 'src/app/models/order';
import { User } from 'src/app/models/user';

import { FoodReport } from 'src/app/models/food-report';
import { Comment } from 'src/app/models/comment';
import { UserOrder } from 'src/app/models/user-order';
import { UserReorder } from 'src/app/models/user-reorder';
import { UsersOrderedFoodDialogComponent } from '../users-ordered-food-dialog/users-ordered-food-dialog.component';
import { debug } from 'util';
import { OpenEventDialogComponent } from './open-event-dialog/open-event-dialog.component';
import { EventDialogEditComponent } from '../event-dialog-edit/event-dialog-edit.component';
import { ReminderDialogComponent } from '../reminder-dialog/reminder-dialog.component';
import { FeedbackService } from 'src/app/services/feedback/feedback.service';
import { ExcelService } from 'src/app/services/print/excel/excel.service';

@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less']
})
export class EventSummaryDialogComponent implements OnInit {
  @ViewChild('personGroupView', { static: false }) userGroupTab: ElementRef;
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
    private excelService: ExcelService
  ) {
    overlayContainer.getContainerElement().classList.add('app-theme1-theme');
    console.log(router.routerState);
  }
  selection = new SelectionModel<FoodReport>(true, []);

  eventData: any;
  emailDataAvailable: boolean;
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
    'picture',
    'name',
    'amount',
    'price',
    'total',
    'totalComment',
    'numberOfUser',
    'showUsers'
  ];
  personGroupViewDisplayedColumns: string[] = [
    'user',
    'food',
    'price',
    'pay-extra',
    'comment'
  ];

  restaurant: any;

  eventDetail: Event;
  foods: FoodReport[] = [];
  foods4Reorder: string[] = [];

  orderByDish: any[] = [];
  orderByPerson: UserOrder[] = [];
  eventId: number;
  totalCost: number;
  orders: Order[];
  users: User[];
  isHostUser: boolean = false;
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  printToPdf() {
    this.printService.printDocument('report', [this.eventId.toString()], {
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
    document.getElementById('container').parentNode['style'].overflow =
      'visible';
    const page = document.getElementById('email-region');
    const options = {
      background: 'white',
      height: 800,
      width: page.clientWidth,
      letterRendering: 1,
      scale: 2
    };
    console.log(this.userGroupTab);
    html2canvas(page).then(pageSource => {
      //Converting canvas to Image
      var pageData = pageSource.toDataURL('image/PNG');
      this.summaryService
        .addReport(this.eventDetail.EventId, window.location.href, pageData)
        .then(result => {
          this.toast('Report sent to email!', 'Dismiss');
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
    this.users = [];
    this.eventDetail = new Event();

    this.route.params.subscribe(params => {
      var id = params['id'];
      this.eventId = id;
      this.eventFormService.GetEventById(id).then((result: Event) => {
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
                this.eventData.restaurant = this.restaurant;
              });
            console.log(this.restaurant);
            // this.restaurant.RestaurantUrl = "01234";
          });
        this.isHost(result);
      });
      this.orderService.GetOrdersByEventId(id).then(orders => {
        this.orders = orders;
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
        this.getUserOrderFoodAndGetTotalCost(orders);
      });
    });

    // this.dishGroupViewdataSource = this.orderByDish;
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
    });
  }

  getPersonGroupView(order, orders) {
    var orderItem: UserOrder = new UserOrder();
    this.userService
      .getUserById(order.IdUser)
      .then((user: User) => {
        orderItem.User = user;
      })
      .then(() => {
        var foods = '';
        var comments: Comment[] = [];
        var total = 0;
        order.FoodDetail.forEach(food => {
          foods += food.Value.Amount + 'x ' + food.Value.Name + ', ';
          // comment += ' ' + food.Value.Comment;
          if (food.Value.Comment !== '') {
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

          total += Number(food.Value.Total);
        });
        orderItem.Food = foods;
        orderItem.Comments = comments;
        orderItem.Price = total;
        if (this.eventDetail && this.eventDetail.MaximumBudget) {
          orderItem.PayExtra =
            Number(this.eventDetail.MaximumBudget) < total
              ? total - Number(this.eventDetail.MaximumBudget)
              : 0;
        }
        // orderItem.comment = comment;

        this.orderByPerson.push(orderItem);
        if (this.orderByPerson.length == orders.length) {
          this.personViewDataAvailable = true;
          // this.dishViewDataAvailable = true;
          this.eventData.orderByPerson = this.orderByPerson;
          this.personViewDataAvailable = true;
        }
      });
  }

  getDishGroupView(food, foodList, foodDetail, foodProceed) {
    var _food: FoodReport = {
      FoodId: food.IdFood,
      Name: food.Value.Name,
      Price: Number(food.Value.Price),
      Picture: food.Value.Photo,
      Comments:
        food.Value.Comment !== ''
          ? [
              {
                Value: food.Value.Comment,
                Amount: 1
              }
            ]
          : [],
      TotalComment: '',
      Amount: Number(food.Value.Amount),
      Total: 0,
      NumberOfUser: 0,
      UserIds: []
    };
    _food.Total = _food.Amount * _food.Price;
    if (!foodList.includes(food.IdFood)) {
      foodList.push(food.IdFood);
      this.foods.push(_food);
    } else {
      var selectedFood = this.foods.findIndex(f => f.FoodId == food.IdFood);
      this.foods[selectedFood].Amount += _food.Amount;
      this.foods[selectedFood].Total += _food.Total;
      if (food.Value.Comment !== '') {
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
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${
      this.selection.isSelected(row) ? 'deselect' : 'select'
    } row ${row.position + 1}`;
  }
  reportDisheout() {
    this.dishGroupViewDisplayedColumns = [
      'select',
      'picture',
      'name',
      'amount',
      'price',
      'total',
      'totalComment',
      'numberOfUser',
      'showUsers'
    ];
    this.reOrder = true;
  }
  openDialog(): void {
    this.resendOrder();
    if (this.eventDetail.Status == 'Closed') {
      const dialogRef = this.dialog.open(OpenEventDialogComponent, {
        scrollStrategy: this.overlay.scrollStrategies.noop(),
        autoFocus: false,
        maxWidth: '80%',
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
      'picture',
      'name',
      'amount',
      'price',
      'total',
      'totalComment',
      'numberOfUser',
      'showUsers'
    ];
  }

  sendEmailFeedback() {
    this.feedbackService
      .sendFeedbackEmail(this.eventDetail.EventId)
      .catch(error => this.toast(error, 'Dismiss'));
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
          this.toast('Reorder success', 'Dismiss');
        }
      })
      .catch(error => this.toast(error, 'Dismiss'));
  }
  closeEvent() {
    const dialogRef = this.dialog.open(ReminderDialogComponent, {
      maxHeight: '98vh',
      minWidth: '50%',
      data: {
        event: this.eventDetail,
        header: 'Close Event',
        isClosedEvent: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
  showUsers(userIds: string[], foodName: string) {
    const listUserOrderFood = this.users.filter(user =>
      userIds.includes(user.Id)
    );
    console.log('data: ', userIds, listUserOrderFood, foodName);
    const dialogRef = this.dialog.open(UsersOrderedFoodDialogComponent, {
      data: {
        users: listUserOrderFood,
        food: foodName,
        eventDetail: this.eventDetail,
        isHostUser: this.isHostUser
      },
      maxHeight: '98vh',
      minWidth: '50%'
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
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
          'user',
          'food',
          'price',
          'pay-extra',
          'comment',
          'editMakeOrder'
        ];
      }
      this.loading = false;
    });
  }

  reOpen() {
    this.summaryService
      .updateEventStatus(this.eventId.toString(), 'Reopened')
      .then(response => {
        if (response === null) {
          this.toast('Event is reopened again', 'Dismiss');
          window.location.reload();
        }
        if (response != null && response.ErrorMessage != null) {
          this.toast('Reopen event failed', 'Dismiss');
        }
      });
  }
  reOpenEvent() {
    const dialogRef = this.dialog.open(OpenEventDialogComponent, {
      scrollStrategy: this.overlay.scrollStrategies.noop(),
      autoFocus: false,
      maxWidth: '80%',
      data: this.eventDetail.Name
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result != undefined) {
        this.reOpen();
      }
    });
  }
  exportExcel() {
    this.excelService.CreateCSV(this.orderByPerson).then(value => {
      window.open(environment.apiUrl + 'api/Excel/DownloadCSV');
    });
  }
}
