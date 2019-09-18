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
  displayedColumns = ['avatar', 'Name', 'Email', 'Order Status'];
  EventTime = '';
  EventStatus = '';
  EventType = '';
  _environment = environment.apiUrl;
  ngOnInit() {
    //get user not order
    var self = this;
    console.log('EventId', this.data.EventId);
    var participants = JSON.parse(this.data.EventParticipantsJson);

    let promise = this.orderService
      .GetUserNotOrdered(this.data.EventId)
      .then(result => {
        result.forEach(element => {
          var participant = participants.filter(
            item => item.Id == element.UserId
          );

          if (participant != null) {
            const userOrder: EventUser = {
              Name: participant[0].DisplayName,
              Email: participant[0].Mail,
              Id: participant[0].Id,
              Img: '',
              IsGroup: 0,
              OrderStatus: 'Not order'
            };
            this.eventusers.push(userOrder);
          }
          this.table.renderRows();
        });
      });

    promise.then(function() {
      var p = participants;
      var e = self.eventusers;
      participants.forEach(element => {
        var flag: Boolean = false;

        self.eventusers.forEach(element2 => {
          if (element.Id === element2.Id) {
            flag = true;
          }
        });

        if (flag === false) {
          console.log(element.DisplayName);
          const userOrder: EventUser = {
            Name: element.DisplayName,
            Email: element.Mail,
            Id: element.Id,
            Img: '',
            IsGroup: 0,
            OrderStatus: 'Order'
          };
          self.eventusers.push(userOrder);
          self.table.renderRows();
        }
      });
    });
    this.eventTitle = this.data.Name;
    this.eventHost = this.data.HostName;
    this.eventRestaurant = this.data.Restaurant;
    this.maximumBudget = this.data.MaximumBudget.toString();
    this.dateTimeToClose = this.data.CloseTime.toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    this.dateToReminder = this.data.RemindTime ? this.data.RemindTime.toString()
      .replace('T', ' ')
      .replace('+07:00', '') : '';

    this.EventTime = this.data.EventDate.toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    this.EventStatus = this.data.Status;
    this.EventType = this.data.EventType;
  }
  OnNoClick(): void {
    this.dialogRef.close();
  }
}
