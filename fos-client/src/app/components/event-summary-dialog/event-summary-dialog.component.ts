import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
  Input
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatTableDataSource, MatSnackBar } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { ActivatedRoute } from '@angular/router';
 
import * as jsPDF from 'jspdf';
// import * as printJs from 'printjs';
import html2canvas from 'html2canvas';
import * as moment from 'moment';
import 'moment/locale/vi';
import { SummaryService } from 'src/app/services/summary/summary.service';
import { Event } from "src/app/models/event";
 
import { environment } from 'src/environments/environment';
import { Report } from 'src/app/models/report';
import { async } from 'q';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { OrderService } from 'src/app/services/order/order.service';
import { Food } from 'src/app/models/food';
import { UserService } from 'src/app/services/user/user.service';
import { PrintService } from 'src/app/services/print/print.service';
 
 
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
    private eventFormService:EventFormService,
    private orderService: OrderService,
    private userService: UserService,
    private printService: PrintService,
    private snackBar: MatSnackBar,
  ) {
    console.log(router.routerState);
  }
 
  eventData:any;
  emailDataAvailable:boolean;
  eventDataAvailable:boolean;
  dishViewDataAvailable:boolean;
  personViewDataAvailable:boolean;
  printMode:boolean;
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);
  
 
  dishGroupViewDisplayedColumns: string[] = [
    'picture',
    'name',
    'amount',
    'price',
    'total',
    'totalComment'
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
  foods: any[] = [];
  orderByDish: any[] = [];
  orderByPerson: any[] = [];
  eventId: number;
 
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
 
  printToPdf() {
    this.printService
      .printDocument('report', [this.eventId.toString()], {
        restaurant:this.restaurant,
        eventDetail:this.eventDetail,
        foods:this.foods,
        orderByPerson:this.orderByPerson
      });
    // this.printMode = true;
    // const printContent = document.getElementById("print");
 
    // // printJs('print', 'html');
    // console.log(printContent)
    // const WindowPrt = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
    // // WindowPrt.document.write('<link rel="stylesheet" type="text/css" href="event-summary-dialog.component.css">');
    // WindowPrt.document.write(printContent.innerHTML);
    // WindowPrt.document.close();
    // console.log(window.document)
    // WindowPrt.focus();
    // WindowPrt.print();
    // WindowPrt.close();
  }
 
  async sendEmail() {
    document.getElementById("container").parentNode["style"].overflow = 'visible';
    const page = document.getElementById('email-region');
    const options = {
      background: "white", height: 800, width: page.clientWidth, letterRendering: 1, scale: 2,};
    console.log(this.userGroupTab)
    html2canvas(page).then(pageSource => {
      //Converting canvas to Image
      var pageData = pageSource.toDataURL('image/PNG');
      this.summaryService.addReport(this.eventDetail.EventId, window.location.href, pageData).then(result => {
        this.toast("Report sent to email!", "Dismiss")
      });
    });
  }
  
 
  ngOnInit() {
    this.restaurant = { }
    this.eventDataAvailable = false;
    this.restaurant.isLoaded = false;
    this.printMode = false;
    this.personViewDataAvailable = false;
    this.dishViewDataAvailable = false;
    this.emailDataAvailable = false;
    this.eventData = {}

    this.dishGroupViewdataSource = this.foods;
    this.personGroupViewdataSource = this.orderByPerson;
 
    this.route.params.subscribe(params => {
      var id = params["id"];
      this.eventId = id;
      this.eventFormService.GetEventById(id).then((result: Event) => {
        console.log(result);
        this.eventDetail = result;
        this.eventDataAvailable = true;
        this.restaurantService.getRestaurants([Number(this.eventDetail.RestaurantId)], Number(this.eventDetail.ServiceId), 217).then(result => {
          console.log(result[0])
          this.restaurant = result[0];
          this.restaurant.address = result[0].Address;
 
          this.restaurantService.getRestaurantDetail(Number(this.restaurant.DeliveryId), Number(this.eventDetail.ServiceId))
          .then(result => {
            this.restaurant.Rating = Number(result.Rating);
            this.restaurant.TotalReview = Number(result.TotalReview);
            this.restaurant.isLoaded = true;
            this.loading = false;
          });
          console.log(this.restaurant)
          // this.restaurant.RestaurantUrl = "01234";
        }); 
      });
      this.orderService.GetOrdersByEventId(id).then(orders => {
        console.log(orders);
        this.foods = [];
        var foodList:string[] = [];
        orders.forEach(order => {
          this.getPersonGroupView(order, orders)
          order.FoodDetail.forEach(food => {
            this.getDishGroupView(food, foodList)
          })
        })
      })
    })
 
    // this.dishGroupViewdataSource = this.orderByDish;
  }

  getPersonGroupView(order, orders) {
    var orderItem = {
      user: '',
      food: '',
      price: 0,
      payExtra: 0,
      comments: [],
    };
    this.userService.getUserById(order.IdUser).then(user => {
      orderItem.user = user.DisplayName;
    }).then(() => {
      var foods = "";
      var comments = [];
      var total = 0;
      order.FoodDetail.forEach(food => {
        foods += food.Value.Amount + 'x ' + food.Value.Name + ', ';
        // comment += ' ' + food.Value.Comment;
        if (food.Value.Comment !== "") {
          if (comments.some(_comment => _comment.comment == food.Value.Comment)) {
            var duplicatedComment = comments.findIndex(c => c.comment == food.Value.Comment);
            comments[duplicatedComment].amount++;
          }
          else {
            comments.push({
              comment: food.Value.Comment,
              amount: 1,
            })
          }
        }

        total += Number(food.Value.Total);
      })
      orderItem.food = foods;
      orderItem.comments = comments;
      orderItem.price = total;
      if (this.eventDetail && this.eventDetail.MaximumBudget) {
        orderItem.payExtra = (Number(this.eventDetail.MaximumBudget) < total) ? (total - Number(this.eventDetail.MaximumBudget)) : 0;
      }
      // orderItem.comment = comment;

      this.orderByPerson.push(orderItem)
      if (this.orderByPerson.length == orders.length) {
        this.personViewDataAvailable = true;
        this.eventData = {
          restaurant:this.restaurant,
          eventDetail:this.eventDetail,
          foods:this.foods,
          orderByPerson:this.orderByPerson
        }
        this.emailDataAvailable = true;
      }

    })
  }

  getDishGroupView(food, foodList) {
    var _food = {
      foodId: food.IdFood,
      name: food.Value.Name,
      price: Number(food.Value.Price),
      picture: food.Value.Photo,
      comments: food.Value.Comment !== "" ? [{
        comment: food.Value.Comment,
        amount: 1
      }] : [],
      totalComment: '',
      amount: Number(food.Value.Amount),
      total: 0
    }
    _food.total = _food.amount * _food.price
    if (!foodList.includes(food.IdFood)) {
      foodList.push(food.IdFood);
      this.foods.push(_food)
    }
    else {
      var selectedFood = this.foods.findIndex(f => f.foodId == food.IdFood);
      // console.log(selectedFood)
      this.foods[selectedFood].amount += _food.amount;
      this.foods[selectedFood].total += _food.total;
      console.log(this.foods[selectedFood].comments)
      console.log(food);
      if (food.Value.Comment !== "") {
        if (this.foods[selectedFood].comments.some(_comment => _comment.comment == food.Value.Comment)) {
          var duplicatedFood = this.foods[selectedFood].comments.findIndex(c => c.comment == food.Value.Comment);
          console.log(duplicatedFood)
          this.foods[selectedFood].comments[duplicatedFood].amount++;
        }
        else {
          this.foods[selectedFood].comments.push({
            comment: food.Value.Comment,
            amount: 1
          })
        }
      }

      this.foods[selectedFood].totalComment +=  _food.totalComment;
    }
  }
}
