import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
// import {EventUsers} from './././EventUser';
import { EventUser } from './eventuser';
import { EventUsers } from './mock-eventuser';
// import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {EventList} from 'src/app/models/eventList';
@Injectable({
    providedIn: 'root',
})
export class EventFormService {

    constructor(private http: HttpClient) { }
    getUser(): void {
        console.log("get user");
    }
    getUsers(): Observable<EventUser[]> {
        return of(EventUsers);
      }

      async addEventListItem(eventlist: EventList): Promise<any>{
        console.log("event list");
        console.log(eventlist);

        this.http.post(environment.apiUrl + 'api/SPList/AddListItem/3a8b82cb-655b-429c-a774-9a3d2af07289',
        {
          eventTitle: eventlist.eventTitle,
          eventId: eventlist.eventId,
          eventRestaurant: eventlist.eventRestaurant,
          eventMaximumBudget: eventlist.eventMaximumBudget,
          eventTimeToClose: eventlist.eventTimeToClose,
          eventTimeToReminder: eventlist.eventTimeToReminder,
          eventHost: eventlist.eventHost
        })
        .subscribe(
          (val) => {
            console.log("POST call successful value returned in body",
              val);
          },
          response => {
            console.log("POST call in error", response);
          },
          () => {
            console.log("The POST observable is now completed.");
          });
      }

    // async getCurrentUser(Host: string): Promise<string> {
    //     var self = this;
    //     var currentUserDisplayName = "";
    //      this.http.get('https://localhost:44398/api/SPUser/GetCurrentUser').subscribe(data => {
    //         console.log("get current User");
    //         // console.log(data.displayName);
    //         var objects = JSON.stringify(data);
    //         // console.log(objects);
    //         var jsonData = JSON.parse(objects);
    //         currentUserDisplayName = jsonData.displayName
    //         console.log(currentUserDisplayName);
    //         Host = currentUserDisplayName;
    //         // for (var i = 0; i < jsonData.value.length; i++) {

    //         //     var counter = jsonData.value[i];
    //         //     currentUserDisplayName = counter.displayName;
    //         // }
    //     });
    //     return Host;
    // }
}