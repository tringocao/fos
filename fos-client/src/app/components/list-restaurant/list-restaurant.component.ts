import { Component, OnInit } from '@angular/core';
import { RestaurantService } from './../../services/restaurant/restaurant.service';

@Component({
  selector: 'app-list-restaurant',
  templateUrl: './list-restaurant.component.html',
  styleUrls: ['./list-restaurant.component.less']
})
export class ListRestaurantComponent implements OnInit {
  restaurants: any;
  sortNameOrder: number;
  sortCategoryOrder: number;
  replicateRestaurants: any;
  categorys: any;

  constructor(private restaurantService: RestaurantService) {}

  ngOnInit() {
    this.restaurants = [
      {
        restaurant: 'a',
        category: 'b',
        address: 'c',
        phone: 'c',
        promotion: 'sale off 100%',
        open: 'sale off 100%'
      },
      {
        restaurant: 'e',
        category: 'f',
        address: 'g',
        phone: 'h'
      },
      {
        restaurant: 'k',
        category: 'l',
        address: 'm',
        phone: 'n'
      },
      {
        restaurant: 'a',
        category: 'b',
        address: 'c',
        phone: 'c'
      }
    ];
    this.categorys = ['a', 'b', 'c', 'd'];
    this.sortNameOrder = 0;
    this.sortCategoryOrder = 0;
  }
  getRestaurant() {
    const data = this.restaurantService.getRestaurant().subscribe(response => {
      this.restaurants = response;
    });
  }
  sortName() {
    this.sortCategoryOrder = 0;
    if (this.sortNameOrder === 2) {
      this.sortUp(true);
      this.sortNameOrder = 0;
    } else if (this.sortNameOrder === 1) {
      this.sortDown(true);
      this.sortNameOrder = 2;
    } else {
      this.restaurants = this.replicateRestaurants.slice();
      this.sortUp(true);
      this.sortNameOrder = 1;
    }
  }

  sortUp(isName: boolean) {
    let switching = true;
    let i;
    let x;
    let y;
    let shouldSwitch;

    while (switching) {
      switching = false;
      for (i = 0; i < this.restaurants.length - 1; i++) {
        shouldSwitch = false;
        if (isName) {
          x = this.restaurants[i].restaurant;
          y = this.restaurants[i + 1].restaurant;
        } else {
          x = this.restaurants[i].category;
          y = this.restaurants[i + 1].category;
        }
        if (x > y) {
          shouldSwitch = true;
          break;
        }
      }
      if (shouldSwitch) {
        const secondElement = this.restaurants[i + 1];
        this.restaurants[i + 1] = this.restaurants[i];
        this.restaurants[i] = secondElement;
        switching = true;
      }
    }
    console.log('replicate restaurant', this.replicateRestaurants);
  }

  sortDown(isName: boolean) {
    let switching = true;
    let i;
    let x;
    let y;
    let shouldSwitch;

    while (switching) {
      switching = false;
      for (i = 0; i < this.restaurants.length - 1; i++) {
        shouldSwitch = false;
        if (isName) {
          x = this.restaurants[i].restaurant;
          y = this.restaurants[i + 1].restaurant;
        } else {
          x = this.restaurants[i].category;
          y = this.restaurants[i + 1].category;
        }
        if (x < y) {
          shouldSwitch = true;
          break;
        }
      }
      if (shouldSwitch) {
        const secondElement = this.restaurants[i + 1];
        this.restaurants[i + 1] = this.restaurants[i];
        this.restaurants[i] = secondElement;
        switching = true;
      }
    }
    console.log('replicate restaurant', this.replicateRestaurants);
  }

  sortCategory() {
    this.sortNameOrder = 0;
    if (this.sortCategoryOrder === 2) {
      this.sortUp(false);
      this.sortCategoryOrder = 0;
    } else if (this.sortCategoryOrder === 1) {
      this.sortDown(false);
      this.sortCategoryOrder = 2;
    } else {
      this.restaurants = this.replicateRestaurants.slice();
      this.sortUp(false);
      this.sortCategoryOrder = 1;
    }
  }
}
