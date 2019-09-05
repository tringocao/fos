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

        this.http.post(environment.apiUrl + 'api/SPList/AddListItemCSOM/3a8b82cb-655b-429c-a774-9a3d2af07289',
        {
          eventTitle: eventlist.eventTitle,
          eventId: eventlist.eventId,
          eventRestaurant: eventlist.eventRestaurant,
          eventMaximumBudget: eventlist.eventMaximumBudget,
          eventTimeToClose: eventlist.eventTimeToClose,
          eventTimeToReminder: eventlist.eventTimeToReminder,
          eventHost: eventlist.eventHost,
          eventParticipants: eventlist.eventParticipants
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

    async getCurrentUser(): Promise<string> {
        var Host = "";
        console.log("get current service");
        // });
        await this.http.get(environment.apiUrl + 'api/SPUser/GetCurrentUser').toPromise().then(result => {
          console.log(result)
        });        
        
        return await Host;
    }

  //   async getUserAvatar(userId: string): Promise<string> {
  //     var avatar = "";
  //     console.log("get current service");
  //     // });
  //     await this.http.get(environment.apiUrl + 'api/SPUser/GetCurrentUser').toPromise().then(result => {
  //       console.log(result)
  //     });        
      
  //     return await Host;
  // }
}