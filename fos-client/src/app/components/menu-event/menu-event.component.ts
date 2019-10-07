import { Component, OnInit, Inject, Input } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
// import { DialogComponent } from '../dialog/dialog.component';
import { EventDialogComponent } from "../event-dialog/event-dialog.component";
import { NoopScrollStrategy, Overlay } from "@angular/cdk/overlay";

interface Restaurant {
  id: string;
  stared: boolean;
  restaurant: string;
  category: string;
  address: string;
  promotion: string;
  open: string;
  delivery_id: string;
  url_rewrite_name: string;
}
@Component({
  selector: "app-menu-event",
  templateUrl: "./menu-event.component.html",
  styleUrls: ["./menu-event.component.less"]
})
export class MenuEventComponent implements OnInit {
  overlay: Overlay;
  @Input() idService: number;

  @Input("restaurant") restaurant: Restaurant;
  constructor(public dialog: MatDialog, overlay: Overlay) {
    this.overlay = overlay;
  }

  ngOnInit() {}
  openDialog(): void {
    //console.log("restaurant", this.restaurant);

    const dialogRef = this.dialog.open(EventDialogComponent, {
      // scrollStrategy: this.overlay.scrollStrategies.noop(),
      // autoFocus: false,
      maxHeight: "98vh",
      width: "80%",
      data: { restaurant: this.restaurant, idService: this.idService }
    });

    dialogRef.afterClosed().subscribe(result => {
      //console.log("The dialog was closed");
    });
  }
}
