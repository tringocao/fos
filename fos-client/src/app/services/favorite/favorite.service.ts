import { Injectable } from "@angular/core";
import { HttpClient, XhrFactory } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { FavoriteRestaurant } from "src/app/models/favorite-restaurant";

@Injectable({
  providedIn: "root"
})
export class FavoriteService {
  constructor(private http: HttpClient) {}

  addFavoriteRestaurant(
    restaurantId
  ): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/favorite/add",
          {
            restaurantId
          }
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
  removeFavoriteRestaurant(restaurantId: string) {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/favorite/remove",
          {
            RestaurantId: restaurantId
          }
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
  getFavorite(): Promise<Array<FavoriteRestaurant>> {
    return new Promise<Array<FavoriteRestaurant>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<FavoriteRestaurant>>>(
          environment.apiUrl + "api/favorite/GetAllByCurrentUser/"
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
