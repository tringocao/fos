import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order/order.service';
import { RestaurantService } from './../../services/restaurant/restaurant.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.less']
})
export class TestComponent implements OnInit {
  testData: any;
  restaurants: any;

  constructor(
    private orderService: OrderService,
    private restaurantService: RestaurantService
  ) {}

  ngOnInit() {}

  click() {
    const data = this.orderService.getOrder('1').subscribe(response => {
      this.testData = response;
    });
  }

  getRestaurant() {
    const data = this.restaurantService.getRestaurant().subscribe(response => {
      this.restaurants = response;
    });
  }
}
