import {
  Component,
  OnInit,
  ElementRef,
  HostListener,
  Input,
  Inject
} from "@angular/core";
import { TooltipPosition } from "@angular/material";
import { FormControl } from "@angular/forms";
import { Event } from "./../../models/event";
import { EventDialogViewComponent } from "../event-dialog-view/event-dialog-view.component";
import { MatDialog } from "@angular/material/dialog";
import { ReminderDialogComponent } from "../reminder-dialog/reminder-dialog.component";
import { EventDialogEditComponent } from "../event-dialog-edit/event-dialog-edit.component";
import { EventFormService } from "src/app/services/event-form/event-form.service";
import { GraphUser } from "src/app/models/graph-user";
import { OrderService } from "src/app/services/order/order.service";
@Component({
  selector: "app-show-action",
  templateUrl: "./show-action.component.html",
  styleUrls: ["./show-action.component.less"]
})
export class ShowActionComponent implements OnInit {
  isShowListAction: boolean;
  positionOptions: TooltipPosition[] = [
    "after",
    "before",
    "above",
    "below",
    "left",
    "right"
  ];
  position = new FormControl(this.positionOptions[2]);
  @Input() event: Event;

  @HostListener("document:click", ["$event"])
  clickout(event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isShowListAction = false;
    }
  }

  constructor(
    private eRef: ElementRef,
    private dialog: MatDialog,
    private eventFormService: EventFormService,
    private orderService: OrderService
  ) {}

  ngOnInit() {
    this.isShowListAction = false;
  }

  showListAction($event) {
    this.isShowListAction = !this.isShowListAction;
  }

  viewEvent() {
    const dialogRef = this.dialog.open(EventDialogViewComponent, {
      maxHeight: "98vh",
      width: "80%",
      data: this.event
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log("The dialog was closed");
    });
    // alert('view event: ' + this.event.EventId);
  }

  editEvent($event) {
    const dialogRef = this.dialog.open(EventDialogEditComponent, {
      maxHeight: "98vh",
      width: "80%",
      data: this.event
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log("The dialog was closed");
    });
    // alert('edit event: ' + this.event.EventId);
  }

  sendReminder($event) {
    const dialogRef = this.dialog.open(ReminderDialogComponent, {
      maxHeight: "98vh",
      minWidth: "50%",
      data: this.event
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log("The dialog was closed");
    });
    this.isShowListAction = false;
  }

  makeOrder($event) {
    alert("make order event: " + this.event.EventId);
    this.eventFormService.GetEventById(this.event.EventId).then(data => {
      const participants: GraphUser[] = JSON.parse(data.EventParticipantsJson);
      this.eventFormService
        .getCurrentUser()
        .toPromise()
        .then(result => {
          const participant = participants.filter(
            item => item.id === result.Data.id
          );
          if (participant.length > 0) {
            this.orderService
              .GetByEventvsUserId(this.event.EventId, result.Data.id)
              .then(order => {
                var url =
                  window.location.protocol +
                  "////" +
                  window.location.host +
                  "/make-order/" +
                  order.Id;
                window.open(url);
              });
          } else {
            var url =
              window.location.protocol +
              "////" +
              window.location.hostname +
              ":4200/make-order/ffa" +
              this.event.EventId;
            window.open(url);
          }
        });
    });
    this.isShowListAction = false;
  }

  viewOrder($event) {
    alert("view order event: " + this.event.EventId);
  }
}
