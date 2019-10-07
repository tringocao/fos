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
import { FoodDetailJson } from "src/app/models/food-detail-json";
import { TablePaging } from "src/app/models/table-paging";
import { Promotion } from "src/app/models/promotion";

interface RestaurantMore {
  restaurant: DeliveryInfos;
  detail: RestaurantDetail;
  idService: number;
}
interface FoodCheck {
  food: Food;
  checked: boolean;
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
  @Input() discountPerItem: Promotion;

  @Input("isOrder") isOrder: boolean;
  @Output() valueChange = new EventEmitter<FoodCheck>();
  @Input("checkedData") checkedData: FoodDetailJson[];

  docsOnThisPage: any[] = [];
  from: number;
  pageSize: number;
  constructor(private restaurantService: RestaurantService) {
    this.pageSize = TablePaging.PagingNumber;
  }
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
  IsSomeSelected(ref, row) {
    var checked = this.selection.isSelected(row);
    ref.checked = checked;
    console.log(checked, row);
    this.valueChange.emit({
      food: row,
      checked: checked
    });
  }
  numberWithCommas2(x: Number) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
  }
  checked(row: Food) {
    this.selection.select(row);
    var found = this.selection.selected.find(x => x.Id == row.Id);
    if (found) found.IsChecked = true;
  }
  isChecked(row: Food) {
    var found = this.selection.selected.find(x => x.Id == row.Id);
    if (found) return found.IsChecked;
  }
  unChecked(row: Food) {
    var found = this.selection.selected.find(x => x.Id == row.Id);
    if (found) found.IsChecked = false;
    this.selection.deselect(found);
  }
  ngOnInit() {
    if (this.isOrder) {
      this.displayedColumns2 = [
        "select",
        "Photos",
        "Name",
        "Description",
        "Price"
      ];
    } else {
      this.displayedColumns2 = ["Photos", "Name", "Description", "Price"];
    }
    this.GetFood(Number(this.data.restaurant.DeliveryId));
  }
  GetFood(deliveryId: number) {
    this.restaurantService
      .getFood(deliveryId, this.data.idService)
      .then(result => {
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
        f.Dishes.forEach(d => {
          if (this.isOrder) {
            this.checkedData.forEach(cd => {
              if (d.Id == cd.IdFood) {
                d.IsChecked = true;
                this.selection.select(d);
              }
            });
          }
          this.dataSource2.data.push(d);
        });
      }
    });
    this.dataSource2.sort = this.sort;
    this.dataSource2.paginator = this.paginator;
    this.load = false;
  }
  MapFoodDetail2Food(food: FoodDetailJson): Food {
    return {
      Id: food.IdFood,
      Name: food.Value["Name"],
      Photos: null,
      Description: null,
      Price: null,
      IsChecked: null,
      IsDiscountedFood: food.Value["IsDiscountedFood"] === "true"
    };
  }
  setNewPrice(price: number, foodId: string): number {
    if (this.discountPerItem == null) return price;
    if (!this.discountPerItem.DiscountedFoodIds[Number(foodId)]) {
      return price;
    } else {
      var newPrice =
        price + this.discountPerItem.DiscountedFoodIds[Number(foodId)];
      return newPrice;
    }
  }
}
