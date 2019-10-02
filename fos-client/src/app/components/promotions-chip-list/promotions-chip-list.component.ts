import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Promotion } from 'src/app/models/promotion';
import { PromotionType } from 'src/app/models/promotion-type';
import { EventPromotionService } from 'src/app/services/event-promotion/event-promotion.service';

@Component({
  selector: 'app-promotions-chip-list',
  templateUrl: './promotions-chip-list.component.html',
  styleUrls: ['./promotions-chip-list.component.less']
})
export class PromotionsChipListComponent implements OnInit {
  constructor(private eventPromotionService: EventPromotionService) { }

  @Input() deliveryId: number;
  @Output() promotionChanged: EventEmitter<Promotion[]> = new EventEmitter();

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;

  promotions: Promotion[] = [];
  promotionOptions: Promotion[] = [];
  promotionType: PromotionType = PromotionType.DiscountAll;
  promotionValue: string = '10';

  ngOnInit() {
    const PromotionTypes = Object.keys(PromotionType).filter(key => !isNaN(Number(PromotionType[key])));
    PromotionTypes.forEach((type, index) => {
      if (index > 0) {
        const promotion = new Promotion();
        promotion.PromotionType = index;
        promotion.IsPercent = !(index === 2);
        this.promotionOptions.push(promotion);
      }
    });
    console.log(this.deliveryId)
    this.eventPromotionService.getPromotionsByExternalService(Number(this.deliveryId), 1).then(promotions => {
      this.promotions = promotions;
      this.promotionChanged.emit(this.promotions);
    });
  }

  getPromotionName(promotionType: number): string {
    return PromotionType[promotionType].split(/(?=[A-Z])/).join(' ');
  }

  isPromotionPercent(): boolean {
    console.log(this.promotionType);
    console.log(this.promotionOptions)
    return this.promotionOptions[this.promotionType - 1 ].IsPercent;
  }

  addToPromotions() {
    const promotion = new Promotion();
    promotion.PromotionType = this.promotionType;
    promotion.Value = Number(this.promotionValue);
    promotion.IsPercent = this.isPromotionPercent();
    this.promotions.push(promotion);
    this.promotionChanged.emit(this.promotions);
    // console.log(this.promotions);
  }

  removePromotion(promotion: Promotion) {
    this.promotions = this.promotions.filter(pr => pr !== promotion);
    this.promotionChanged.emit(this.promotions);
  }

}
