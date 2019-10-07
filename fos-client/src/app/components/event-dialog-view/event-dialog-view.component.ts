import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable
} from '@angular/material';
import { Event } from './../../models/event';
import { EventUser } from 'src/app/models/eventuser';
import { OrderService } from 'src/app/services/order/order.service';
import { from } from 'rxjs';
import { GraphUser } from 'src/app/models/graph-user';
import { element } from 'protractor';
import { environment } from 'src/environments/environment';
import * as moment from 'moment';
import { UserNotOrder } from 'src/app/models/user-not-order';
import { Order } from 'src/app/models/order';

moment.locale('vi');

@Component({
  selector: 'app-event-dialog-view',
  templateUrl: './event-dialog-view.component.html',
  styleUrls: ['./event-dialog-view.component.less']
})
export class EventDialogViewComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  eventusers: EventUser[] = [];
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Event,
    public dialogRef: MatDialogRef<EventDialogViewComponent>,
    private orderService: OrderService
  ) {}
  apiUrl = environment.apiUrl;
  eventTitle = '';
  eventHost = '';
  eventRestaurant = '';
  maximumBudget = '';
  dateTimeToClose = '';
  dateToReminder = '';
  eventTime = '';
  displayedColumns = ['avatar', 'Name', 'Email', 'Order Status'];
  EventTime = '';
  EventStatus = '';
  EventType = '';
  _environment = environment.apiUrl;
  currency = 'đ';
  ngOnInit() {
    //get user not order
    console.log('EventId', this.data.EventId);
    const participants: Array<GraphUser> = JSON.parse(this.data.EventParticipantsJson);

    this.orderService.GetOrdersByEventId(this.data.EventId).then((order: Array<Order>) =>{
      order.forEach(o=>{

        const userNotOrder: GraphUser[] = participants.filter(p =>p.Mail == o.Email);
        if(o.OrderStatus == 0){
          const UserNot: EventUser ={
            Email: userNotOrder[0].Mail,
            Name: userNotOrder[0].DisplayName,
            OrderStatus: 'Not Ordered',
            Img:'',
            IsGroup: 0,
            Id: userNotOrder[0].Id
          }
          this.eventusers.push(UserNot);
        }else if(o.OrderStatus == 1){
          const UserNot: EventUser ={
            Email: userNotOrder[0].Mail,
            Name: userNotOrder[0].DisplayName,
            OrderStatus: 'Ordered',
            Img:'',
            IsGroup: 0,
            Id: userNotOrder[0].Id
          }
          this.eventusers.push(UserNot);
        }else if(o.OrderStatus == 2){
          const UserNot: EventUser ={
            Email: userNotOrder[0].Mail,
            Name: userNotOrder[0].DisplayName,
            OrderStatus: 'Not Participant',
            Img:'',
            IsGroup: 0,
            Id: userNotOrder[0].Id
          }
          this.eventusers.push(UserNot);
        }
      })
      this.table.renderRows();
    })


    // this.orderService.GetUserNotOrdered(this.data.EventId).then((user: Array<UserNotOrder>) =>{
    //   user.forEach( u =>{
    //     const userNotOrder: GraphUser[] = participants.filter(p =>p.Id == u.UserId);
    //     const UserNot: EventUser ={
    //       Email: userNotOrder[0].Mail,
    //       Name: userNotOrder[0].DisplayName,
    //       OrderStatus: 'Not Order',
    //       Img:'',
    //       IsGroup: 0,
    //       Id: userNotOrder[0].Id
    //     }
    //     this.eventusers.push(UserNot);


    //     this.table.renderRows();
    //   })
    // })


    this.eventTitle = this.data.Name;
    this.eventHost = this.data.HostName;
    this.eventRestaurant = this.data.Restaurant;
    this.maximumBudget = this.data.MaximumBudget.toString();
    this.dateTimeToClose = moment(this.data.CloseTime).format(
      'MM/DD/YYYY HH:mm'
    );
    this.dateToReminder = this.data.RemindTime
      ? moment(this.data.RemindTime).format('MM/DD/YYYY HH:mm')
      : ' ';

    this.EventTime = moment(this.data.EventDate).format('MM/DD/YYYY HH:mm');
    this.EventStatus = this.data.Status;
    this.EventType = this.data.EventType;
  }
  OnNoClick(): void {
    this.dialogRef.close();
  }
  formatCurrency(value: string) {
    return Number(value)
      .toFixed(0)
      .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
  }
}
