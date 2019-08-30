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
    @Inject(MAT_DIALOG_DATA) public data: Restaurant) { }

    async ngOnInit(): Promise<void> {
      console.log("-------------------------------------")

      this.restaurantService.getFood(Number(this.data.delivery_id)).then(result => {
        this.foodCategory = result;
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


  


  onNoClick(): void {
    this.dialogRef.close();
  }

}
