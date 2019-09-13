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
@Component({
  selector: 'app-event-dialog-view',
  templateUrl: './event-dialog-view.component.html',
  styleUrls: ['./event-dialog-view.component.less']
})
export class EventDialogViewComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  eventusers: EventUser[] = [  ];
  constructor(@Inject(MAT_DIALOG_DATA) public data: Event,
  public dialogRef: MatDialogRef<EventDialogViewComponent>,
  private orderService: OrderService) {}
  eventTitle = '';
  eventHost = '';
  eventRestaurant = '';
  maximumBudget = '';
  dateTimeToClose = '';
  dateToReminder = '';
  displayedColumns = ['avatar', 'Name', 'Email', 'Order Status'];
  EventTime = '';
  EventStatus ='';
  EventType = '';
  ngOnInit() {
    //get user not order
    var self = this;
    console.log('EventId', this.data.EventId);
    var participants = JSON.parse(this.data.EventParticipantsJson);
    var userNotOrder = [];


    // var userOrder: EventUser = {
    //   Name: 'abc',
    //   Email: 'mail',
    //   Id: 'id1',
    //   Img: '',
    //   IsGroup: 0,
    //   OrderStatus: 'not order'
    // }
    // self.eventusers.push(userOrder);

    // let array = [1,2,3];
    // for (let i = 0; i < array.length; i++) {
    //   const userOrder: EventUser = {
    //     Name: 'abc',
    //     Email: 'mail',
    //     Id: 'id1',
    //     Img: '',
    //     IsGroup: 0,
    //     OrderStatus: 'not order'
    //   }
    //   self.eventusers.push(userOrder);
    // }


    this.orderService.GetUserNotOrdered(this.data.EventId).then(
      result => {
        result.forEach(element => {
          // console.log(element);
          // const participants: GraphUser[] = JSON.parse(
          //   this.data.EventParticipantsJson
          // );

          // const participant = participants.filter(
          //   item => item.id === element.UserId
          // );
          // console.log('participant: ', participant);

          const userOrder: EventUser = {
            Name: element.UserId,
            Email:'',
            Id: 'id1',
            Img: '',
            IsGroup: 0,
            OrderStatus: 'not order'

          }
          this.eventusers.push(userOrder);
          this.table.renderRows();
        });
      }
    )
    
    // setTimeout(function(){
    //   // console.log(userNotOrder);
    //   participants.map(
    //     value =>{
    //       userNotOrder.map(
    //         NotOrder =>{
    //           // console.log('not order', NotOrder);
    //           if(value.id === NotOrder.UserId){
    //             var userOrder: EventUser = {
    //               Name: 'abc',
    //               Email: 'mail',
    //               Id: 'id1',
    //               Img: '',
    //               IsGroup: 0,
    //               OrderStatus: 'not order'
    //             }
    //             self.eventusers.push(userOrder);
    //           }
    //         }
    //       )
    //     }
    //   )

    // },2000)




    
    // console.log(self.eventusers);
    this.eventTitle = this.data.Name;
    this.eventHost = this.data.HostName;
    this.eventRestaurant = this.data.Restaurant;
    this.maximumBudget = this.data.MaximumBudget.toString();
    this.dateTimeToClose = this.data.CloseTime
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    this.dateToReminder = this.data.RemindTime
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');

      this.EventTime = this.data.EventDate
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
      this.EventStatus = this.data.Status;
      this.EventType = this.data.EventType;

    
    // participants.map(value => {
    //   console.log('paricipant ', value);
    //   this.eventusers.push({
    //     Id: value.id,
    //     Name: value.displayName,
    //     Img: '',
    //     Email: value.mail,
    //     IsGroup: 0,
    //     OrderStatus: 'not order'
    //   });
    // })

    
  }
  OnNoClick(): void {
    this.dialogRef.close();
  }
}
