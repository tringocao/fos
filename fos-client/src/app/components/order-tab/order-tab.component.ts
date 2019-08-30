import { Component, OnInit, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'app-order-tab',
  templateUrl: './order-tab.component.html',
  styleUrls: ['./order-tab.component.less']
})
export class OrderTabComponent implements OnInit {
  isMyOrder = true;
  constructor() {}

  ngOnInit() {}
  selectedTabChange(event: any) {
    if (event.index === 0) {
      this.isMyOrder = true;
    } else {
      this.isMyOrder = false;
    }
  }
}
