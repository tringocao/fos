import { RestaurantService } from './../../services/restaurant/restaurant.service';
import { Component, OnInit, ViewChild, OnChanges, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

const restaurants: any = [];

interface Restaurant {
  id: string;
  stared: boolean;
  restaurant: string;
  category: string;
  address: string;
  promotion: string;
  open: string;
  delivery_id: string;
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
  displayedColumns: string[] = ['id','restaurant', 'category', 'promotion', 'open', 'menu'];
  dataSource: any = new MatTableDataSource(restaurants);
  userId: string;
  load:boolean;
  favoriteRestaurants: string[];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  

  constructor(private restaurantService: RestaurantService) {}

  addToFavorite(event, restaurantId:string) {
    console.log("add", restaurantId);
    this.restaurantService.addFavoriteRestaurant(this.userId, restaurantId).subscribe(response => {
      console.log(response);
      this.favoriteRestaurants = [];  
      this.restaurantService.getFavorite(this.userId).subscribe(response => {
        console.log(response);
        response.map(item => {
          if (!this.favoriteRestaurants.includes(item)) {
            this.favoriteRestaurants.push(item.RestaurantId);
          }
        })
        console.log( this.favoriteRestaurants);
        this.getRestaurant();
      })
    })
  }

  removeFromFavorite(event, restaurantId:string) {
    console.log("remove", restaurantId);
    this.restaurantService.removeFavoriteRestaurant(this.userId, restaurantId).subscribe(response => {
      console.log(response);
      this.favoriteRestaurants = [];
      this.restaurantService.getFavorite(this.userId).subscribe(response => {
        console.log(response);
        response.map(item => {
          if (!this.favoriteRestaurants.includes(item)) {
            this.favoriteRestaurants.push(item.RestaurantId);
          }
        })
        console.log( this.favoriteRestaurants);
        this.getRestaurant();
      })
    })
  }

  ngOnInit() {
    this.load = true;
    this.categorys = ['a', 'b', 'c', 'd'];
    this.sortNameOrder = 0;
    this.sortCategoryOrder = 0;
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userId = '';
    this.favoriteRestaurants = [];

    this.restaurantService.getCurrentUserId().subscribe(response => {
      console.log(response.id);
      this.userId = response.id;
      // this.restaurantService.addFavoriteRestaurant("fb15617f-0884-448e-9333-f242ad77152f", 47168).subscribe(response => {
      //   console.log(response);
      // })
      // this.restaurantService.removeFavoriteRestaurant(this.userId, 47168).subscribe(response => {
      //   console.log(response);
      // })
      this.restaurantService.getFavorite(this.userId).subscribe(response => {
        console.log(response);
        response.map(item => {
          this.favoriteRestaurants.push(item.RestaurantId);
        })
        console.log( this.favoriteRestaurants);
      })
    });

    this.getRestaurant()
  }

  getRestaurant() {
    this.restaurantService.getRestaurantIds(JSON.parse("[]"), "").subscribe(response => {
      this.restaurantService.getRestaurants(response).subscribe(result => {
        if(result != null && result != ""){
        const jsonData = JSON.parse(result);
        this.dataSource = [];
        const dataSourceTemp = [];
        jsonData.forEach((element, index) => {
          // console.log(element)
          // tslint:disable-next-line:prefer-const
          let restaurantItem: Restaurant = {
            id: element.restaurant_id,
            delivery_id: element.delivery_id,
            stared: this.favoriteRestaurants.includes(element.restaurant_id),
            restaurant: element.name,
            address: element.address,
            category:
              element.categories.length > 0 ? element.categories[0] : '',
            promotion:
              element.promotion_groups.length > 0
                ? element.promotion_groups[0].text
                : '',
            open:
                  (element.operating.open_time || "?") + '-' + (element.operating.close_time || "?"),
              url_rewrite_name:""
          };
          dataSourceTemp.push(restaurantItem);
        });
        console.log(dataSourceTemp);
        this.dataSource = new MatTableDataSource(dataSourceTemp);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.load = false;
      }});
    });

  }

  getRes($event) {
    this.load = true;
    this.restaurantService.getRestaurantIds($event.topic, $event.keyword).subscribe(response => {
      this.restaurantService.getRestaurants(response).subscribe(result => {
        if(result != null && result != ""){
        const jsonData = JSON.parse(result);
        this.dataSource = [];
        const dataSourceTemp = [];
        jsonData.forEach((element, index) => {
          // tslint:disable-next-line:prefer-const
          let restaurantItem: Restaurant = {
            id: element.restaurant_id,
            delivery_id: element.delivery_id,
            stared: this.favoriteRestaurants.includes(element.restaurant_id),
            restaurant: element.name,
            address: element.address,
            category:
              element.categories.length > 0 ? element.categories[0] : '',
            promotion:
              element.promotion_groups.length > 0
                ? element.promotion_groups[0].text
                : '',
            open:
                  (element.operating.open_time || "?") + '-' + (element.operating.close_time || "?"),
              url_rewrite_name:""
          };
          dataSourceTemp.push(restaurantItem);
        });
        this.dataSource = new MatTableDataSource(dataSourceTemp);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.load = false;

      }});
    });

  }
}
