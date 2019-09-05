import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
  OnChanges,
  Input
} from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { RestaurantService } from './../../services/restaurant/restaurant.service';
import { MatSelectModule } from '@angular/material/select';

import {
  debounceTime,
  distinctUntilChanged,
  switchMap,
  tap,
  finalize
} from 'rxjs/operators';
import { FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { stringify } from '@angular/compiler/src/util';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.less']
})
export class SearchComponent implements OnInit, OnChanges {
  restaurant$: Restaurant[];
  //restaurant: RestaurantSearch[] = [];

  private searchTerms = new Subject<string>();
  toppings = new FormControl();
  usersForm: FormGroup;
  isLoading = false;

  show$ = false;
  color = 'primary';
  mode = 'indeterminate';
  toppingList: CategoryGroup[];
  @Input('loading') loading: boolean;
  @ViewChild(MatSelectModule, { static: true }) x: MatSelectModule;
  @Output() change = new EventEmitter();
  submitted = false;
  isOpen = false;
  ngOnInit(): void {
    this.restaurantService.GetMetadataForCategory().then(result => {
      result.forEach((element, index) => {
        if (element.categories.length < 1) {
          let selectAll: Category = {
            name: 'All',
            id: element.id,
            code: element.code
          };
          element.categories.push(selectAll);
        }
      });
      this.toppingList = result;
    });
    this.usersForm = this.fb.group({
      userInput: null
    });

    this.usersForm
      .get('userInput')
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this.isLoading = true)),
        switchMap(value =>
          this.restaurantService
            .SearchRestaurantName(value, 4)
            .pipe(finalize(() => (this.isLoading = true)))
        )
      )
      .subscribe(data =>
        this.restaurantService.getRestaurants(data.Data).then(result => {
          var dataSourceTemp = [];
          result.forEach((element, index) => {
            // tslint:disable-next-line:prefer-const
            let restaurantItem: Restaurant = {
              id: element.restaurant_id,
              delivery_id: element.delivery_id,
              stared: false,
              restaurant: element.name,
              address: element.address,
              category:
                element.categories.length > 0 ? element.categories[0] : '',
              promotion:
                element.promotion_groups.length > 0
                  ? element.promotion_groups[0].text
                  : '',
              open:
                (element.operating.open_time || '?') +
                '-' +
                (element.operating.close_time || '?'),
              url_rewrite_name: '',
              picture: "",
            };
            dataSourceTemp.push(restaurantItem);
          });
          this.restaurant$ = dataSourceTemp;
          this.isLoading = false;
        })
      );
    // this.restaurant$ = this.searchTerms.pipe(
    //   // wait 500ms after each keystroke before considering the term
    //   debounceTime(500),

    //   // ignore new term if same as previous term
    //   distinctUntilChanged(),
    //   // switch to new search observable each time the term changes
    //   switchMap((term: string) =>this.restaurantService.SearchRestaurantName(term, 4)),
    //   );
  }
  ngOnChanges(changes: import('@angular/core').SimpleChanges): void {
    console.log(changes);
  }
  onBlur() {
    this.show$ = false;
  }
  constructor(
    private fb: FormBuilder,
    private restaurantService: RestaurantService
  ) {}
  displayFn(user: Restaurant) {
    if (user) {
      return user.restaurant;
    }
  }

  openedChange(opened: boolean) {
    console.log(opened ? 'opened' : 'closed');
    if (!opened) {
      this.onSubmit();
    }
  }
  onSubmit() {
    this.submitted = true;
    let cod = this.toppings.value
      ? this.getCondition(this.toppings.value)
      : '[]';
    console.log(cod);
    var keyword = '';
    if (this.usersForm.get('userInput').value != null) {
      keyword = this.usersForm.get('userInput').value.restaurant
        ? this.usersForm.get('userInput').value.restaurant
        : this.usersForm.get('userInput').value;
    }

    this.change.emit({ topic: JSON.parse(cod), keyword: keyword });
  }
  getCondition(term: Category[]): string {
    let getCod = '';
    term.forEach(e => {
      getCod = getCod + ',{"code":' + e.code + ',"id":' + e.id + '}';
    });
    getCod = getCod.substr(1);
    return '[' + getCod + ']';
  }
  // Push a search term into the observable stream
}
