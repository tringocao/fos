import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { RecurrenceEvent } from "src/app/models/recurrence-event";

@Injectable({
  providedIn: "root"
})
export class RecurrenceEventService {
  constructor(
    private http: HttpClient // , private envService: EnvironmentService
  ) {
    // this.baseUrl = envService.getApiUrl() + "/api/order";
  }

  deleteRecurrenceEvent(id: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<void>>(
          environment.apiUrl + "api/RecurrenceEvent/DeleteById",
          {
            params: {
              id: JSON.stringify(id)
            }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  updateRecurrenceEvent(recurrenceEvent: RecurrenceEvent): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/RecurrenceEvent/UpdateRecurrenceEvent",
          recurrenceEvent
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  addRecurrenceEvent(recurrenceEvent: RecurrenceEvent): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/RecurrenceEvent/Add",
          recurrenceEvent
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  getById(id: number): Promise<RecurrenceEvent> {
    return new Promise<RecurrenceEvent>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<RecurrenceEvent>>(
          environment.apiUrl + "api/RecurrenceEvent/GetById",
          {
            params: {
              id: JSON.stringify(id)
            }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  getByUserId(userId: string): Promise<RecurrenceEvent> {
    return new Promise<RecurrenceEvent>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<RecurrenceEvent>>(
          environment.apiUrl + "api/RecurrenceEvent/GetByUserId",
          {
            params: {
              userId: userId
            }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
}
