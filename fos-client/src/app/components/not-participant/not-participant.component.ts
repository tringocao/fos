import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from 'src/app/services/order/order.service';

@Component({
  selector: 'app-not-participant',
  templateUrl: './not-participant.component.html',
  styleUrls: ['./not-participant.component.less']
})
export class NotParticipantComponent implements OnInit {
  public OwnerForm: FormGroup;
  constructor(private route: ActivatedRoute, private orderService: OrderService) {
    this.OwnerForm = new FormGroup({
      // Title: new FormControl('')
    });
  }

  ngOnInit() {
    const guid = this.route.snapshot.paramMap.get('id');
    this.orderService.UpdateOrderStatusByOrderId(guid, 2).then(value=>{
      this.orderService.UpdateFoodDetailByOrderId(guid,'{}');
    })
  }

}
