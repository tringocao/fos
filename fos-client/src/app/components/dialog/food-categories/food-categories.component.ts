import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { FormControl } from "@angular/forms";
import { FoodCategory } from "src/app/models/food-category";

@Component({
  selector: "app-food-categories",
  templateUrl: "./food-categories.component.html",
  styleUrls: ["./food-categories.component.less"]
})
export class FoodCategoriesComponent implements OnInit {
  @Output() change = new EventEmitter();
  @Input("loading") loading: boolean;
  @Input("toppingList") toppingList: FoodCategory[];
  ts = new FormControl();
  color = "primary";
  mode = "indeterminate";
  constructor() {}
  openedChange(opened: boolean) {
    //console.log(opened ? "opened" : "closed");
    if (!opened) {
      this.loading = true;
      let topic = this.ts.value ? this.getTopic(this.ts.value) : 0;
      this.change.emit({ topic: topic });
      this.loading = false;
    }
  }
  getTopic(term: string) {
    //console.log(this.ts.value);
    return this.toppingList.map(x => x.DishTypeId).indexOf(term);
  }
  ngOnInit() {
    var name: string = "Show all";
    let allfood: FoodCategory = {
      DishTypeName: name,
      DishTypeId: "-1",
      Dishes: null
    };
    this.toppingList.push(allfood);
  }
}
