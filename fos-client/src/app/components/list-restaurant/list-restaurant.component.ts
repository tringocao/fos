import { RestaurantService } from './../../services/restaurant/restaurant.service';
import { FavoriteService } from './../../services/favorite/favorite.service';
import { UserService } from './../../services/user/user.service';
import {
  Component,
  OnInit,
  ViewChild,
  OnChanges,
  Input,
  ChangeDetectorRef
} from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from 'src/app/models/user';
import { FavoriteRestaurant } from 'src/app/models/favorite-restaurant';
// import { FavoriteRestaurant } from '../../models/favoriteRestaurant';

const restaurants: any = [];

interface Restaurant {
  id: number;
  stared: boolean;
  restaurant: string;
  category: string;
  address: string;
  promotion: string;
  open: string;
  delivery_id: number;
  url_rewrite_name: string;
  picture: string;
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
  displayedColumns: string[] = [
    'id',
    'picture',
    'restaurant',
    'category',
    'promotion',
    'open',
    'menu',
    'addEvent'
  ];
  dataSource:MatTableDataSource<Restaurant>;
  favoriteOnlyDataSource: Restaurant[];
  baseDataSource: Restaurant[];
  userId: string;
  load: boolean;
  favoriteRestaurants: string[];
  favoriteOnly: boolean;
  topic: string;
  keyword: string;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngOnInit() {
    this.load = true;
    this.categorys = ['a', 'b', 'c', 'd'];
    this.sortNameOrder = 0;
    this.sortCategoryOrder = 0;



    var restaurants : Restaurant[] = [];
    this.dataSource = new MatTableDataSource<Restaurant>(restaurants);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.userId = '';
    this.favoriteRestaurants = [];
    this.favoriteOnly = false;
    this.topic = JSON.parse('[]');
    this.keyword = '';

    this.userService.getCurrentUserId().then((response: User) => {
      console.log(response.Id);
      this.userId = response.Id;
      this.favoriteService.getFavorite(this.userId).then(response => {
        console.log(response);
        response.map((item: FavoriteRestaurant) => {
          this.favoriteRestaurants.push(item.RestaurantId);
        });
        console.log(this.favoriteRestaurants);
      });
    });
    this.getRestaurant({ topic: this.topic, keyword: this.keyword });
  }

  constructor(
    private changeDetectorRefs: ChangeDetectorRef,
    private _snackBar: MatSnackBar,
    private restaurantService: RestaurantService,
    private userService: UserService,
    private favoriteService: FavoriteService
  ) {}

  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }

  addToFavorite(event, restaurantId: string) {
    console.log("add", restaurantId);
    var favoriteRestaurant:FavoriteRestaurant = new FavoriteRestaurant();
    favoriteRestaurant.RestaurantId = restaurantId;
    favoriteRestaurant.UserId = this.userId; //add to remove
    this.favoriteService
      .addFavoriteRestaurant(favoriteRestaurant)
      .then(response => {
        // console.log(this.dataSource.data);
        if (response != null && response.ErrorMessage != null) {
          this.toast('Error happnened ', 'Dismiss');
        } else {
          this.dataSource.data.forEach(data => {
            // console.log(data)
            if (data.id.toString() == restaurantId) {
              data.stared = true;
              this.toast(data.restaurant + ' added! ', 'Dismiss');
            }
          });
        }
      });
  }
  filterByFavorite(event) {
    this.favoriteOnly = event.checked;
    if (this.favoriteOnly) {
      this.favoriteOnlyDataSource = this.dataSource.data.filter(
        restaurant => restaurant.stared
      );
      this.baseDataSource = this.dataSource.data;
      this.dataSource.data = this.favoriteOnlyDataSource;
      this.toast('Filtered by favorite! ', 'Dismiss');
    } else {
      this.dataSource.data = this.baseDataSource;
    }
    // this.getRestaurant({topic: this.topic, keyword: this.keyword});
  }

  removeFromFavorite(event, restaurantId: string) {
    console.log("remove", restaurantId);
    var favoriteRestaurant:FavoriteRestaurant = new FavoriteRestaurant();
    favoriteRestaurant.RestaurantId = restaurantId;
    favoriteRestaurant.UserId = this.userId; //add to remove
    this.favoriteService
      .removeFavoriteRestaurant(favoriteRestaurant)
      .then(response => {
        // console.log(this.dataSource.data);
        if (response != null && response.ErrorMessage != null) {
          this.toast('Error happnened ', 'Dismiss');
        } else {
          this.dataSource.data.forEach(data => {
            // console.log(data)
            if (data.id.toString() == restaurantId) {
              data.stared = false;
              this.toast(data.restaurant + ' removed! ', 'Dismiss');
            }
          });
        }
      });
  }

  getRestaurant($event) {
    if ($event.isChecked) {
      this.favoriteOnlyDataSource = this.dataSource && this.dataSource.data && this.dataSource.data.filter(
        restaurant => restaurant.stared
      );
      this.baseDataSource = this.dataSource.data;
      this.dataSource.data = this.favoriteOnlyDataSource;
      this.toast('Filtered by favorite! ', 'Dismiss');
    } else {
      this.dataSource.data = this.baseDataSource;
    }
    if ($event.topic != undefined && $event.keyword != undefined) {
      this.load = true;

      this.topic = $event.topic;
      this.keyword = $event.keyword;
      this.restaurantService
        .getRestaurantIds($event.topic, $event.keyword)
        .then(response => {
          this.restaurantService.getRestaurants(response).then(result => {
            const dataSourceTemp = result;
            // this.dataSource.data = dataSourceTemp;
            // console.log(
            //   dataSourceTemp.filter(
            //     (restaurant: Restaurant) => restaurant.stared
            //   )
            // );
            // this.dataSource.data = this.favoriteOnly
            //   ? dataSourceTemp.filter(
            //       (restaurant: Restaurant) => restaurant.stared
            //     )
            //   : dataSourceTemp;

            this.dataSource.sort = this.sort;
            this.dataSource.paginator = this.paginator;
            this.load = false;
          });
          this.changeDetectorRefs.detectChanges();
        });
    }
  }
}
