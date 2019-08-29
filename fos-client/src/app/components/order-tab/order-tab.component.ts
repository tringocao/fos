import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-order-tab',
  templateUrl: './order-tab.component.html',
  styleUrls: ['./order-tab.component.less']
})
export class OrderTabComponent implements OnInit {
  isMyOrder = true;
  constructor() {}

  ngOnInit() {}

  tabChange(tabChangeEvent: any): void {
    if (tabChangeEvent.index === 0) {
      this.isMyOrder = true;
    } else {
      this.isMyOrder = false;
    }
  }
}
