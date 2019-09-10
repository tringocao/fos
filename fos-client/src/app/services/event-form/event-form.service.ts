import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { EventList } from 'src/app/models/eventList';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { switchMap, debounceTime, tap, finalize } from 'rxjs/operators';
import { User } from 'src/app/models/user';
import { GraphUser } from 'src/app/models/graph-user';
@Injectable({
  providedIn: 'root'
})
export class EventFormService {

  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }

  constructor(private http: HttpClient, private _snackBar: MatSnackBar) {}

  // GetAvatarByUserId(userId: string): Observable<ApiOperationResult<any>>{
  //     return this.http
  //     .get(
  //       environment.apiUrl + 'api/SPUser/GetAvatarByUserId',
  //       {
  //         params: {
  //           userId: userId
  //         }
  //       }
  //     )
  //     .pipe(
  //       tap((response: ApiOperationResult<Array<any>>) => {
  //         return response;
  //       })
  //     );
  // }

  AddEventListItem(eventlist: EventList): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post(
          environment.apiUrl +
          'api/SPList/AddEventListItem?Id=d7415c0c-8295-4851-bbe8-6717e939f7f6',
          {
            EventTitle: eventlist.EventTitle,
            EventRestaurant: eventlist.EventRestaurant,
            EventMaximumBudget: eventlist.EventMaximumBudget,

            EventTimeToClose: eventlist.EventTimeToClose,
            EventTimeToReminder: eventlist.EventTimeToReminder,
            EventHost: eventlist.EventHost,
            EventParticipants: eventlist.EventParticipants,

            EventCategory: eventlist.EventCategory,
            EventRestaurantId: eventlist.EventRestaurantId,
            EventServiceId: eventlist.EventServiceId,
            EventDeliveryId: eventlist.EventDeliveryId,
            EventCreatedUserId: eventlist.EventCreatedUserId,
            EventHostId: eventlist.EventHostId,
            EventDate: eventlist.EventDate,
            EventParticipantsJson: eventlist.EventParticipantsJson
          }
        )
        .subscribe(
          val => {
            this.toast('Create event successfuly ', 'Dismiss');
          },
          response => {
            console.log('POST call in error', response);
          },
          () => {
            console.log('The POST observable is now completed.');
          }
        );
    })
  }

  SearchUserByName(searchText: string): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + 'api/SPUser/searchUserByName',
        {
          params: {
            searchText: searchText
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  getCurrentUser(): Observable<ApiOperationResult<GraphUser>> {
    return this.http
      .get<ApiOperationResult<GraphUser>>(
        environment.apiUrl + 'api/SPUser/GetCurrentUser'
      )
      .pipe(
        tap((response: ApiOperationResult<GraphUser>) => {
          return response;
        })
      );
  }

  GetMemberInGroups(groupId: string): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + '/api/SPUser/GetMemberInGroups',
        {
          params: {
            groupId: groupId
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  GetUsersByName(searchName: string): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + '/api/SPUser/GetUsersByName',
        {
          params: {
            searchName: searchName
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  GetGroups(): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + '/api/SPUser/GetGroups'
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  GetUsers(): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + '/api/SPUser/GetUsers'
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  GroupListMemers(groupId: string): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + '/api/SPUser/GroupListMemers',
        {
          params: {
            groupId: groupId
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }
}
