import { RestaurantService } from './../../services/restaurant/restaurant.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

const restaurants: any = [];

interface Restaurant {
  restaurant: string;
  category: string;
  address: string;
  promotion: string;
  open: string;
  url_rewrite_name: string;
}

@Component({
  selector: 'app-list-restaurant',
  templateUrl: './list-restaurant.component.html',
  styleUrls: ['./list-restaurant.component.less']
})
export class ListRestaurantComponent implements OnInit {
  sortNameOrder: number;
  sortCategoryOrder: number;
  categorys: any;
  displayedColumns: string[] = ['restaurant', 'category', 'promotion', 'open'];
  dataSource: any = new MatTableDataSource(restaurants);

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private restaurantService: RestaurantService) {}

  ngOnInit() {
    this.categorys = ['a', 'b', 'c', 'd'];
    this.sortNameOrder = 0;
    this.sortCategoryOrder = 0;
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

    this.restaurantService.getRestaurantIds(JSON.parse("[]"),"").subscribe(response => {
      this.restaurantService.getRestaurants(response).subscribe(result => {
        const jsonData = JSON.parse(result);
        this.dataSource = [];
        const dataSourceTemp = [];
        jsonData.forEach((element, index) => {
          // tslint:disable-next-line:prefer-const
          let restaurantItem: Restaurant = {
            restaurant: element.name,
            address: element.address,
            category:
              element.categories.length > 0 ? element.categories[0] : '',
            promotion:
              element.promotion_groups.length > 0
                ? element.promotion_groups[0].text
                : '',
            open:
              element.operating.open_time + '-' + element.operating.close_time,
              url_rewrite_name:""
          };
          dataSourceTemp.push(restaurantItem);
        });
        this.dataSource = new MatTableDataSource(dataSourceTemp);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
    });
  }

  getRes($event) {
    this.restaurantService.getRestaurantIds($event.topic, $event.keyword).subscribe(response => {
      this.restaurantService.getRestaurants(response).subscribe(result => {
        const jsonData = JSON.parse(result);
        this.dataSource = [];
        const dataSourceTemp = [];
        jsonData.forEach((element, index) => {
          // tslint:disable-next-line:prefer-const
          let restaurantItem: Restaurant = {
            restaurant: element.name,
            address: element.address,
            category:
              element.categories.length > 0 ? element.categories[0] : '',
            promotion:
              element.promotion_groups.length > 0
                ? element.promotion_groups[0].text
                : '',
            open:
              element.operating.open_time + '-' + element.operating.close_time,
              url_rewrite_name:""
          };
          dataSourceTemp.push(restaurantItem);
        });
        this.dataSource = new MatTableDataSource(dataSourceTemp);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
    });
  }
}
