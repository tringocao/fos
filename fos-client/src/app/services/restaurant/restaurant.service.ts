import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, of } from 'rxjs';
import { tap, filter } from 'rxjs/operators';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { FoodCategory } from 'src/app/models/food-category';
import { CategoryGroup } from 'src/app/models/category-group';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  ids: any;

  constructor(private http: HttpClient) {}
  setEmail(title: string): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<any>>(environment.apiUrl + 'SendEmail', {
          params: {
            eventId: title
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
  getFood(
    delivery_id: number,
    IdService: number = 1
  ): Promise<Array<FoodCategory>> {
    return new Promise<Array<FoodCategory>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<FoodCategory>>>(
          environment.apiUrl + 'GetFoodCatalogues',
          {
            params: {
              IdService: JSON.stringify(IdService),
              delivery_id: JSON.stringify(delivery_id)
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
    IdService: number = 1,
    city_id: number = 217
  ): Promise<Array<number>> {
    return new Promise<Array<number>>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<Array<number>>>(
          environment.apiUrl + 'api/Restaurant/PutCategorySearch',
          {
            categories: topic
          },
          {
            params: {
              IdService: JSON.stringify(IdService),
              city_id: JSON.stringify(city_id),
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
  getRestaurantDetail(id: number, IdService: number = 1): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<any>>(
          environment.apiUrl + 'api/Delivery/GetDeliveryDetail',
          {
            params: {
              IdService: JSON.stringify(IdService),
              delivery_id: JSON.stringify(id)
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
    cityId: number = 217,
    idService: number = 1
  ): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<any>>(
          environment.apiUrl + 'api/Delivery/GetFirstId',
          {
            params: {
              IdService: JSON.stringify(idService),
              city_Id: JSON.stringify(cityId),
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
    IdService: number = 1,
    city_id: number = 217
  ): Promise<Array<DeliveryInfos>> {
    return new Promise<Array<DeliveryInfos>>((resolve, reject) => {
      this.http
        .put<ApiOperationResult<Array<DeliveryInfos>>>(
          environment.apiUrl + 'api/Delivery/PutRestaurantIds',
          {
            restaurant_id: ids
          },
          {
            params: {
              IdService: JSON.stringify(IdService),
              city_id: JSON.stringify(city_id)
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
  GetMetadataForCategory(IdService: number = 1): Promise<Array<CategoryGroup>> {
    return new Promise<Array<CategoryGroup>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<CategoryGroup>>>(
          environment.apiUrl + 'api/Restaurant/GetMetadataForCategory',
          {
            params: {
              IdService: JSON.stringify(IdService)
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
    IdService: number = 1,
    city_id: number = 217
  ): Observable<ApiOperationResult<Array<number>>> {
    return this.http
      .get<ApiOperationResult<Array<number>>>(
        environment.apiUrl + 'api/Restaurant/GetByKeywordLimit',
        {
          params: {
            IdService: JSON.stringify(IdService),
            city_id: JSON.stringify(city_id),
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
