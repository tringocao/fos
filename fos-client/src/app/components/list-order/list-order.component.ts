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

moment.locale('vi');

interface Order {
  restaurant: string;
  category: string;
  date: Date;
  participants: number;
  maximumBudget: number;
  host: number;
  name: string;
}

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
  allOrder = [];
  myOrder = [];
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
    });
    this.getOrders(true);
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

  getOrders(isOnInit: boolean) {
    this.orderService.getAllOrder().subscribe(response => {
      const myOrder = response.value.filter(
        item => item.createdBy.user.id === this.userId
      );
      this.myOrder = this.mapResponseToOders(myOrder);
      this.allOrder = this.mapResponseToOders(response.value);
      this.getCateroriesFromOrders(this.myOrder, true);
      this.getCateroriesFromOrders(this.allOrder, false);
      this.setDataSource(this.myOrder);
      this.categoryList = this.myOrderCategories;
    });
  }

  getCateroriesFromOrders(orders: any, isMyOrder: boolean) {
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

  mapResponseToOders(response: any) {
    const result = [];
    response.forEach(element => {
      const order: Order = {
        name: element.fields.EventTitle,
        category: element.fields.EventCategory,
        date: element.fields.EventTimeToClose,
        maximumBudget: element.fields.EventMaximumBudget,
        participants: element.fields.EventParticipants,
        restaurant: element.fields.EventRestaurant,
        host: element.fields.EventHostLookupId
      };
      result.push(order);
    });
    return result;
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
      this.dataSource.filterPredicate = (dataFilter: Order, filter: string) => {
        return (
          dataFilter.category === this.categorySelected &&
          dataFilter.restaurant
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1
        );
      };
    } else if (this.searchQuery !== '' && this.categorySelected === null) {
      this.dataSource.filterPredicate = (dataFilter: Order, filter: string) => {
        return (
          dataFilter.restaurant
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1
        );
      };
    } else if (this.categorySelected !== null && this.searchQuery === '') {
      this.dataSource.filterPredicate = (dataFilter: Order, filter: string) => {
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
