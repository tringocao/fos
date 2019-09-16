import { Component, OnInit, Inject } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material';
@Component({
  selector: 'app-event-dialog-confirm',
  templateUrl: './event-dialog-confirm.component.html',
  styleUrls: ['./event-dialog-confirm.component.less']
})
export class EventDialogConfirmComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<EventDialogConfirmComponent>,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    var self = this;
    // var dataSource = self.data;
    // console.log(dataSource);
    // debugger;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }
  onYesClick(): void {
    this.dialogRef.close();
  }
}
