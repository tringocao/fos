import { Component, OnInit, ViewChild, Input, OnChanges } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { OrderService } from './../../services/order/order.service';
import { FormControl } from '@angular/forms';
import { RestaurantService } from './../../services/restaurant/restaurant.service';
import * as moment from 'moment';
import 'moment/locale/vi';

moment.locale('vi');

interface Order {
  restaurant: string;
  category: string;
  date: Date;
  participants: number;
  maximumBudget: number;
}

const myOrders = [
  {
    restaurant: 'string',
    category: 'string',
    date: new Date(),
    participants: 23,
    maximumBudget: 123123
  },
  {
    restaurant: 'string1',
    category: 'string1',
    date: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),
    participants: 23,
    maximumBudget: 123
  },
  {
    restaurant: 'string2',
    category: 'string2',
    date: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),
    participants: 23,
    maximumBudget: 12312
  },
  {
    restaurant: 'string3',
    category: 'string3',
    date: new Date(),
    participants: 23,
    maximumBudget: 123
  },
  {
    restaurant: 'string6',
    category: 'string6',
    date: new Date(),
    participants: 23,
    maximumBudget: 123213123
  }
];
const allOrders = [
  {
    restaurant: 'string',
    category: 'string',
    date: new Date(),
    participants: 23,
    maximumBudget: 123123
  },
  {
    restaurant: 'string1',
    category: 'string1',
    date: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),
    participants: 23,
    maximumBudget: 123
  }
];

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.less']
})
export class ListOrderComponent implements OnInit, OnChanges {
  displayedColumns: string[] = [
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

  categories = new FormControl();
  categoryList: string[] = [
    'Extra cheese',
    'Mushroom',
    'Onion',
    'Pepperoni',
    'Sausage',
    'Tomato'
  ];

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
    this.getOrders(true);
  }

  ngOnChanges() {
    console.log(this.isMyOrder);
    if (!this.isMyOrder) {
      if (!this.displayedColumns.includes('host')) {
        this.displayedColumns.push('host');
      }
    } else {
      if (this.displayedColumns.includes('host')) {
        this.displayedColumns.pop();
      }
    }
    console.log(this.displayedColumns);
    this.getOrders(false);
  }

  getOrders(isOnInit: boolean) {
    this.dataSource = [];
    this.orderService.getAllOrder().subscribe(response => {
      this.mapResponseToOders(response.value);
      if (this.isMyOrder) {
        this.restaurantService.getCurrentUserId().subscribe(value => {
          this.userId = value.id;
        });
        const myOrder = response.value.filter(
          item => item.createdBy.user.id === this.userId
        );
        this.mapResponseToOders(myOrder);
      }
      this.dataSource = new MatTableDataSource(this.allOrder);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });
  }

  mapResponseToOders(response: any) {
    const result = [];
    response.forEach(element => {
      const order: Order = {
        category: '',
        date: element.fields.EventTimeToClose,
        maximumBudget: element.fields.EventMaximumBudget,
        participants: element.fields.EventParticipants,
        restaurant: element.fields.EventRestaurant
      };
      result.push(order);
    });
    this.allOrder = result;
  }
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:MM');
  }
}
