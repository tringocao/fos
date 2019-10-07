import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { Promotion } from "src/app/models/promotion";
import { PromotionType } from "src/app/models/promotion-type";
import { EventPromotionService } from "src/app/services/event-promotion/event-promotion.service";
import { EventPromotion } from "src/app/models/event-promotion";
import { MatSnackBar, MatDialog, MatDialogRef } from "@angular/material";
import { AddPromotionDialogComponent } from "../event-summary-dialog/add-promotion-dialog/add-promotion-dialog.component";

@Component({
  selector: "app-promotions-chip-list",
  templateUrl: "./promotions-chip-list.component.html",
  styleUrls: ["./promotions-chip-list.component.less"]
})
export class PromotionsChipListComponent implements OnInit {
  constructor(
    private eventPromotionService: EventPromotionService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<AddPromotionDialogComponent>
  ) {}

  @Input() deliveryId: number;
  @Input() isHost: boolean;
  @Input() eventId: string;
  @Input() changable: boolean;
  @Output() promotionChanged: EventEmitter<Promotion[]> = new EventEmitter();

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;

  @Input() eventPromotion: EventPromotion;
  @Input() promotions: Promotion[] = [];
  promotionOptions: Promotion[] = [];
  promotionType: PromotionType = PromotionType.DiscountAll;
  promotionValue: string = "10";
  promotionTypeOfValue: string = "%";

  ngOnInit() {
    const PromotionTypes = Object.keys(PromotionType).filter(
      key => !isNaN(Number(PromotionType[key]))
    );
    PromotionTypes.forEach((type, index) => {
      if (index > 0) {
        const promotion = new Promotion();
        promotion.PromotionType = index;
        promotion.IsPercent = !(index === 2);
        this.promotionOptions.push(promotion);
      }
    });
    // this.eventPromotionService.GetByEventId(Number(this.eventId)).then(eventPromotion => {
    //   this.eventPromotion = eventPromotion;
    //   this.promotions = this.eventPromotion.Promotions;
    //   this.promotionChanged.emit(this.promotions);
    // });
  }

  getPromotionName(promotionType: number): string {
    return PromotionType[promotionType].split(/(?=[A-Z])/).join(" ");
  }

  isPromotionPercent(): boolean {
    return this.promotionOptions[this.promotionType - 1].IsPercent;
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  updateEventPromotion() {
    this.eventPromotion.Promotions.forEach(promotion => {
      if (promotion.PromotionType !== PromotionType.DiscountPerItem) {
        promotion.DiscountedFoodIds = null;
      }
    });
    this.eventPromotionService
      .UpdateEventPromotion(this.eventPromotion)
      .then(response => {
        if (response != null) {
          this.toast("update fail", "Dismiss");
        }
        this.toast("update success", "Dismiss");
      });
  }

  addNewPromotion() {
    const dialogRef = this.dialog.open(AddPromotionDialogComponent, {
      maxWidth: "50%",
      data: {}
    });

    dialogRef.afterClosed().subscribe((promotion: Promotion) => {
      if (promotion && promotion.PromotionType) {
        this.addToPromotions(promotion);
      }
    });
  }

  addToPromotions(promotion) {
    // const promotion = new Promotion();
    // promotion.PromotionType = this.promotionType;
    // promotion.Value = Number(this.promotionValue);
    // promotion.IsPercent = this.promotionTypeOfValue === "%";
    // promotion.MaxDiscountAmount = 0;
    // promotion.MinOrderAmount = 0;
    this.promotions.push(promotion);
    this.promotionChanged.emit(this.promotions);
    // //console.log(this.promotions);
  }

  removePromotion(promotion: Promotion) {
    this.promotions = this.promotions.filter(pr => pr !== promotion);
    this.promotionChanged.emit(this.promotions);
  }
}
