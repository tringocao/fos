import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { User } from 'src/app/models/user';
import { GraphUser } from 'src/app/models/graph-user';
import { OrderService } from 'src/app/services/order/order.service';
import { Event } from './../../models/event';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { environment } from 'src/environments/environment';
import { UserNotOrderMailInfo } from './../../models/user-not-order-mail-info';
import { UserNotOrder } from 'src/app/models/user-not-order';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reminder-dialog',
  templateUrl: './reminder-dialog.component.html',
  styleUrls: ['./reminder-dialog.component.less']
})
export class ReminderDialogComponent implements OnInit {
  graphUserNotOrder: GraphUser[] = [];
  userNotOrder: UserNotOrder[] = [];
  displayedColumns = ['avatar', 'Name', 'Email'];
  dataSource: MatTableDataSource<GraphUser>;
  apiUrl = environment.apiUrl;
  isReminding = false;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    public dialogRef: MatDialogRef<ReminderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Event,
    private orderService: OrderService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.orderService.GetUserNotOrdered(this.data.EventId).then(result => {
      this.userNotOrder = result;
      this.graphUserNotOrder = [];
      result.forEach(element => {
        const participants: GraphUser[] = JSON.parse(
          this.data.EventParticipantsJson
        );
        const participant = participants.filter(
          item => item.Id === element.UserId
        );
        console.log('participant: ', participant);
        this.graphUserNotOrder.push(...participant);
      });
      this.dataSource = new MatTableDataSource(this.graphUserNotOrder);
      this.dataSource.paginator = this.paginator;
    });
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  closeDialog($event) {
    this.dialogRef.close();
  }

  remind($event) {
    this.isReminding = true;
    const info: UserNotOrderMailInfo[] = [];
    this.graphUserNotOrder.forEach(item => {
      const element = new UserNotOrderMailInfo();
      element.EventRestaurant = this.data.Restaurant;
      element.EventTitle = this.data.Name;
      element.UserMail = item.Mail;
      element.OrderId = this.userNotOrder.filter(
        user => user.UserId === item.Id
      )[0].OrderId;

      info.push(element);
    });
    this.orderService.SendEmailToNotOrderedUser(info).then(response => {
      if (response === null) {
        this.toast('Reminder success', 'Dismiss');
      }
      if (response != null && response.ErrorMessage != null) {
        this.toast('Reminder fail', 'Dismiss');
      }
      this.isReminding = false;
    });
  }
}
