import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef  } from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import * as jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import * as moment from 'moment';
import 'moment/locale/vi';
 
const database: any[] = [
  {userId:"e618f708-8dde-4f04-9d9b-5c5bc3a4905d", payExtra: 10000, comment:'không hành', orderDetail: [
    {foodId: '', foodName: 'Chicken Rice', price: 30000},
    {foodId: '', foodName: 'Coka', price: 10000},
    {foodId: '', foodName: 'Draft beer', price: 20000},
  ]},
  {userId:"e618f708-8dde-4f04-9d9b-5c5bc3a4905d", payExtra: 10000, comment:'không hành', orderDetail: [
    {foodId: '', foodName: 'Chicken Rice', img:'', price: 30000},
    {foodId: '', foodName: 'Coka', img:'', price: 10000},
    {foodId: '', foodName: 'Draft beer', img:'', price: 20000},
  ]},
  {userId:"e618f708-8dde-4f04-9d9b-5c5bc3a4905d", payExtra: 10000, comment:'không hành', orderDetail: [
    {foodId: '', foodName: 'Chicken Rice', img:'', price: 30000},
    {foodId: '', foodName: 'Coka', img:'', price: 10000},
    {foodId: '', foodName: 'Draft beer', img:'', price: 20000},
  ]}
]
 
 
@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less'],
})
export class EventSummaryDialogComponent implements OnInit {
 
  @ViewChild('personGroupView', {static: false}) userGroupTab: ElementRef;

  constructor(private router: Router, private restaurantService:RestaurantService) {
    console.log(router.routerState);
  }
 
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);
 
  dishGroupViewDisplayedColumns: string[] = ['picture', 'name', 'amount', 'price', 'total', 'comment'];
  personGroupViewDisplayedColumns: string[] = ['user', 'food', 'price', 'pay-extra', 'comment'];
 
  restaurant: any;
 
  eventDetail: EventList;
 
  orderByDish: any[] = [
    {foodId: '', picture: 'https://images.foody.vn/res/g1/595/prof/s60x60/foody-upload-api-foody-mobile-10-jpg-180508140146.jpg', name: 'Coka', amount: 4, price: 10000, total:40000, comment: '2x không gas, 1x không đường' },
    {foodId: '', picture: 'https://images.foody.vn/res/g1/595/prof/s60x60/foody-upload-api-foody-mobile-10-jpg-180508140146.jpg', name: 'Coka', amount: 4, price: 10000, total:40000, comment: '2x không gas, 1x không đường' },
  ];
 
  orderByPerson: any[] = [
    {user: 'admin', food: '1xChicken rice + 1x coca', price: 40000, payExtra: 5000, comment: 'không hành' },
    {user: 'admin', food: '1xChicken rice + 1x coca', price: 40000, payExtra: 5000, comment: 'không hành' },
    {user: 'admin', food: '1xChicken rice + 1x coca', price: 40000, payExtra: 5000, comment: 'không hành' },
  ];
  
  dishGroupView() {
    database.map(order => {
      order.orderDetail.map(detail => {
        if (!this.orderByDish.includes(order => order.foodId == detail.foodId)) {
          // this.orderData.push()
        }
      })
    })
  }
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }
 
  toPdf() {
    window['html2canvas'] = html2canvas;
    // var report = new jsPDF('p', 'pt', 'a4');

  //   report.html(document.getElementById('report'), {
  //     callback: function (doc) {
  //       console.log(doc);
  //       doc.save();
  //     }
  //  })

    console.log(this.userGroupTab)

    const page = document.getElementById('report');
    // const userGroupTab = document.get('personGroupView');
    const options = {background: "white", height: page.clientHeight, width: page.clientWidth, letterRendering: 1};
    const options2 = {background: "white", height: this.userGroupTab.nativeElement.clientHeight, width: this.userGroupTab.nativeElement.clientWidth};

    html2canvas(page, options).then((pageSource) => {
      // html2canvas(userGroupTab, options2).then((userTabSource) => {
        //Initialize JSPDF
        let doc = new jsPDF();
        //Converting canvas to Image
        let pageData = pageSource.toDataURL("image/PNG");
        // let userGroupData = userTabSource.toDataURL("image/PNG")
        // console.log(imgData)
        //Add image Canvas to PDF
        doc.addImage(pageData, 'PNG', 20, 20, 200, 200);
        // doc.addImage(userGroupData, 'PNG', 20, 20, 200, 200);

        let pdfOutput = doc.output();
        // using ArrayBuffer will allow you to put image inside PDF
        let buffer = new ArrayBuffer(pdfOutput.length);
        let array = new Uint8Array(buffer);
        for (let i = 0; i < pdfOutput.length; i++) {
            array[i] = pdfOutput.charCodeAt(i);
        }
        const fileName = "report.pdf";
        doc.save(fileName);
      });
    // })
  }

  ngOnInit() {
    console.log(history.state);
    this.eventDetail = {
      eventTitle: "FIKA2",
      eventId: "11",
      eventRestaurant: "Morico - Contemporary Japanese Lifestyle - Lê Lợi",
      eventMaximumBudget: 20000,
      eventTimeToClose: "2019-09-04T11:35:00+07:00",
      eventTimeToReminder: "2019-09-04T11:35:00+07:00",
      eventHost: "Amie Perigo",
      eventParticipants: "owner123@devpreciovn.onmicrosoft.com;#member@devpreciovn.onmicrosoft.com;#toandh3xco@gmail.com",
      eventCategory: "Café/Dessert",
      eventRestaurantId: "595",
      eventServiceId: '1',
      eventDeliveryId: '',
      eventCreatedUserId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb",
      eventHostId: "4cf3230b-6dd5-4942-a0cd-bcb8db6dc8eb"
    }
 
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
