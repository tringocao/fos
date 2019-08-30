import { Component, OnInit, ViewChild, Output, EventEmitter, OnChanges, Input } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { RestaurantService } from './../../services/restaurant/restaurant.service';
import {MatSelectModule} from '@angular/material/select';

import {
  debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { stringify } from '@angular/compiler/src/util';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.less']
})
export class SearchComponent implements OnInit, OnChanges {
  restaurant$: Observable<RestaurantSearch[]>;
  private searchTerms = new Subject<string>();
  toppings = new FormControl();
  show$ = false;
  keyword = "";
  color = 'primary';
  mode = 'indeterminate';
  toppingList: CategoryGroup[];
  @Input('loading') loading : boolean;
  @ViewChild(MatSelectModule, { static: true }) x: MatSelectModule;
  @Output()  change = new EventEmitter();
  submitted = false;
  isOpen = false;
  ngOnInit(): void {
    this.loading = true;
    this.restaurantService.GetMetadataForCategory().then(result => {
      result.forEach((element, index) => {
        if(element.categories.length < 1){
          let selectAll : Category = {
            name: "All",
            id: element.id,
            code: element.code
          };
          element.categories.push(selectAll);
        }
      });
      this.toppingList = result;
    });
    this.restaurant$ = this.searchTerms.pipe(
      // wait 500ms after each keystroke before considering the term
      debounceTime(500),

      // ignore new term if same as previous term
      distinctUntilChanged(),
      // switch to new search observable each time the term changes
      switchMap((term: string) =>this.restaurantService.SearchRestaurantName(term, 4)),
      );
    }
  ngOnChanges(changes: import("@angular/core").SimpleChanges): void {
    console.log(changes);
  }
  onBlur(){
    this.show$ = false;
  }
  constructor(private restaurantService: RestaurantService) {
      
  }


  
  openedChange(opened: boolean) {
    console.log(opened ? 'opened' : 'closed');
    if(!opened){
      this.onSubmit();
    }
  }
  onSubmit() { 
    this.submitted = true;
    let cod = this.toppings.value ? this.getCondition(this.toppings.value) : "[]"
    console.log(cod);
    this.change.emit({topic: JSON.parse(cod), keyword: this.keyword});
    this.keyword = "";
  }
  getCondition(term:Category[]):string{
    let getCod = "";
    term.forEach(e =>{
      getCod = getCod + ",{\"code\":" + e.code + ",\"id\":" + e.id + "}"
    });
    getCod = getCod.substr(1);
    return "[" + getCod + "]";
  }
  // Push a search term into the observable stream.
  search(term: string): void {
    this.keyword = term;
    this.show$ = term != ""? true : false;
    console.log(this.show$);
    this.searchTerms.next(term);
  }

}