import { Component, OnInit, Inject } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-reminder-dialog',
  templateUrl: './reminder-dialog.component.html',
  styleUrls: ['./reminder-dialog.component.less']
})
export class ReminderDialogComponent implements OnInit {
  participants: [];
  userNotOrder: User;

  constructor(
    public dialogRef: MatDialogRef<ReminderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) {}

  ngOnInit() {
    this.participants = JSON.parse(this.data);
    console.log('data: ', JSON.parse(this.data)[0]);
  }

  closeDialog($event) {
    this.dialogRef.close();
  }
}
