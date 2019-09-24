import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FoodDetailJson } from 'src/app/models/food-detail-json';

@Component({
  selector: 'app-dialog-check-action',
  templateUrl: './dialog-check-action.component.html',
  styleUrls: ['./dialog-check-action.component.less']
})
export class DialogCheckActionComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<DialogCheckActionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FoodDetailJson) {}
  ngOnInit() {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
