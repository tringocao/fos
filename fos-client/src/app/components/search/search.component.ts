import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
  OnChanges,
  Input
} from "@angular/core";
import { Observable, Subject } from "rxjs";
import { RestaurantService } from "./../../services/restaurant/restaurant.service";
import { MatSelectModule } from "@angular/material/select";

import {
  debounceTime,
  distinctUntilChanged,
  switchMap,
  tap,
  finalize
} from "rxjs/operators";
import { FormControl, FormBuilder, FormGroup } from "@angular/forms";
import { stringify } from "@angular/compiler/src/util";
import { Restaurant } from "src/app/models/restaurant";
import { CategoryGroup } from "src/app/models/category-group";
import { Category } from "src/app/models/category";
import { DeliveryInfos } from "src/app/models/delivery-infos";

@Component({
  selector: "app-search",
  templateUrl: "./search.component.html",
  styleUrls: ["./search.component.less"]
})
export class SearchComponent implements OnInit, OnChanges {
  restaurant$: DeliveryInfos[];
  //restaurant: RestaurantSearch[] = [];

  private searchTerms = new Subject<string>();
  toppings = new FormControl();
  usersForm: FormGroup;
  isLoading = false;

  show$ = false;
  color = "primary";
  mode = "indeterminate";
  toppingList: CategoryGroup[];
  @Input() idService: number;
  @Input("loading") loading: boolean;
  @ViewChild(MatSelectModule, { static: true }) x: MatSelectModule;
  @Output() change = new EventEmitter();
  submitted = false;
  isOpen = false;
  isChecked = false;
  isMyFavorite = false;

  ngOnInit(): void {
    this.restaurantService.GetMetadataForCategory().then(result => {
      result.forEach((element, index) => {
        if (element.Categories.length < 1) {
          let selectAll: Category = {
            Name: "All",
            Id: element.Id,
            Code: element.Code
          };
          element.Categories.push(selectAll);
        }
      });
      this.toppingList = result;
    });
    this.usersForm = this.fb.group({
      userInput: null
    });

    this.usersForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(500),
        tap(() => (this.isLoading = true)),
        switchMap(value =>
          this.restaurantService
            .SearchRestaurantName(value, 4, this.idService, 217)
            .pipe(finalize(() => (this.isLoading = true)))
        )
      )
      .subscribe(data =>
        this.restaurantService
          .getRestaurants(data.Data, this.idService, 217)
          .then(result => {
            this.restaurant$ = result;
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
  ngOnChanges(changes: import("@angular/core").SimpleChanges): void {
    //console.log(changes);
  }
  onBlur() {
    this.show$ = false;
  }
  constructor(
    private fb: FormBuilder,
    private restaurantService: RestaurantService
  ) {}
  displayFn(user: DeliveryInfos) {
    if (user) {
      return user.Name;
    }
  }
  closeMiniSearch() {
    this.onSubmit();
  }
  filterByFavorite($event) {
    //console.log($event);
    this.isChecked = $event.checked;
    this.change.emit({ isChecked: $event.checked });
    //this.change.emit({ isChecked: event.checked });
  }

  openedChange(opened: boolean) {
    //console.log(opened ? "opened" : "closed");
    if (!opened) {
      this.onSubmit();
    }
  }
  onSubmit() {
    this.submitted = true;
    let cod = this.toppings.value
      ? this.getCondition(this.toppings.value)
      : "[]";
    //console.log(cod);
    var keyword = "";
    if (this.usersForm.get("userInput").value != null) {
      keyword = this.usersForm.get("userInput").value.Name
        ? this.usersForm.get("userInput").value.Name
        : this.usersForm.get("userInput").value;
    }
    this.isChecked = false;
    this.change.emit({
      topic: JSON.parse(cod),
      keyword: keyword,
      isChecked: this.isChecked
    });
  }

  getCondition(term: Category[]): string {
    let getCod = "";
    term.forEach(e => {
      getCod = getCod + ',{"code":' + e.Code + ',"id":' + e.Id + "}";
    });
    getCod = getCod.substr(1);
    return "[" + getCod + "]";
  }
  // Push a search term into the observable stream
}
