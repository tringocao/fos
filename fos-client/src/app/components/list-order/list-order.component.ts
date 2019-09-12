import { Component, OnInit, ViewChild, Input, OnChanges } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { OrderService } from './../../services/order/order.service';
import { FormControl } from '@angular/forms';
import { UserService } from './../../services/user/user.service';
import * as moment from 'moment';
import 'moment/locale/vi';
import { MatDialog } from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import { Event } from './../../models/event';
import { EventDialogViewComponent } from './../event-dialog-view/event-dialog-view.component';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { MatSelectChange } from '@angular/material/select';
import { Overlay } from '@angular/cdk/overlay';

moment.locale('vi');

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.less']
})
export class ListOrderComponent implements OnInit, OnChanges {
  displayedColumns: string[] = [
    'Name',
    'Restaurant',
    'Category',
    'Participants',
    'CloseTime',
    'MaximumBudget',
    'Status',
    'HostName'
  ];
  dataSource: MatTableDataSource<Event>;
  isLoading = true;
  currency = 'VND';
  userId: string;
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
    this.dataSource = new MatTableDataSource<Event>([]);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userService.getCurrentUserId().then((response: User) => {
      this.userId = response.Id;
      this.getAllEvent();
    });
  }

  ngOnChanges() {
    this.categoryList = [];
    if (!this.isMyOrder) {
      if (!this.displayedColumns.includes('HostName')) {
        this.displayedColumns.pop();
        this.displayedColumns.push('HostName');
        this.displayedColumns.push('Status');
      }
      this.setDataSource(this.allOrder);
      this.allOrderCategories.forEach(item => {
        this.categoryList.push(item);
      });
    } else {
      if (this.displayedColumns.includes('HostName')) {
        this.displayedColumns.pop();
        this.displayedColumns.pop();
        this.displayedColumns.push('Status');
      }
      this.setDataSource(this.myOrder);
      this.myOrderCategories.forEach(item => {
        this.categoryList.push(item);
      });
    }
    this.searchQuery = '';
    this.categorySelected = null;
  }

  setDataSource(data: Event[]) {
    this.dataSource = new MatTableDataSource(data);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  getAllEvent() {
    this.orderService.getAllEvent(this.userId).then(response => {
      this.allOrder = response;
      this.myOrder = this.allOrder.filter(item => {
        return item.IsMyEvent === true;
      });

      this.getCateroriesFromOrders(this.myOrder, true);
      this.getCateroriesFromOrders(this.allOrder, false);

      this.sortEventByDateAndStatus(this.myOrder, true);
      this.sortEventByDateAndStatus(this.allOrder, false);

      this.setDataSource(this.myOrder);
      this.categoryList = this.myOrderCategories;
      this.isLoading = false;
    });
  }

  sortEventByDateAndStatus(events: Event[], isMyOrder: boolean) {
    const eventOpen: Event[] = [];
    const eventError: Event[] = [];
    const eventClose: Event[] = [];
    events.forEach(item => {
      if (item.Status === 'Opened') {
        eventOpen.push(item);
      } else if (item.Status === 'Closed') {
        eventClose.push(item);
      } else {
        eventError.push(item);
      }
    });
    events = [];

    const openEventSorted = eventOpen.sort(this.sortDateAsc);
    const closeEventSorted = eventClose.sort(this.sortDateAsc);
    const errorEventSorted = eventError.sort(this.sortDateAsc);
    events.push(...openEventSorted);
    events.push(...errorEventSorted);
    events.push(...closeEventSorted);

    if (isMyOrder) {
      this.myOrder = events;
    } else {
      this.allOrder = events;
    }
  }

  sortDateAsc = (first: Event, second: Event) => {
    const firstDate = new Date(first.CloseTime);
    const secondDate = new Date(second.CloseTime);
    return Number(firstDate.getTime()) - Number(secondDate.getTime());
    // tslint:disable-next-line:semicolon
  };

  sortDateDesc = (first: Event, second: Event) => {
    const firstDate = new Date(first.CloseTime);
    const secondDate = new Date(second.CloseTime);
    return Number(secondDate.getTime()) - Number(firstDate.getTime());
    // tslint:disable-next-line:semicolon
  };

  getCateroriesFromOrders(orders: Event[], isMyOrder: boolean) {
    const categories = [];
    orders.forEach(element => {
      if (element.Category !== null && element.Category.length > 0) {
        categories.push(element.Category);
      }
    });
    const myCategories = [...new Set(categories)].filter(
      arrayItem => arrayItem !== undefined
    );
    if (isMyOrder) {
      this.myOrderCategories = myCategories;
    } else {
      this.allOrderCategories = myCategories;
    }
  }

  async categoryChange(event: MatSelectChange) {
    await this.sleep(500);
    this.filterBoth();
    this.dataSource.filter = event.value;
    if (this.categorySelected === null && this.searchQuery !== '') {
      this.dataSource.filter = this.searchQuery;
    }
  }

  sleep(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  filterBoth() {
    if (this.searchQuery !== '' && this.categorySelected !== null) {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return (
          dataFilter.Category === this.categorySelected &&
          (dataFilter.Restaurant.toLowerCase().indexOf(
            this.searchQuery.toLowerCase()
          ) > -1 ||
            dataFilter.Name.toLowerCase().indexOf(
              this.searchQuery.toLowerCase()
            ) > -1)
        );
      };
    } else if (this.searchQuery !== '' && this.categorySelected === null) {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return (
          dataFilter.Restaurant.toLowerCase().indexOf(
            this.searchQuery.toLowerCase()
          ) > -1 ||
          dataFilter.Name.toLowerCase().indexOf(
            this.searchQuery.toLowerCase()
          ) > -1
        );
      };
    } else if (this.categorySelected !== null && this.searchQuery === '') {
      this.dataSource.filterPredicate = (dataFilter: Event, filter: string) => {
        return dataFilter.Category === this.categorySelected;
      };
    }
  }

  async onSearchChange(event: string) {
    await this.sleep(500);
    this.filterBoth();
    this.dataSource.filter = event;
    if (this.searchQuery === '' && this.categorySelected !== null) {
      this.dataSource.filter = this.categorySelected;
    }
  }

  toStandardDate(date: number) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }

  getNumberOfParticipant(participants: string) {
    return participants.split(';#').length;
  }

  formatCurrency(value: string) {
    return (
      Number(value)
        .toFixed(0)
        .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') + ' '
    );
  }
}
