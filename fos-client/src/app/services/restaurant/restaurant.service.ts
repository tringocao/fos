import { Injectable } from "@angular/core";
import { HttpClient, XhrFactory } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";
import { tap, filter } from "rxjs/operators";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { FoodCategory } from "src/app/models/food-category";
import { CategoryGroup } from "src/app/models/category-group";
import { RestaurantDetail } from "src/app/models/restaurant-detail";
import { Restaurant } from "src/app/models/restaurant";
import { Promotion } from "src/app/models/promotion";

@Injectable({
  providedIn: "root"
})
export class RestaurantService {
  ids: any;

  constructor(private http: HttpClient) {}
  setEmail(id: string): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<any>>(environment.apiUrl + "SendEmail", {
          params: {
            eventId: id
          }
        })
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => console.log(alert));
    });
  }
  getFood(deliveryId: number, idService: number): Promise<Array<FoodCategory>> {
    return new Promise<Array<FoodCategory>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<FoodCategory>>>(
          environment.apiUrl + "GetFoodCatalogues",
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
  getRestaurantIds(
    topic: any,
    keyword: string,
    idService: number,
    cityId: number
  ): Promise<Array<number>> {
    return new Promise<Array<number>>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<Array<number>>>(
          environment.apiUrl + "api/Restaurant/PutCategorySearch",
          {
            categories: topic
          },
          {
            params: {
              idService: JSON.stringify(idService),
              cityId: JSON.stringify(cityId),
              keyword: '"' + keyword + '"'
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
  getRestaurantDetail(
    deliveryId: number,
    idService: number
  ): Promise<RestaurantDetail> {
    return new Promise<RestaurantDetail>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<RestaurantDetail>>(
          environment.apiUrl + "api/Delivery/GetDeliveryDetail",
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
  getRestaurantDetailById(
    restaurantId: number,
    cityId: number,
    idService: number
  ): Promise<DeliveryInfos> {
    return new Promise<DeliveryInfos>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<DeliveryInfos>>(
          environment.apiUrl + "api/Delivery/GetFirstId",
          {
            params: {
              idService: JSON.stringify(idService),
              cityId: JSON.stringify(cityId),
              restaurant_id: JSON.stringify(restaurantId)
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
  getRestaurants(
    ids: Array<number>,
    idService: number,
    cityId: number
  ): Promise<Array<DeliveryInfos>> {
    let ress: Restaurant[] = [];
    ids.forEach(id => {
      ress.push({ Id: id, DeliveryId: "" });
    });
    return new Promise<Array<DeliveryInfos>>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<Array<DeliveryInfos>>>(
          environment.apiUrl + "api/Delivery/PutRestaurantIds",
          ress,
          {
            params: {
              idService: JSON.stringify(idService),
              cityId: JSON.stringify(cityId)
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
  GetMetadataForCategory(idService: number = 1): Promise<Array<CategoryGroup>> {
    return new Promise<Array<CategoryGroup>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<CategoryGroup>>>(
          environment.apiUrl + "api/Restaurant/GetMetadataForCategory",
          {
            params: {
              idService: JSON.stringify(idService)
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
  getDiscountFoodIds(
    deliveryId: number,
    idService: number,
    promotion: Promotion
  ): Promise<Promotion> {
    return new Promise<Promotion>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<Promotion>>(
          environment.apiUrl + "GetDiscountedFoodIds",
          promotion,
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
  SearchRestaurantName(
    name: string,
    limit: number,
    idService: number,
    cityId: number
  ): Observable<ApiOperationResult<Array<number>>> {
    return this.http
      .get<ApiOperationResult<Array<number>>>(
        environment.apiUrl + "api/Restaurant/GetByKeywordLimit",
        {
          params: {
            idService: JSON.stringify(idService),
            cityId: JSON.stringify(cityId),
            keyword: '"' + name + '"',
            limit: JSON.stringify(limit)
          }
        }
      )
      .pipe(
        tap((response: ApiOperationResult<Array<number>>) => {
          // response.Data = response.Data
          //   .map(user =>{th

          //   })
          //   // Not filtering in the server since in-memory-web-api has somewhat restricted api
          //   .filter(user => user.name.includes(filter.name))

          return response;
        })
      );

    //   .subscribe(response => {

    // })
    // return of(dataSourceTemp);
  }
}
