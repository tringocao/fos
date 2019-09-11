import { Component, OnInit, ViewChild, Input } from "@angular/core";
import { User } from "src/app/models/user";
import { Order } from "src/app/models/order";
import { MatSort, MatPaginator, MatTableDataSource } from "@angular/material";
import { Food } from "src/app/models/food";
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { FLOAT, float } from 'html2canvas/dist/types/css/property-descriptors/float';

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
  public FoodOfAmount : any = {};

  constructor() {}
  load = true;
  @Input() totalBudget: Number;
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
  getTotalCost() {
    return this.dataSource2.data.map(t => parseFloat(t.Value["Price"].replace("đ",""))).reduce((acc, value) => acc + value, 0);
  }
  onBlurMethod(amount:number, food: Food){
    this.dataSource2.data.forEach(f =>{
      if(f.IdFood == food.Id){
        f.Value["Amount"] = amount.toString();
        f.Value["Total"] = (Number(f.Value["Amount"])*Number(f.Value["Price"])).toString()
      }
    })
  }
  getAllFoodDetail():FoodDetailJson[]{
    return this.dataSource2.data;
  }
  // setTotalPrice(){
  //   this.dataSource2.data.forEach(f =>{
  //     f.Value["Total"] = (Number(f.Value["Amount"])*Number(f.Value["Price"])).toString()
  //   })
  // }
  updateTable() {
    this.dataSource2.data = this.order.FoodDetail;
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
}
