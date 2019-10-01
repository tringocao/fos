import { Component, OnInit } from "@angular/core";
import { MatChipInputEvent } from '@angular/material';

@Component({
  selector: "app-promotions-chip",
  templateUrl: "./promotions-chip.component.html",
  styleUrls: ["./promotions-chip.component.less"]
})
export class PromotionsChipComponent implements OnInit {
  constructor() {}

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  // readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  // fruits: [] = [{ name: "Lemon" }, { name: "Lime" }, { name: "Apple" }];

  ngOnInit() {}
  // add(event: MatChipInputEvent): void {
  //   const input = event.input;
  //   const value = event.value;

  //   // Add our fruit
  //   if ((value || "").trim()) {
  //     this.fruits.push({ name: value.trim() });
  //   }

  //   // Reset the input value
  //   if (input) {
  //     input.value = "";
  //   }
  // }

  // remove(fruit: Fruit): void {
  //   const index = this.fruits.indexOf(fruit);

  //   if (index >= 0) {
  //     this.fruits.splice(index, 1);
  //   }
  // }
}
