import {Component, Inject, Input} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { NoopScrollStrategy, Overlay } from '@angular/cdk/overlay';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { Observable } from 'rxjs';
interface FoodCategory {
  name: string;
  id:string;
  dishes: Food[];
}
interface Food {
  id: string;
  name: string;
  category: string;
  description: string;
  price: string;
}
@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})
export class MenuComponent{
  overlay: Overlay;
  foodCategory: FoodCategory[];
  @Input('deliveryId') deliveryId : string;

  constructor(public dialog: MatDialog, overlay: Overlay) {
    this.overlay = overlay;    
  }

  openDialog(): void {

    const dialogRef = this.dialog.open(DialogComponent, {
      scrollStrategy: this.overlay.scrollStrategies.noop(),
      autoFocus: false,
      maxHeight: '90vh',
      data: this.deliveryId 
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
}
