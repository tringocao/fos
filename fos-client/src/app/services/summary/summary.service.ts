import { Injectable } from "@angular/core";
import { HttpClient, XhrFactory } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Report } from "src/app/models/report";
import { Event } from "src/app/models/event";
import { RestaurantSummary } from "src/app/models/restaurant-summary";
import { DishesSummary } from "./../../models/dishes-summary";

@Injectable({
  providedIn: "root"
})
export class SummaryService {
  constructor(private http: HttpClient) {}

  sendEmail(report: Report): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      console.log(report);
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/summary/sendreport",
          report
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

  addReport(
    eventId: string,
    reportUrl: string,
    content: string
  ): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/summary/addreport",
          {
            Name: eventId,
            Content: content
          },
          {
            params: {
              eventId,
              reportUrl
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
  GetRestaurantSummary() {
    return new Promise<RestaurantSummary[]>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<RestaurantSummary[]>>(
          environment.apiUrl + "api/summary/GetRestaurantSummary"
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
  getDishesSummary(
    restaurantId: string,
    deliveryId: string,
    serviceId: string
  ): Promise<DishesSummary[]> {
    return new Promise<DishesSummary[]>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<DishesSummary[]>>(
          environment.apiUrl + "api/summary/GetDishesSummary",
          {
            params: {
              restaurantId,
              deliveryId,
              serviceId
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

  updateEventStatus(
    eventId: string,
    status: string
  ): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl +
            "api/splist/UpdateEventStatus?Id=" +
            eventId +
            "&eventStatus=" +
            status,
          {}
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(null);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  SetTime2CloseToEventDate(eventId: string): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl +
            "api/splist/SetTime2CloseToEventDate?Id=" +
            eventId,
          {}
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(null);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
}
