import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
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
  selector: 'app-food-categories',
  templateUrl: './food-categories.component.html',
  styleUrls: ['./food-categories.component.less']
})
export class FoodCategoriesComponent implements OnInit {
  ts = new FormControl();
  show$ = false;
  color = 'primary';
  mode = 'indeterminate';
  constructor() { }
  @Output()  change = new EventEmitter();
  @Input('loading') loading : boolean;
  @Input('toppingList') toppingList: FoodCategory[];


  openedChange(opened: boolean) {
    console.log(opened ? 'opened' : 'closed');
    if(!opened){
      this.loading = true;
      let topic = this.ts.value ? this.getTopic(this.ts.value) : 0
      this.change.emit({topic: topic});
      this.loading = false;

    }
  }
  getTopic(term: string){
    console.log(this.ts.value);
    return this.toppingList.map(x => x.dish_type_id ).indexOf(term);
  }
  ngOnInit() {
  }

}
