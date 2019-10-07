import { Component, OnInit, Inject } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material';
import { OverlayContainer } from '@angular/cdk/overlay';
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-event-dialog-confirm',
  templateUrl: './event-dialog-confirm.component.html',
  styleUrls: ['./event-dialog-confirm.component.less']
})
export class EventDialogConfirmComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<EventDialogConfirmComponent>,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: string,
    private overlayContainer: OverlayContainer
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

  ngOnInit() {
    var self = this;
    // var dataSource = self.data;
    // console.log(dataSource);
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
