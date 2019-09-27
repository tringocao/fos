import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Observable } from "rxjs";
import { switchMap, debounceTime, tap, finalize } from "rxjs/operators";
import { User } from "src/app/models/user";
import { GraphUser } from "src/app/models/graph-user";
import { Event } from "src/app/models/event";
import { Group } from 'src/app/models/group';

@Injectable({
  providedIn: "root"
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

  AddEventListItem(
    eventlist: Event
  ): Observable<ApiOperationResult<string>> {
    return this.http
      .post(
        environment.apiUrl +
          "api/SPList/AddEventListItem?Id=d7415c0c-8295-4851-bbe8-6717e939f7f6",
          eventlist
      )
      .pipe(
        tap((response: ApiOperationResult<string>) => {
          return response;
        })
      );
  }

  UpdateEventListItem(
    Id: String,
    eventlist: Event
  ): Observable<ApiOperationResult<void>> {
    return this.http
      .post(environment.apiUrl + "api/SPList/UpdateListItem?Id=" + Id, eventlist)
      .pipe(
        tap((response: ApiOperationResult<void>) => {
          return response;
        })
      );
  }

  UpdateListItemWhenRestaurantChanges(
    Id: String,
    eventlist: Event
  ): Observable<ApiOperationResult<void>> {
    return this.http
      .post(environment.apiUrl + "api/SPList/UpdateListItemWhenRestaurantChanges?Id=" + Id, eventlist)
      .pipe(
        tap((response: ApiOperationResult<void>) => {
          return response;
        })
      );
  }

  SearchUserByName(
    searchText: string
  ): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + "api/SPUser/searchUserByName",
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
        environment.apiUrl + "api/SPUser/GetCurrentUserGraph"
      )
      .pipe(
        tap((response: ApiOperationResult<GraphUser>) => {
          return response;
        })
      );
  }

  GetMemberInGroups(
    groupId: string
  ): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + "/api/SPUser/GetMemberInGroups",
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

  GetUsersByName(
    searchName: string
  ): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + "/api/SPUser/GetUsersByName",
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
        environment.apiUrl + "/api/SPUser/GetGroups"
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
        environment.apiUrl + "/api/SPUser/GetUsers"
      )
      .pipe(
        tap((response: ApiOperationResult<Array<User>>) => {
          return response;
        })
      );
  }

  GroupListMemers(
    groupId: string
  ): Observable<ApiOperationResult<Array<User>>> {
    return this.http
      .get<ApiOperationResult<Array<User>>>(
        environment.apiUrl + "/api/SPUser/GroupListMemers",
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
  GetEventById(id: string): Promise<Event> {
    return new Promise<Event>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Event>>(
          environment.apiUrl + "api/splist/GetEventById",
          {
            params: {
              id: id
            }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          }
        })
        .catch(alert => console.log(alert));
    });
  }

  SearchGroupOrUserByName(
    searchName: string
  ): Observable<ApiOperationResult<Array<Group>>> {
    return this.http
      .get<ApiOperationResult<Array<Group>>>(
        environment.apiUrl + "api/SPUser/SearchGroupOrUserByName",
        {
          params: {
            searchName: searchName
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<Group>>) => {
          return response;
        })
      );
  }
}
