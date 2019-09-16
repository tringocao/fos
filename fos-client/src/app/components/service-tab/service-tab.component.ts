import { Component, OnInit } from '@angular/core';
import { ExternalFoodService } from 'src/app/services/external-food/external-food.service';
import { ExternalService } from 'src/app/models/external-service';

@Component({
  selector: 'app-service-tab',
  templateUrl: './service-tab.component.html',
  styleUrls: ['./service-tab.component.less']
})
export class ServiceTabComponent implements OnInit {
  data:ExternalService[];
  constructor(private externalFoodService: ExternalFoodService) { }

  ngOnInit() {
    this.externalFoodService.GetAllExternalService().then(result => this.data = result)
  }

}
