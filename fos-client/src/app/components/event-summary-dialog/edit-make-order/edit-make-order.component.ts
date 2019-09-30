import { Component, OnInit, Output, Input } from "@angular/core";
import { Event } from "src/app/models/event";
import { User } from "src/app/models/user";
import { OrderService } from "src/app/services/order/order.service";

@Component({
  selector: "app-edit-make-order",
  templateUrl: "./edit-make-order.component.html",
  styleUrls: ["./edit-make-order.component.less"]
})
export class EditMakeOrderComponent implements OnInit {
  constructor(private orderService: OrderService) {}
  @Input() isHostUser: boolean;
  @Input() eventDetail: Event;
  @Input() user: User;

  ngOnInit() {}
  makeOrderByHost() {
    if (this.isHostUser) {
      this.orderService
        .GetByEventvsUserId(this.eventDetail.EventId, this.user.Id)
        .then(order => {
          var url =
            window.location.protocol +
            "////" +
            window.location.host +
            "/make-order/" +
            order.Id;
          window.open(url);
        });
    }
  }
}
