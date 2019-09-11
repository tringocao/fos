import { Component, OnInit, ViewChild, Input } from "@angular/core";
import { User } from "src/app/models/user";
import { Order } from "src/app/models/order";
import { MatSort, MatPaginator, MatTableDataSource } from "@angular/material";
import { Food } from "src/app/models/food";
import { FoodDetailJson } from "src/app/models/food-detail-json";

@Component({
  selector: "app-list-ordered-foods",
  templateUrl: "./list-ordered-foods.component.html",
  styleUrls: ["./list-ordered-foods.component.less"]
})
export class ListOrderedFoodsComponent implements OnInit {
  @Input() user: User;
  @Input() order: Order;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  foodOrdered: Food;
  displayedColumns2: string[] = ["name", "price", "amount", "total"];
  dataSource2: MatTableDataSource<FoodDetailJson>;
  constructor() {}
  load = true;
  ngOnInit() {
    this.dataSource2 = new MatTableDataSource<FoodDetailJson>();
    console.log(this.user.DisplayName);
    this.updateTable();
  }
  Caculator() {
    this.dataSource2.data;
  }
  MapFoodCheck2FoodDetail(food: Food): FoodDetailJson {
    return {
      IdFood: food.Id,
      Value: {
        ["Name"]: food.Name,
        ["Price"]: food.Price,
        ["Amount"]: "1",
        ["Total"]: food.Price
      }
    };
  }
  AddFoodDetail(food: FoodDetailJson) {
    this.dataSource2.data.unshift(food);
    this.dataSource2.filter = "";
  }
  DeleteFoodDetail(food: Food) {
    var deleteItem = this.dataSource2.data.findIndex(x => x.IdFood == food.Id);
    this.dataSource2.data.splice(deleteItem, 1);
    this.dataSource2.filter = "";
  }

  updateTable() {
    this.dataSource2.data = this.order.FoodDetail;
    this.dataSource2.data.push({
      IdFood: "-1",
      Value: {
        ["Name"]: "food.Name",
        ["Price"]: "food.Price",
        ["Amount"]: "1",
        ["Total"]: "food.Price"
      }
    });
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
}
