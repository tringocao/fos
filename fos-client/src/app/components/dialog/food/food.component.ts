import {
  Component,
  OnInit,
  ViewChild,
  Input,
  Output,
  EventEmitter
} from "@angular/core";
import { MatSort, MatPaginator, MatTableDataSource } from "@angular/material";
import { FoodCategory } from "src/app/models/food-category";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { Food } from "src/app/models/food";
import { SelectionModel } from "@angular/cdk/collections";
interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
}
interface FoodCheck {
  value: Food;
  checked: Boolean;
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
  count = 0;
  foodCategory: FoodCategory[] = [];
  @Input("data") data: RestaurantMore;
  @Input("isOrder") isOrder: boolean;
  @Output() valueChange = new EventEmitter<FoodCheck>();

  constructor(private restaurantService: RestaurantService) {}
  displayedColumns2: string[] = [
    "select",
    "picture",
    "name",
    "description",
    "price"
  ];
  dataSource2: MatTableDataSource<Food>;
  selection = new SelectionModel<Food>(true, []);
  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows =
      this.dataSource2 != undefined ? this.dataSource2.data.length : 0;
    return numSelected === numRows;
  }
  length = 1;
  isSomeSelected(ref, row) {
    var checked = this.selection.isSelected(row);
    ref.checked = checked;
    console.log(checked, row);
    this.valueChange.emit({
      value: this.selection.selected[this.selection.selected.length - 1],
      checked: checked
    });
  }
  /** Selects all rows if they are not all selected; otherwise clear selection. */
  // masterToggle(ref) {
  //   //if (this.isSomeSelected()) {
  //   // console.log(
  //   //   this.selection.selected ? this.selection.selected : "-----d",
  //   //   this.count++
  //   // );
  //   //this.selection.clear();
  //   //ref.checked = false;
  //   //} else {
  //   this.isAllSelected()
  //     ? this.selection.clear()
  //     : this.dataSource2.data.forEach(row => this.selection.select(row));
  //   //}
  // }
  // dsth() {}
  // /** The label for the checkbox on the passed row */
  // checkboxLabel(row?: Food, index?: number): string {
  //   if (!row) {
  //     return `${this.isAllSelected() ? "select" : "deselect"} all`;
  //   }
  //   return `${
  //     this.selection.isSelected(row) ? "deselect" : "select"
  //   } row ${index + 2}`;
  // }
  ngOnInit() {
    if (this.isOrder) {
      this.displayedColumns2 = [
        "select",
        "picture",
        "name",
        "description",
        "price"
      ];
    } else {
      this.displayedColumns2 = ["picture", "name", "description", "price"];
    }
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
