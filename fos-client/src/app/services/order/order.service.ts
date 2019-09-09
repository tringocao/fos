import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EventResponse, EventModel } from './../../models/event';
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

  getAllEvent(userId: string) {
    return this.http.get<EventResponse>(
      environment.apiUrl + 'api/splist/getallevent',
      {
        params: {
          userId
        }
      }
    );
  }

  mapResponseDataToEvent(response: EventModel[]) {
    const result: Event[] = [];
    response.forEach(element => {
      const event: Event = {
        restaurant: element.Restaurant,
        category: element.Category,
        closeTime: new Date(element.CloseTime),
        participants: element.Participants,
        maximumBudget: element.MaximumBudget,
        hostName: element.HostName,
        hostId: element.HostId,
        name: element.Name,
        createdBy: element.CreatedBy,
        eventId: element.EventId,
        status: element.Status,
        remindTime: new Date(element.RemindTime),
        isMyEvent: element.IsMyEvent
      };
      result.push(event);
    });
    return result;
  }
}
