import { Component, OnInit, Inject } from '@angular/core';
import { PromotionType } from 'src/app/models/promotion-type';
import { Promotion } from 'src/app/models/promotion';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-add-promotion-dialog',
  templateUrl: './add-promotion-dialog.component.html',
  styleUrls: ['./add-promotion-dialog.component.less']
})
export class AddPromotionDialogComponent implements OnInit {
  promotionOptions: Promotion[] = [];
  promotionType: PromotionType = PromotionType.DiscountAll;
  promotionValue: string = "10";
  promotionTypeOfValue: string = "%";
  constructor(private dialogRef: MatDialogRef<AddPromotionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

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
  }

  closeDialog() {
    const promotion = new Promotion();
    promotion.PromotionType = this.promotionType;
    promotion.Value = Number(this.promotionValue);
    promotion.IsPercent = this.promotionTypeOfValue === "%";
    promotion.MaxDiscountAmount = 0;
    promotion.MinOrderAmount = 0;
    this.dialogRef.close(promotion);
  }

  updatePromotion() {
    this.dialogRef.close('update');
  }

  getPromotionName(promotionType: number): string {
    return PromotionType[promotionType].split(/(?=[A-Z])/).join(" ");
  }

  isPromotionPercent(): boolean {
    return this.promotionOptions[this.promotionType - 1].IsPercent;
  }

}
