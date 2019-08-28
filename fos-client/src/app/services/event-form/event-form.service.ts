import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
// import {EventUsers} from './././EventUser';
import { EventUser } from './eventuser';
import { EventUsers } from './mock-eventuser';
// import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class EventFormService {

    constructor(private http: HttpClient) { }
    getUser(): void {
        console.log("get user");
    }
    //   getUser(): Observable<Hero[]> {
    //     // this.messageService.add('HeroService: fetched heroes');
    //     // return of(HEROES);
    //   }
    getUsers(): Observable<EventUser[]> {
        // TODO: send the message _after_ fetching the heroes
        // this.messageService.add('HeroService: fetched heroes');
        return of(EventUsers);
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