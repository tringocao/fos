import { Component, Inject, Input } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { NoopScrollStrategy, Overlay } from '@angular/cdk/overlay';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { Observable } from 'rxjs';
import { RestaurantDetail } from 'src/app/models/restaurant-detail';
interface FoodCategory {
  name: string;
  id: string;
  dishes: Food[];
}
interface Food {
  id: string;
  name: string;
  category: string;
  description: string;
  price: string;
}
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
}

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})
export class MenuComponent {
  overlay: Overlay;
  @Input('restaurant') restaurant: Restaurant;
  resDetail: RestaurantDetail;

  constructor(
    public dialog: MatDialog,
    overlay: Overlay,
    private restaurantService: RestaurantService
  ) {
    this.overlay = overlay;
    let restaurantItem: RestaurantDetail = { Rating: 0, TotalReview: 0 };

    this.resDetail = restaurantItem;
  }

  openDialog(): void {
    this.restaurantService
      .getRestaurantDetail(Number(this.restaurant.delivery_id))
      .then(result => {
        this.resDetail.Rating = Number(result.rating.avg);
        this.resDetail.TotalReview = Number(result.rating.total_review);
        const dialogRef = this.dialog.open(DialogComponent, {
          scrollStrategy: this.overlay.scrollStrategies.noop(),
          autoFocus: false,
          maxHeight: '98vh',
          width: '80%',
          data: { restaurant: this.restaurant, detail: this.resDetail }
        });

        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
        });
      });
  }
}
