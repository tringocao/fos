import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { ActivatedRoute } from '@angular/router';

import * as jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import * as moment from 'moment';
import 'moment/locale/vi';
import { SummaryService } from 'src/app/services/summary/summary.service';

import { environment } from 'src/environments/environment';
 
const database: any[] = [
  {
    userId: 'e618f708-8dde-4f04-9d9b-5c5bc3a4905d',
    payExtra: 10000,
    comment: 'không hành',
    orderDetail: [
      { foodId: '', foodName: 'Chicken Rice', price: 30000 },
      { foodId: '', foodName: 'Coka', price: 10000 },
      { foodId: '', foodName: 'Draft beer', price: 20000 }
    ]
  },
  {
    userId: 'e618f708-8dde-4f04-9d9b-5c5bc3a4905d',
    payExtra: 10000,
    comment: 'không hành',
    orderDetail: [
      { foodId: '', foodName: 'Chicken Rice', img: '', price: 30000 },
      { foodId: '', foodName: 'Coka', img: '', price: 10000 },
      { foodId: '', foodName: 'Draft beer', img: '', price: 20000 }
    ]
  },
  {
    userId: 'e618f708-8dde-4f04-9d9b-5c5bc3a4905d',
    payExtra: 10000,
    comment: 'không hành',
    orderDetail: [
      { foodId: '', foodName: 'Chicken Rice', img: '', price: 30000 },
      { foodId: '', foodName: 'Coka', img: '', price: 10000 },
      { foodId: '', foodName: 'Draft beer', img: '', price: 20000 }
    ]
  }
];

@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less']
})
export class EventSummaryDialogComponent implements OnInit {
 
  @ViewChild('personGroupView', {static: false}) userGroupTab: ElementRef;

  constructor(private router: Router, private restaurantService:RestaurantService, private summaryService:SummaryService, private route: ActivatedRoute) {
    console.log(router.routerState);
  }

  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);

  dishGroupViewDisplayedColumns: string[] = [
    'picture',
    'name',
    'amount',
    'price',
    'total',
    'comment'
  ];
  personGroupViewDisplayedColumns: string[] = [
    'user',
    'food',
    'price',
    'pay-extra',
    'comment'
  ];

  restaurant: any;

  eventDetail: EventList;

  orderByDish: any[] = [
    {
      foodId: '',
      picture:
        'https://images.foody.vn/res/g1/595/prof/s60x60/foody-upload-api-foody-mobile-10-jpg-180508140146.jpg',
      name: 'Coka',
      amount: 4,
      price: 10000,
      total: 40000,
      comment: '2x không gas, 1x không đường'
    },
    {
      foodId: '',
      picture:
        'https://images.foody.vn/res/g1/595/prof/s60x60/foody-upload-api-foody-mobile-10-jpg-180508140146.jpg',
      name: 'Coka',
      amount: 4,
      price: 10000,
      total: 40000,
      comment: '2x không gas, 1x không đường'
    }
  ];

  orderByPerson: any[] = [
    {
      user: 'admin',
      food: '1xChicken rice + 1x coca',
      price: 40000,
      payExtra: 5000,
      comment: 'không hành'
    },
    {
      user: 'admin',
      food: '1xChicken rice + 1x coca',
      price: 40000,
      payExtra: 5000,
      comment: 'không hành'
    },
    {
      user: 'admin',
      food: '1xChicken rice + 1x coca',
      price: 40000,
      payExtra: 5000,
      comment: 'không hành'
    }
  ];

  dishGroupView() {
    database.map(order => {
      order.orderDetail.map(detail => {
        if (
          !this.orderByDish.includes(order => order.foodId == detail.foodId)
        ) {
          // this.orderData.push()
        }
      });
    });
  }
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }
 
  sendEmail() {
    const page = document.getElementById('report');
    const options = {background: "white", height: page.clientHeight, width: page.clientWidth, letterRendering: 1};

    html2canvas(page, options).then((pageSource) => {
      var html = '<html> <a href="'+ window.location.href + '">Click here to go to event report' + '</a></br><img style="width:500px;height:500px;" src="' + pageSource.toDataURL("image/PNG") + '" /></html>';
      console.log(html)
      this.summaryService.sendEmail(html);
    });
  }

  pageToImage() {
    window['html2canvas'] = html2canvas;

    console.log(this.userGroupTab)

    const page = document.getElementById('report');
    // const userGroupTab = document.get('personGroupView');
    const options = {background: "white", height: page.clientHeight, width: page.clientWidth, letterRendering: 1};
    // const options2 = {background: "white", height: this.userGroupTab.nativeElement.clientHeight, width: this.userGroupTab.nativeElement.clientWidth};

    html2canvas(page, options).then(pageSource => {
      // html2canvas(userGroupTab, options2).then((userTabSource) => {
        //Initialize JSPDF
        let doc = new jsPDF();
        //Converting canvas to Image
        return pageSource.toDataURL("image/PNG");
        // let userGroupData = userTabSource.toDataURL("image/PNG")
        // console.log(imgData)
        //Add image Canvas to PDF%
        // doc.addImage(pageData, 'PNG', 0, 0, window.innerWidth*0.25, window.innerHeight*0.25);
        // doc.addImage(userGroupData, 'PNG', 20, 20, 200, 200);

        // let pdfOutput = doc.output();
        // let buffer = new ArrayBuffer(pdfOutput.length);
        // let array = new Uint8Array(buffer);
        // for (let i = 0; i < pdfOutput.length; i++) {
        //     array[i] = pdfOutput.charCodeAt(i);
        // }
        // const fileName = "report.pdf";
        // doc.save(fileName);
      });
    // })
  }

  ngOnInit() {
    this.eventDetail = {
      eventTitle: 'FIKA2',
      eventId: '11',
      eventRestaurant: 'Morico - Contemporary Japanese Lifestyle - Lê Lợi',
      eventMaximumBudget: 20000,
      eventTimeToClose: '2019-09-04T11:35:00+07:00',
      eventTimeToReminder: '2019-09-04T11:35:00+07:00',
      eventHost: 'Amie Perigo',
      eventParticipants:
        'owner123@devpreciovn.onmicrosoft.com;#member@devpreciovn.onmicrosoft.com;#toandh3xco@gmail.com',
      eventCategory: 'Café/Dessert',
      eventRestaurantId: '595',
      eventServiceId: '1',
      eventDeliveryId: '',
      eventCreatedUserId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb",
      eventHostId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb"
    }

    this.route.params.subscribe(params => {
      var id = params['id'];
      this.summaryService.getEventById(id).then(result => {
        this.eventDetail = {
          eventTitle: result.Name,
          eventId: result.EventId,
          eventRestaurant: result.Restaurant,
          eventMaximumBudget: result.MaximumBudget,
          eventTimeToClose: result.Date,
          eventTimeToReminder: result.TimeToRemind,
          eventHost: result.HostName,
          eventParticipants: result.Participants,
          eventCategory: result.Category,
          eventRestaurantId: result.RestaurantId,
          eventServiceId: '1',
          eventDeliveryId: '',
          eventCreatedUserId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb",
          eventHostId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb"
        }
      });
    })

 
    this.restaurant = {
    }
 
    this.restaurantService.getRestaurants([Number(this.eventDetail.eventRestaurantId)]).then(result => {
      console.log(result[0])
      this.restaurant.address = result[0].address;
      this.restaurant.phoneNumber = "01234";
    });
    
    // this.restaurantService.getRestaurantDetailById(Number(this.eventDetail.eventRestaurantId), 217, 1).then(result =>{
    //   // this.restaurant.address = result.address;
    //   // this.restaurant. = result.

    //   console.log(result);
    // });

    this.dishGroupViewdataSource = this.orderByDish;
    this.personGroupViewdataSource = this.orderByPerson;
  }
}
