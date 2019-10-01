import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { OrderService } from "src/app/services/order/order.service";
import { EventFormService } from "src/app/services/event-form/event-form.service";

@Component({
  selector: "app-not-participant",
  templateUrl: "./not-participant.component.html",
  styleUrls: ["./not-participant.component.less"]
})
export class NotParticipantComponent implements OnInit {
  public OwnerForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private eventService: EventFormService,
    private router: Router
  ) {
    this.OwnerForm = new FormGroup({
      // Title: new FormControl('')
    });
  }
  ngOnInit() {
    const guid = this.route.snapshot.paramMap.get("id");
    this.orderService.GetOrder(guid).then(o => {
      const eventId = o.IdEvent;
      this.eventService.GetEventById(eventId).then(e => {
        const eventStatus = e.Status;
        if (eventStatus === "Closed") {
          this.router.navigateByUrl("events");
        } else {
          this.orderService.UpdateOrderStatusByOrderId(guid, 2).then(value => {
            this.orderService.UpdateFoodDetailByOrderId(guid, "{}");
          });
        }
      });
    });
  }
}
