import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import Event from './../../models/event';
// import { EnvironmentService } from "../shared/service/environment.service";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl: string;

  constructor(
    private http: HttpClient // , private envService: EnvironmentService
  ) {
    // this.baseUrl = envService.getApiUrl() + "/api/order";
  }

  getAllOrder() {
    return this.http.get<any>(environment.apiUrl + 'api/splist/getallorder');
  }

  mapResponseDataToEvent(response: any) {
    const result: Event[] = [];
    response.forEach(element => {
      const event: Event = {
        restaurant: element.Restaurant,
        restaurantId: element.RestaurantId,
        category: element.Category,
        date: element.Date,
        participants: element.Participants,
        maximumBudget: element.MaximumBudget,
        hostName: element.HostName,
        hostId: element.HostId,
        name: element.Name,
        createdBy: element.CreatedBy,
        eventId: element.EventId,
        status: element.Status,
        timeToRemind: element.TimeToRemind
      };
      result.push(event);
    });
    return result;
  }
}
