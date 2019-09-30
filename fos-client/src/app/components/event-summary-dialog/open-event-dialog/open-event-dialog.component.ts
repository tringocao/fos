import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'app-open-event-dialog',
  templateUrl: './open-event-dialog.component.html',
  styleUrls: ['./open-event-dialog.component.less']
})
export class OpenEventDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<OpenEventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,
    overlayContainer: OverlayContainer
  ) {
    overlayContainer.getContainerElement().classList.add("app-theme1-theme");
  }
  ngOnInit() {}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
