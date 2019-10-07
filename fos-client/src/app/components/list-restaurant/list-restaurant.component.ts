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
import { FavoriteRestaurant } from 'src/app/models/favorite-restaurant';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { User } from 'src/app/models/user';
import { Restaurant } from 'src/app/models/restaurant';
import * as moment from 'moment';

moment.locale('vi');

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
    'Photo',
    'Name',
    'Categories',
    'PromotionGroups',
    'Operating',
    'menu',
    'addEvent'
  ];
  dataSource: any = new MatTableDataSource<DeliveryInfos>([]);
  favoriteOnlyDataSource: any = new MatTableDataSource<DeliveryInfos>([]);
  baseDataSource: any = new MatTableDataSource<DeliveryInfos>([]);
  load: boolean;
  favoriteRestaurants: string[];
  favoriteOnly: boolean;
  topic: string;
  keyword: string;
  imageStyle: string;
  @Input() idService: number;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngOnInit() {
    this.dataSource = new MatTableDataSource();
    this.load = true;
    this.categorys = ['a', 'b', 'c', 'd'];
    this.sortNameOrder = 0;
    this.sortCategoryOrder = 0;
    this.imageStyle = "width:68px;height:68px;";

    var restaurants: DeliveryInfos[] = [];
    this.dataSource = new MatTableDataSource<DeliveryInfos>(restaurants);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.favoriteRestaurants = [];
    this.favoriteOnly = false;
    this.topic = JSON.parse('[]');
    this.keyword = '';

    // this.getRestaurant({ topic: this.topic, keyword: this.keyword });

    // this.userService.getCurrentUserId().then((response: User) => {
    this.favoriteService
      .getFavorite()
      .then(response => {
        console.log(response);
        response.map((item: FavoriteRestaurant) => {
          this.favoriteRestaurants.push(item.RestaurantId);
        });
        console.log(this.favoriteRestaurants);
      })
      .then(() => {
        this.getRestaurant({ topic: this.topic, keyword: this.keyword });
      });
    // })
  }

  constructor(
    // private changeDetectorRefs: ChangeDetectorRef,
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
    console.log('add', restaurantId);
    this.dataSource.data.forEach(data => {
      console.log(data);
      if (data.RestaurantId == restaurantId) {
        data.IsFavorite = true;
        this.toast(data.Name + ' added! ', 'Dismiss');
      }
    });
    this.favoriteRestaurants.push(restaurantId);
    this.favoriteService.addFavoriteRestaurant(restaurantId).then(response => {
      // console.log(this.dataSource.data);
      if (response != null && response.ErrorMessage != null) {
        this.toast('Error happnened ', 'Dismiss');
      }
    });
  }
  // filterByFavorite(event) {
  //   this.favoriteOnly = event.checked;
  //   if (this.favoriteOnly) {
  //     this.favoriteOnlyDataSource = this.dataSource.data.filter(
  //       restaurant => restaurant.IsFavorite
  //     );
  //     this.baseDataSource = this.dataSource.data;
  //     this.dataSource.data = this.favoriteOnlyDataSource;
  //     this.toast("Filtered by favorite! ", "Dismiss");
  //   } else {
  //     this.dataSource.data = this.baseDataSource;
  //   }
  //   this.getRestaurant({topic: this.topic, keyword: this.keyword});
  // }

  removeFromFavorite(event, restaurantId: string) {
    console.log('remove', this.dataSource.data);
    this.dataSource.data.forEach(data => {
      if (data.RestaurantId == restaurantId) {
        data.IsFavorite = false;
        this.toast(data.Name + ' removed! ', 'Dismiss');
      }
    });
    this.favoriteRestaurants = this.favoriteRestaurants.filter(id => id !== restaurantId);
    this.favoriteService
      .removeFavoriteRestaurant(restaurantId)
      .then(response => {
        // console.log(this.dataSource.data);
        if (response != null && response.ErrorMessage != null) {
          this.toast('Error happnened ', 'Dismiss');
        }
      });
  }

  getRestaurant($event) {
    if ($event.isChecked) {
      this.baseDataSource.data = this.dataSource.data;
      this.restaurantService.getRestaurants(this.favoriteRestaurants.map(Number), this.idService, 217).then(result => {
        console.log(result)
        result.forEach(item => item.IsFavorite = true);
        this.dataSource.data = result;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.toast('Filtered by favorite! ', 'Dismiss');
      })
    } else {
      this.dataSource.data = this.baseDataSource.data.map(restaurant => {
        if (this.favoriteRestaurants.includes(restaurant.RestaurantId)) {
          restaurant.IsFavorite = true;
        }
        else {
          restaurant.IsFavorite = false;
        }
        return restaurant;
      });
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
    console.log($event.isChecked);
    if ($event.topic != undefined && $event.keyword != undefined) {
      this.load = true;

      this.topic = $event.topic;
      this.keyword = $event.keyword;
      this.restaurantService
        .getRestaurantIds($event.topic, $event.keyword, this.idService, 217)
        .then(response => {
          this.restaurantService
            .getRestaurants(response, this.idService, 217)
            .then(result => {
              var dataSourceTemp = [];
              dataSourceTemp = result.map(item => {
                if (this.favoriteRestaurants.includes(item.RestaurantId)) {
                  item.IsFavorite = true;
                }
                return item;
              });
              this.dataSource.data = dataSourceTemp;
              this.dataSource.sort = this.sort;
              this.dataSource.paginator = this.paginator;
              this.load = false;
            });
          // this.changeDetectorRefs.detectChanges();
        });
    }
  }
  formatTime(operating: string) {
    if (operating.length > 1) {
      const time = operating.split('-');
      const open = moment(time[0], 'HH:mm:ss').format('HH:mm');
      const close = moment(time[1], 'HH:mm:ss').format('HH:mm');
      return open + ' - ' + close;
    } else {
      return '-';
    }
  }
}
