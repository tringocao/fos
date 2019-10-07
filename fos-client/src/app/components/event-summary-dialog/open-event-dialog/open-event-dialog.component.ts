import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OverlayContainer } from '@angular/cdk/overlay';
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';

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
    ,
    private dataRouting: DataRoutingService
 ) {
      this.getNavTitleSubscription = this.dataRouting.getNavTitle()
   .subscribe((appTheme: string) => this.appTheme = appTheme);
   overlayContainer.getContainerElement().classList.add("app-"+this.appTheme+"-theme");
 }
 ngOnDestroy() {
   // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
   this.getNavTitleSubscription.unsubscribe();
 }
 private getNavTitleSubscription: Subscription;
 appTheme: string;
  ngOnInit() {}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
