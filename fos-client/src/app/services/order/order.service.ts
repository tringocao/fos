import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Event } from "./../../models/event";
import { Order } from "src/app/models/order";
// import { EnvironmentService } from "../shared/service/environment.service";

@Injectable({
  providedIn: "root"
})
export class OrderService {
  private baseUrl: string;

  constructor(
    private http: HttpClient // , private envService: EnvironmentService
  ) {
    // this.baseUrl = envService.getApiUrl() + "/api/order";
  }

  getAllEvent(userId: string) {
    return this.http.get<any>(environment.apiUrl + "api/splist/getallevent", {
      params: {
        userId
      }
    });
  }
  SetOrder(order: Order): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/Order/UpDateOrder",
          order
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
  GetOrder(orderId: string): Promise<Order> {
    return new Promise<Order>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Order>>(
          environment.apiUrl + "api/Order/GetById",
          {
            params: {
              orderId: orderId
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
