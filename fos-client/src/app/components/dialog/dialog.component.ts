import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Observable, Observer } from 'rxjs';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { EventDialogComponent } from '../event-dialog/event-dialog.component';

interface FoodCategory {
  dish_type_name: string;
  dish_type_id:string;
  dishes: Food[];
}
interface Food {
  id: string;
  name: string;
  photos: string;
  description: string;
  price: string;
}

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
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.less']
})
export class DialogComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  foodCategory: FoodCategory[]= [];
  load = true;
  sortNameOrder: number;
  sortCategoryOrder: number;
  categorys: any;
  displayedColumns2: string[] = ['picture','name', 'description', 'price'];
  dataSource2: any;
  userId: string;
  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    private restaurantService: RestaurantService,
    @Inject(MAT_DIALOG_DATA) public data: Restaurant,
    public dialog: MatDialog) { }

    async ngOnInit(): Promise<void> {
      console.log("-------------------------------------")

      this.restaurantService.getFood(this.data.delivery_id).subscribe(result => {
        const dataSourceTemp = [];
        const jsonData = JSON.parse(result);
        jsonData.forEach((element, index) => {
          // tslint:disable-next-line:prefer-const
          let dataSourceTemp2 = [];
          element.dishes.forEach(e => {
            let Category: Food = {
              name: e.name,
              id: e.id,
              photos: e.photos,
              description: e.description,
              price:e.price.text
            };
            dataSourceTemp2.push(Category);
          });
          let categoriesItem: FoodCategory = {
              dish_type_name: element.dish_type_name,
              dish_type_id: element.dish_type_id,
              dishes: dataSourceTemp2,
          };
          this.foodCategory.push(categoriesItem);
        });
        this.showAll(this.foodCategory);
      });
  }
  showAll(dataSourceTemp:any[]){
    console.log(dataSourceTemp);
    this.dataSource2 = new MatTableDataSource();
    
    dataSourceTemp.forEach(f =>  {
      if(f.dishes != null){
        f.dishes.forEach(d => this.dataSource2.data.push(d));
      }
    });
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
  updateTable(dataSourceTemp:any[]){
    console.log(dataSourceTemp);
    this.dataSource2 = new MatTableDataSource(dataSourceTemp);
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
  setFood($event) {
    if($event.topic == 0) this.showAll(this.foodCategory);
    else this.updateTable(this.foodCategory[$event.topic].dishes);
    this.load = false;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(EventDialogComponent, {
      // scrollStrategy: this.overlay.scrollStrategies.noop(),
      // autoFocus: false,
      maxHeight: '98vh',
      width: '80%',
      data: this.data
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
  


  onNoClick(): void {
    this.dialogRef.close();
  }

}
