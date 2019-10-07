import { Injectable } from '@angular/core';
import { EventUser } from 'src/app/models/eventuser';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { Event } from './../../../models/event';
import { GraphUser } from 'src/app/models/graph-user';

@Injectable({
  providedIn: 'root'
})
export class EventFormValidationService {
  constructor() {}

  CheckEventChangeRestaurant(
    currentDataEvent: Event,
    changeRestaurant: DeliveryInfos,
  ): Boolean {
    var checkRestaurant: Boolean = false;
    if (currentDataEvent.RestaurantId !== changeRestaurant.RestaurantId
      && currentDataEvent.DeliveryId !== changeRestaurant.DeliveryId) {
      checkRestaurant = true;
    }
    return checkRestaurant;
  }

  GetNewParticipants(
    currentDataEvent: Event,
    changeUsers: GraphUser[]
  ): Array<GraphUser> {
    var participants = JSON.parse(currentDataEvent.EventParticipantsJson);
    var newUser: GraphUser[] = [];
    changeUsers.forEach(element => {
      var user: GraphUser[] = participants.filter(
        item => item.Mail === element.Mail
      );
      if(user.length == 0){
        newUser.push(element);
      }
    });
    return newUser;
  }

  GetRemoveParticipants(
    currentDataEvent: Event,
    changeUsers: GraphUser[]
  ): Array<GraphUser> {
    var participants: GraphUser[] = JSON.parse(currentDataEvent.EventParticipantsJson);
    var removeList: GraphUser[] = [];
    participants.forEach(element => {
      var removeUser: GraphUser[] = changeUsers.filter(u => u.Mail === element.Mail);
      if(removeUser.length == 0){
        removeList.push(element);
      }
    });
    return removeList;
  }
}
