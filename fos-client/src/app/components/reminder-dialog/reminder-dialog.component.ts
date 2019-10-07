import { Component, OnInit, Inject, ViewChild, Input } from '@angular/core';
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
import { SummaryService } from 'src/app/services/summary/summary.service';
import { OverlayContainer } from '@angular/cdk/overlay';
import { reject } from 'q';
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';

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

  @Input() header;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    public dialogRef: MatDialogRef<ReminderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data,
    private orderService: OrderService,
    private snackBar: MatSnackBar,
    private summaryService: SummaryService,
    private overlayContainer: OverlayContainer
    ,
    private dataRouting: DataRoutingService
 ) {
      this.getNavTitleSubscription = this.dataRouting.getNavTitle()
   .subscribe((appTheme: string) => this.appTheme = appTheme);
   overlayContainer.getContainerElement().classList.add("app-"+this.appTheme+"-theme");
 }
 ngOnDestroy() {
   // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
   this.getNavTitleSubscription.unsubscribe();
 }
 private getNavTitleSubscription: Subscription;
 appTheme: string;
  ngOnInit() {
    //console.log(this.data.isClosedEvent);
    this.orderService
      .GetUserNotOrdered(this.data.event.EventId)
      .then(result => {
        this.userNotOrder = result;
        this.graphUserNotOrder = [];
        result.forEach(element => {
          const participants: GraphUser[] = JSON.parse(
            this.data.event.EventParticipantsJson
          );
          const participant = participants.filter(
            item => item.Id === element.UserId
          );
          //console.log('participant: ', participant);
          this.graphUserNotOrder.push(...participant);
        });
        this.dataSource = new MatTableDataSource(this.graphUserNotOrder);
        this.dataSource.paginator = this.paginator;
      })
      .catch(error => this.toast(error, 'Dismiss'));
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
    this.closeDialog($event);
    const info: UserNotOrderMailInfo[] = [];
    this.graphUserNotOrder.forEach(item => {
      const element = new UserNotOrderMailInfo();
      element.EventRestaurant = this.data.event.Restaurant;
      element.EventTitle = this.data.event.Name;
      element.UserMail = item.Mail;
      element.UserName = item.DisplayName;
      element.OrderId = this.userNotOrder.filter(
        user => user.UserId === item.Id
      )[0].OrderId;

      info.push(element);
    });
    this.orderService
      .SendEmailToNotOrderedUser(info)
      .then(response => {
        if (response === null) {
          this.toast('Reminder success', 'Dismiss');
        }
      })
      .catch(error => this.toast(error, 'Dismiss'));
  }

  closeEvent($event) {
    this.summaryService
      .updateEventStatus(this.data.event.EventId, 'Closed')
      .then(response => {
        if (response === null) {
          this.toast('Event Closed', 'Dismiss');
          window.location.reload();
          this.closeDialog($event);
        }
        if (response != null && response.ErrorMessage != null) {
          this.toast('Close event failed', 'Dismiss');
        }
      });
  }
}
