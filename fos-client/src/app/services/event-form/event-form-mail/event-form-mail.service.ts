import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GraphUser } from 'src/app/models/graph-user';
import { UpdateEvent } from 'src/app/models/update-event';
import { EventUser } from 'src/app/models/event-user';

@Injectable({
  providedIn: 'root'
})
export class EventFormMailService {
  constructor(private http: HttpClient) {}

  SendMailUpdateEvent(updateEvent: UpdateEvent): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + 'SendMailUpdateEvent',
          updateEvent
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result);
          } else {
            reject(result.ErrorMessage);
          }
        })
        .catch(alert => console.log(alert));
    });
  }
  SendCancelEventMail(users: Array<EventUser>): Promise<void> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + 'SendCancelEventMail',
          users
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result);
          } else {
            reject(result.ErrorMessage);
          }
        })
        .catch(alert => console.log(alert));
    });
  }
}
