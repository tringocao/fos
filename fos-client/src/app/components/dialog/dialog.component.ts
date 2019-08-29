import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Observable, Observer } from 'rxjs';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
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

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.less']
})
export class DialogComponent implements OnInit {
  // async ngOnInit() {
  //   let dataSourceTemp: FoodCategory[] = [];
  //   console.log(this.data);
  //   dataSourceTemp = await this.restaurantService.getFood(this.data);
  //   console.log(dataSourceTemp)
  //   this.dataSource2.sort = this.sort;
  //   this.dataSource2.paginator = this.paginator;
  //   this.data = this.data + "1";
  //   console.log(this.data);
  //   this.dataSource2 = new MatTableDataSource(dataSourceTemp);  }

    async ngOnInit(): Promise<void> {
      console.log("-------------------------------------")

      this.restaurantService.getFood(this.data).subscribe(result => {
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
        console.log(this.foodCategory);
        this.updateTable(this.foodCategory[0].dishes);
      });
  }
  updateTable(dataSourceTemp:any[]){
    console.log(dataSourceTemp);
    this.dataSource2 = new MatTableDataSource(dataSourceTemp);
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
  setFood($event) {
    this.updateTable(this.foodCategory[$event.topic].dishes)
    this.load = false;
  }


  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  foodCategory: FoodCategory[]= [];
  load = true;
  sortNameOrder: number;
  sortCategoryOrder: number;
  categorys: any;
  displayedColumns2: string[] = ['picture','name', 'category', 'price'];
  dataSource2: any;
  userId: string;
  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    private restaurantService: RestaurantService,
    @Inject(MAT_DIALOG_DATA) public data: string) {
      
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
