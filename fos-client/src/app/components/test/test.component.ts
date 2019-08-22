import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order/order.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.less']
})
export class TestComponent implements OnInit {
  testData: any;

  constructor(private orderService: OrderService) {}

  ngOnInit() {}

  click() {
    const data = this.orderService.getOrder('1').subscribe(response => {
      this.testData = response;
    });
  }
}
