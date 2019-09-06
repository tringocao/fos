import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef  } from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';

const ORDER_DATA: any[] = [
  {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
  {position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li'},
  {position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  {position: 5, name: 'Boron', weight: 10.811, symbol: 'B'},
  {position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C'},
  {position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  {position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O'},
  {position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F'},
  {position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne'},
];


@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less'],
//   providers: [
//     { provide: MAT_DIALOG_DATA, useValue: {} },
//     { provide: MatDialogRef, useValue: {} }
// ]
})
export class EventSummaryDialogComponent implements OnInit {

  constructor(private router: Router, private restaurantService:RestaurantService) {
    console.log(router.routerState);
  }

  dishGroupViewdataSource: any = new MatTableDataSource([]);

  dishGroupViewDisplayedColumns: string[] = ['picture', 'name', 'amount', 'price', 'total', 'comment'];

  restaurant: any;

  eventDetail: EventList;

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

    this.dishGroupViewdataSource = ORDER_DATA;
    // console.log('ass', this.data)
    // this.eventDetail = this.data;
  }

}
