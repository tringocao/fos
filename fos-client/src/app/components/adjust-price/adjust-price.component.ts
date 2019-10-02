import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-adjust-price",
  templateUrl: "./adjust-price.component.html",
  styleUrls: ["./adjust-price.component.less"]
})
export class AdjustPriceComponent implements OnInit {
  @Input() baseCost: number;
  @Input() adjustedCost: number;
  constructor() {}

  ngOnInit() {}
  numberWithCommas(x: Number) {
    if (x < 0) return 0;
    if (x != undefined) {
      var parts = x.toString().split(".");
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      return parts.join(".");
    }
  }
}
