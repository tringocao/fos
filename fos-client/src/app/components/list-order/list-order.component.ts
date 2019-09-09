import { Component, OnInit, ViewChild, Input, OnChanges } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { OrderService } from './../../services/order/order.service';
import { FormControl } from '@angular/forms';
import { UserService } from './../../services/user/user.service';
import * as moment from 'moment';
import 'moment/locale/vi';
import { EventDialogComponent } from '../event-dialog/event-dialog.component';
import { EventSummaryDialogComponent } from '../event-summary-dialog/event-summary-dialog.component';
import { Overlay } from '@angular/cdk/overlay';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import Event from './../../models/event';
import { EventDialogViewComponent } from './../event-dialog-view/event-dialog-view.component';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';

moment.locale('vi');

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.less']
})
export class ListOrderComponent implements OnInit, OnChanges {
  displayedColumns: string[] = [
    'name',
    'restaurant',
    'category',
    'date',
    'participants',
    'maximumBudget',
    'status',
    'host'
  ];
  dataSource: any = new MatTableDataSource([]);
  isLoading = true;
  currency = 'VND';
  userId: any;
  allOrder: Event[];
  myOrder: Event[];
  myOrderCategories = [];
  allOrderCategories = [];

  categories = new FormControl();
  categoryList = [];
  searchQuery = '';
  categorySelected = null;

  eventListItem: EventList;

  @Input() isMyOrder: any;

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private orderService: OrderService,
    private userService: UserService,
    public dialog: MatDialog,
    private router: Router,
    overlay: Overlay
  ) {}

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userService.getCurrentUserId().then((response: User) => {
      this.userId = response.id;
      this.getOrders();
    });
  }

  ngOnChanges() {
    this.categoryList = [];
    if (!this.isMyOrder) {
      if (!this.displayedColumns.includes('host')) {
        this.displayedColumns.pop();
        this.displayedColumns.push('host');
        this.displayedColumns.push('status');
      }
      this.setDataSource(this.allOrder);
      this.allOrderCategories.forEach(item => {
        this.categoryList.push(item);
      });
    } else {
      if (this.displayedColumns.includes('host')) {
        this.displayedColumns.pop();
        this.displayedColumns.pop();
        this.displayedColumns.push('status');
      }
      this.setDataSource(this.myOrder);
      this.myOrderCategories.forEach(item => {
        this.categoryList.push(item);
      });
    }
    this.searchQuery = '';
    this.categorySelected = null;
  }

  setDataSource(data: any) {
    this.dataSource = new MatTableDataSource(data);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  getOrders() {
    this.orderService.getAllOrder().subscribe(response => {
      this.allOrder = this.orderService.mapResponseDataToEvent(response.Data);
      console.log('all order: ', this.allOrder);
      this.myOrder = this.allOrder.filter(item => {
        return item.createdBy === this.userId || item.hostId === this.userId;
      });
      this.getCateroriesFromOrders(this.myOrder, true);
      this.getCateroriesFromOrders(this.allOrder, false);
      this.setDataSource(this.myOrder);
      this.categoryList = this.myOrderCategories;
      this.isLoading = false;
    });
  }

  getCateroriesFromOrders(orders: Event[], isMyOrder: boolean) {
    const categories = [];
    orders.forEach(element => {
      if (element.category !== null && element.category.length > 0) {
        categories.push(element.category);
      }
    });
    console.log('category ', categories);
    const myCategories = [...new Set(categories)].filter(
      arrayItem => arrayItem !== undefined
    );
    if (isMyOrder) {
      this.myOrderCategories = myCategories;
    } else {
      this.allOrderCategories = myCategories;
    }
  }

  async categoryChange(event: any) {
    await this.sleep(500);
    this.filterBoth();
    this.dataSource.filter = event.value;
    if (this.categorySelected === null && this.searchQuery !== '') {
      this.dataSource.filter = this.searchQuery;
    }
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  filterBoth() {
    if (this.searchQuery !== '' && this.categorySelected !== null) {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return (
          dataFilter.category === this.categorySelected &&
          (dataFilter.restaurant
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1 ||
            dataFilter.name
              .toLowerCase()
              .indexOf(this.searchQuery.toLowerCase()) > -1)
        );
      };
    } else if (this.searchQuery !== '' && this.categorySelected === null) {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return (
          dataFilter.restaurant
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1 ||
          dataFilter.name
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1
        );
      };
    } else if (this.categorySelected !== null && this.searchQuery === '') {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return dataFilter.category === this.categorySelected;
      };
    }
  }

  async onSearchChange(event: any) {
    await this.sleep(500);
    this.filterBoth();
    this.dataSource.filter = event;
    if (this.searchQuery === '' && this.categorySelected !== null) {
      this.dataSource.filter = this.categorySelected;
    }
  }

  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }

  private toDateString(date: Date): string {
    return (
      date.getFullYear().toString() +
      '-' +
      ('0' + (date.getMonth() + 1)).slice(-2) +
      '-' +
      ('0' + date.getDate()).slice(-2) +
      'T' +
      date.toTimeString().slice(0, 5)
    );
  }

  showEvent(row: any) {
    // console.log(row);
    this.mapRowToEventList(row);

    const dialogRef = this.dialog.open(EventDialogViewComponent, {
      maxHeight: '98vh',
      width: '80%',
      data: this.eventListItem
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  mapRowToEventList(row:any) {
    this.eventListItem = {
      eventTitle: row.name,
      eventId: row.eventId,
      eventRestaurant: row.restaurant,
      eventMaximumBudget: row.maximumBudget,
      eventTimeToClose: row.date,
      eventTimeToReminder: row.timeToRemind,
      eventHost: row.hostName,
      eventParticipants: row.participants,
      eventCategory: row.category,
      eventRestaurantId: row.restaurantId,
      eventServiceId: '1',
      eventDeliveryId: '',
      eventCreatedUserId: this.userId,
      eventHostId: row.hostId
    };
  }

  remind(event: any, element: any) {
    event.stopPropagation();
  }

  close(row:any) {
    // const eventSummaryDialog = this.dialog.open(EventSummaryDialogComponent, {
    //   maxHeight: '98vh',
    //   width: '80%',
    //   data: this.eventListItem
    // });
    // this.router.navigate([]).then(result => { window.open('/summary', '_blank'); });
    // this.mapRowToEventList(row);
    // this.router.navigate(['/summary'], {state: {data: this.eventListItem}})
    // eventSummaryDialog.afterClosed().subscribe(result => {
    //   console.log('The dialog was closed');
      
    // });
    // event.stopPropagation();
  }

  getNumberOfParticipant(participants: string) {
    return participants.split(';#').length;
  }
}
