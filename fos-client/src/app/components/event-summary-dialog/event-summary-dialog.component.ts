import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
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


@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less']
})
export class EventSummaryDialogComponent implements OnInit {
  @ViewChild('personGroupView', { static: false }) userGroupTab: ElementRef;

  constructor(
    private router: Router,
    private restaurantService: RestaurantService,
    private summaryService: SummaryService,
    private route: ActivatedRoute,
    private eventFormService:EventFormService,
    private orderService: OrderService,
    private userService: UserService,
  ) {
    console.log(router.routerState);
  }

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
  foods: any[];
  orderByDish: any[] = [];
  orderByPerson: any[] = [];

  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }
 
  printToPdf() {
    // this.printMode = true;
    const printContent = document.getElementById("print");

    // printJs('print', 'html');
    console.log(printContent)
    const WindowPrt = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
    // WindowPrt.document.write('<link rel="stylesheet" type="text/css" href="event-summary-dialog.component.css">');
    WindowPrt.document.write(printContent.innerHTML);
    WindowPrt.document.close();
    console.log(window.document)
    WindowPrt.focus();
    WindowPrt.print();
    WindowPrt.close();
  }

  async sendEmail() {
    const page = document.getElementById('print');
    const options = {
      background: "white", height: 800, width: page.clientWidth, letterRendering: 1, scale: 2,};
    console.log(this.userGroupTab)
    // pageSource.toDataURL("image/PNG")
    // let doc = new jsPDF();
    // var html = '<html> <a href="'+ window.location.href + '">Click here to go to event report' + '</a></html>';
    html2canvas(page, options).then(pageSource => {
      //Converting canvas to Image
      var pageData = pageSource.toDataURL('image/PNG');
      // let userGroupData = userTabSource.toDataURL("image/PNG")
      // Add image Canvas to PDF%
      // doc.addImage(pageData, 'PNG', 0, 0, window.innerWidth*0.25, window.innerHeight*0.25);

      this.summaryService.addReport(this.eventDetail.EventId, window.location.href, pageData)
      // doc.addImage(userGroupData, 'PNG', 20, 20, 200, 200);
      console.log('html2canvas');
    });
  }

  ngOnInit() {
    this.restaurant = { }
    this.eventDataAvailable = false;
    this.restaurant.isLoaded = false;
    this.printMode = false;
    this.personViewDataAvailable = false;
    this.dishViewDataAvailable = false;

    this.route.params.subscribe(params => {
      var id = params['id'];
      this.eventFormService
      .GetEventById(id).then((result:Event) => {
        console.log(result)
        this.eventDetail = result;
        this.eventDataAvailable = true;
        this.restaurantService.getRestaurants([Number(this.eventDetail.RestaurantId)]).then(result => {
          console.log(result[0])
          this.restaurant = result[0];
          this.restaurant.address = result[0].Address;

          this.restaurantService.getRestaurantDetail(Number(this.restaurant.DeliveryId))
          .then(result => {
            this.restaurant.Rating = Number(result.Rating);
            this.restaurant.TotalReview = Number(result.TotalReview);
            this.restaurant.isLoaded = true;
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
          order.FoodDetail.forEach(food => {
            var _food = {
              foodId: food.IdFood,
              name: food.Value.Name,
              price: Number(food.Value.Price),
              picture: food.Value.Photo,
              comments: [
                {
                  comment: food.Value.Comment,
                  amount: 1
                }
              ],
              totalComment: food.Value.Comment,
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
              console.log(selectedFood)
              this.foods[selectedFood].amount += _food.amount;
              this.foods[selectedFood].total += _food.total;
              // this.foods[selectedFood].comments.forEach(_comment => {
              //   // if (food.c) {

              //   // }
              // });
              this.foods[selectedFood].totalComment += ' ' + _food.totalComment;
              // if (comment == _food.comment) {
              //   this.foods[selectedFood].comment = _food.tota
              // }
              // this.foods[selectedFood].comment += _food.tota
            }
          })
        })
        this.dishGroupViewdataSource = this.foods;
        console.log(this.foods)
        this.dishViewDataAvailable = true;

        orders.forEach(order => {
          var orderItem = {
            user: 'admin',
            food: '1xChicken rice + 1x coca',
            price: 40000,
            payExtra: 5000,
            comment: 'không hành'
          };
          this.userService.getUserById(order.IdUser).then(user => {
            orderItem.user = user.DisplayName;
          }).then(() => {
            var foods = "";
            var comment = "";
            var total = 0;
            order.FoodDetail.forEach(food => {
              foods += food.Value.Amount + 'x ' + food.Value.Name + ', ';
              comment += ' ' + food.Value.Comment; // try if same name
              total += Number(food.Value.Total);
            })
            orderItem.food = foods;
            orderItem.payExtra = (Number(this.eventDetail.MaximumBudget) < total) ? (total - Number(this.eventDetail.MaximumBudget)) : 0;
            orderItem.comment = comment;

            this.orderByPerson.push(orderItem)
            this.personViewDataAvailable = this.orderByPerson.length == orders.length;
          })
        })
      })
    })

    // this.dishGroupViewdataSource = this.orderByDish;
    this.personGroupViewdataSource = this.orderByPerson;
  }
}
