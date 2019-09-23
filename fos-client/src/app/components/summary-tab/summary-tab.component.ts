import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-summary-tab',
  templateUrl: './summary-tab.component.html',
  styleUrls: ['./summary-tab.component.less']
})
export class SummaryTabComponent implements OnInit {
  index = 0;
  constructor() {}

  ngOnInit() {}

  selectedTabChange(event: any) {
    this.index = event.index;
  }
}
