import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class FavoriteService {

  constructor(private http: HttpClient) { }

  addFavoriteRestaurant(userId:string, restaurantId): Promise<ApiOperationResult<any>> {
      return new Promise<ApiOperationResult<any>>((resolve,reject)=>{
        this.http.post<ApiOperationResult<any>>(environment.apiUrl + 'api/favorite/add', 
        {
          userId,
          restaurantId
        }
      )
      .toPromise().then(result => {
        if(result.Success){resolve(result.Data)}
        else reject(new Error(JSON.stringify(result.ErrorMessage)));        
      }).catch(alert => console.log(alert))
    });
  }
  removeFavoriteRestaurant(userId: any, restaurantId: any) {
    return new Promise<ApiOperationResult<any>>((resolve,reject)=>{
      this.http.post<ApiOperationResult<any>>(environment.apiUrl + 'api/favorite/remove', 
        {
          UserId: userId,
          RestaurantId: restaurantId,
        }
      )
      .toPromise().then(result => {
        if(result.Success){resolve(result.Data)}
        else reject(new Error(JSON.stringify(result.ErrorMessage)));        
      }).catch(alert => console.log(alert))
    });
  }
  getFavorite(userId: any): Promise<Array<FavoriteRestaurant>> {
    return new Promise<Array<FavoriteRestaurant>>((resolve,reject)=> {
      this.http.get<ApiOperationResult<Array<FavoriteRestaurant>>>(environment.apiUrl + 'api/favorite/GetAllById/' + userId)
      .toPromise().then(result => {
      if(result.Success){resolve(result.Data)}
      else reject(new Error(JSON.stringify(result.ErrorMessage)));        
    }).catch(alert => console.log(alert))
  });
  }
}
