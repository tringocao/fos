import { Component, OnInit, ViewChild, Input, OnChanges } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { OrderService } from './../../services/order/order.service';
import { FormControl } from '@angular/forms';
import { RestaurantService } from './../../services/restaurant/restaurant.service';
import * as moment from 'moment';
import 'moment/locale/vi';
import { Category } from './../search/search.component';
import Event from '../../models/event';

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
    'host'
  ];
  dataSource: any = new MatTableDataSource([]);
  isLoading = false;
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

  @Input() isMyOrder: any;

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private orderService: OrderService,
    private restaurantService: RestaurantService
  ) {}

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.restaurantService.getCurrentUserId().subscribe(value => {
      this.userId = value.id;
      this.getOrders();
    });
  }

  ngOnChanges() {
    this.categoryList = [];
    if (!this.isMyOrder) {
      if (!this.displayedColumns.includes('host')) {
        this.displayedColumns.push('host');
      }
      this.setDataSource(this.allOrder);
      this.allOrderCategories.forEach(item => {
        this.categoryList.push(item);
      });
    } else {
      if (this.displayedColumns.includes('host')) {
        this.displayedColumns.pop();
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
      this.allOrder = this.orderService.mapResponseDataToEvent(response);
      this.myOrder = this.allOrder.filter(item => {
        return item.createdBy === this.userId || item.hostId === this.userId;
      });
      this.getCateroriesFromOrders(this.myOrder, true);
      this.getCateroriesFromOrders(this.allOrder, false);
      this.setDataSource(this.myOrder);
      this.categoryList = this.myOrderCategories;
    });
  }

  getCateroriesFromOrders(orders: Event[], isMyOrder: boolean) {
    const categories = [];
    orders.forEach(element => {
      categories.push(element.category);
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
    return moment(date).format('DD/MM/YYYY HH:MM');
  }
}
