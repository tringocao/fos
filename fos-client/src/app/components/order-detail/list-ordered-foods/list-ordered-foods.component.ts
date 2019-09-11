import { Component, OnInit, ViewChild, Input } from "@angular/core";
import { User } from "src/app/models/user";
import { Order } from "src/app/models/order";
import { MatSort, MatPaginator, MatTableDataSource } from "@angular/material";
import { Food } from "src/app/models/food";
interface FoodCheck {
  value: Food;
  checked: Boolean;
}
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
  foodOrdered: FoodCheck;
  displayedColumns2: string[] = ["name", "price", "amount", "total"];
  dataSource2: MatTableDataSource<{ [key: number]: { [key: string]: string } }>;
  constructor() {}

  ngOnInit() {
    console.log(this.user.DisplayName);
  }
  updateTable(dataSourceTemp: FoodCheck[]) {
    // console.log(dataSourceTemp);
    // this.dataSource2 = new MatTableDataSource(dataSourceTemp);
    // this.dataSource2.sort = this.sort;
    // this.dataSource2.paginator = this.paginator;
    // this.load = false;
  }
}
