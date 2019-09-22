import { Injectable } from '@angular/core';
import { EventUser } from 'src/app/models/eventuser';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { Event } from './../../../models/event';

@Injectable({
  providedIn: 'root'
})
export class EventFormValidationService {
  constructor() {}

  CheckEventChangeRestaurant(
    currentDataEvent: Event,
    currentRestaurant: DeliveryInfos,
    newtUsers: EventUser
  ): Boolean {
    if (currentDataEvent.RestaurantId !== currentRestaurant.RestaurantId) {
      return false;
    }
  }
  CheckEventChangeParticipants(
    currentDataEvent: Event,
    changeUsers: EventUser[]
  ) {
    // currentDataEvent.EventParticipantsJson.forEach(element => {
    // });
    // var findUser = changeUsers.find(u => u.Id === currentDataEvent.)
  }
}
