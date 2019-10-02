import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-adjust-price',
  templateUrl: './adjust-price.component.html',
  styleUrls: ['./adjust-price.component.less']
})
export class AdjustPriceComponent implements OnInit {

  @Input() baseCost: number;
  @Input() adjustedCost: number;
  constructor() { }

  ngOnInit() {
  }

}
