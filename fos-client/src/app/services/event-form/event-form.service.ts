import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
// import {EventUsers} from './././EventUser';
import { EventUser } from './eventuser';
import { EventUsers } from './mock-eventuser';
// import { MessageService } from './message.service';

@Injectable({
    providedIn: 'root',
})
export class EventFormService {

    constructor() { }
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
}