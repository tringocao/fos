import { Component, OnInit, ViewChild, Input } from "@angular/core";
import { MatSort, MatPaginator, MatTableDataSource } from "@angular/material";
import { FoodCategory } from "src/app/models/food-category";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { Food } from "src/app/models/food";
interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
}
@Component({
  selector: "app-food",
  templateUrl: "./food.component.html",
  styleUrls: ["./food.component.less"]
})
export class FoodComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  load = true;
  foodCategory: FoodCategory[] = [];
  @Input("data") data: RestaurantMore;
  constructor(private restaurantService: RestaurantService) {}
  displayedColumns2: string[] = ["picture", "name", "description", "price"];
  dataSource2: MatTableDataSource<Food>;
  ngOnInit() {
    this.GetFood(Number(this.data.restaurant.DeliveryId));
  }
  GetFood(deliveryId: number) {
    this.restaurantService.getFood(deliveryId).then(result => {
      result.forEach(c => this.foodCategory.push(c));
      this.showAll(this.foodCategory);
    });
  }
  updateTable(dataSourceTemp: Food[]) {
    console.log(dataSourceTemp);
    this.dataSource2 = new MatTableDataSource(dataSourceTemp);
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
  setFood($event) {
    if ($event.topic == 0) this.showAll(this.foodCategory);
    else this.updateTable(this.foodCategory[$event.topic].Dishes);
    this.load = false;
  }
  showAll(dataSourceTemp: FoodCategory[]) {
    console.log(dataSourceTemp);
    this.dataSource2 = new MatTableDataSource();

    dataSourceTemp.forEach(f => {
      if (f.Dishes != null) {
        f.Dishes.forEach(d => this.dataSource2.data.push(d));
      }
    });
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
}
