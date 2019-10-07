import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MatChipInputEvent } from "@angular/material";
import { PromotionType } from "src/app/models/promotion-type";
import { OverlayContainer } from "@angular/cdk/overlay";
import { Promotion } from "src/app/models/promotion";
import { DataRoutingService } from "src/app/data-routing.service";
import { Subscription } from "rxjs";

@Component({
  selector: "app-promotions-chip",
  templateUrl: "./promotions-chip.component.html",
  styleUrls: ["./promotions-chip.component.less"]
})
export class PromotionsChipComponent implements OnInit {
  constructor(
    private overlayContainer: OverlayContainer,
    private dataRouting: DataRoutingService
  ) {
    this.getNavTitleSubscription = this.dataRouting
      .getNavTitle()
      .subscribe((appTheme: string) => (this.appTheme = appTheme));
    overlayContainer
      .getContainerElement()
      .classList.add("app-" + this.appTheme + "-theme");
  }
  ngOnDestroy() {
    // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
    this.getNavTitleSubscription.unsubscribe();
  }
  private getNavTitleSubscription: Subscription;
  appTheme: string;
  @Input() visible = true;
  @Input() selectable = true;
  @Input() removable = true;
  @Input() addOnBlur = true;

  @Input() promotion;
  @Output() removePressed: EventEmitter<Promotion> = new EventEmitter();

  color = "primary";

  ngOnInit() {}

  getPromotionName(promotionType: number): string {
    return PromotionType[promotionType].split(/(?=[A-Z])/).join(" ");
  }

  getPromotionStyle(promotionType: number) {
    switch (promotionType) {
      case 2:
        return "pay-more";
      default:
        return "pay-less";
    }
  }

  removePromotion() {
    this.removePressed.emit(this.promotion);
  }

  getPromotionIcon() {}
}
