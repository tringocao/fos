import { Component, OnInit } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
@Component({
  selector: 'app-event-dialog-confirm',
  templateUrl: './event-dialog-confirm.component.html',
  styleUrls: ['./event-dialog-confirm.component.less']
})
export class EventDialogConfirmComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<EventDialogConfirmComponent>) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onYesClick(): void {
    this.dialogRef.close();
  }
}
