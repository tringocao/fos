import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from "@angular/common/http";
import { Promotion } from 'src/app/models/promotion';
import { environment } from "src/environments/environment";
import { EventPromotion } from 'src/app/models/event-promotion';

@Injectable({
  providedIn: 'root'
})
export class EventPromotionService {

  constructor(private http: HttpClient) { }
  getPromotionsByExternalService(
    deliveryId: number,
    idService: number
  ): Promise<Array<Promotion>> {
    return new Promise<Array<Promotion>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<Promotion>>>(
          environment.apiUrl + "api/EventPromotion/GetPromotionsByExternalService",
          {
            params: {
              idService: JSON.stringify(idService),
              deliveryId: JSON.stringify(deliveryId)
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
  GetByEventId(
    eventId: number
  ): Promise<EventPromotion> {
    return new Promise<EventPromotion>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<EventPromotion>>(
          environment.apiUrl + "api/EventPromotion/GetByEventId",
          {
            params: {
              eventId: JSON.stringify(eventId)
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
  UpdateEventPromotionByEventId(
    eventId: number,  newPromos:Array<Promotion>
  ): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<void>>(
          environment.apiUrl + "api/EventPromotion/UpdateEventPromotionByEventId",
          newPromos,
          {
            params: {
              eventId: JSON.stringify(eventId)
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
  AddEventPromotion(
    eventId: number,  newPromos:Array<Promotion>
  ): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/EventPromotion/AddEventPromotion",
          newPromos,
          {
            params: {
              eventId: JSON.stringify(eventId)
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
  UpdateEventPromotion(
    eventPromo: EventPromotion
  ): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/EventPromotion/UpdateEventPromotion",
          eventPromo
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
