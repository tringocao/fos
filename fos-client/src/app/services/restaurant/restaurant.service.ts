import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  ids: any;

  constructor(private http: HttpClient) {}
  getFood(delivery_id: any) {
    return this.http.get<any>(environment.apiUrl + 'GetFoodCatalogues', 
     {
      params: {
        IdService: '1',
        delivery_id: delivery_id,
      }
    })
    }
  getRestaurantIds(topic: any, keyword:string) {
    return this.http.put<any>(environment.apiUrl + 'api/Restaurant/PutCategorySearch', 
    {
      categories: topic
    },
     {
      params: {
        IdService: '1',
        city_id: '217',
        keyword: "\"" + keyword + "\""
      }
    });
  }
  getRestaurants(ids: any) {
    return this.http.put<any>(
      environment.apiUrl + 'api/Delivery/PutRestaurantIds',
      {
        restaurant_ids: ids
      },
      {
        params: {
          IdService: '1',
          city_id: '217'
        }
      }
    );
  }
  GetMetadataForCategory() {
    return this.http.get<any>(
      environment.apiUrl + 'api/Restaurant/GetMetadataForCategory', {
        params: {
          IdService: '1'
        }
      }
    );
  }
  SearchRestaurantName(name: any, limit: string) {
    const dataSourceTemp = [];

    this.http.get<any>(environment.apiUrl + 'api/Restaurant/GetByKeywordLimit', {
      params: {
        IdService: '1',
        city_id: '217',
        keyword: "\"" + name + "\"",
        limit: limit
      }
    }).subscribe(response => {
      this.getRestaurants(response).subscribe(result => {
        const jsonData = JSON.parse(result);
        jsonData.forEach((element, index) => {
          // tslint:disable-next-line:prefer-const
          let restaurantItem: Restaurant = {
            name: element.name,
            delivery_id: element.delivery_id,
            address: element.address,
            category:
              element.categories.length > 0 ? element.categories[0] : '',
            promotion:
              element.promotion_groups.length > 0
                ? element.promotion_groups[0].text
                : '',
            open:
              element.operating.open_time + '-' + element.operating.close_time,
              url_rewrite_name: element.url_rewrite_name
          };
          dataSourceTemp.push(restaurantItem);
        });
      });
  })
  return of(dataSourceTemp);
  }
  addFavoriteRestaurant(userId: any, restaurantId: any) {
    return this.http.post<any>(environment.apiUrl + 'api/favorite/add', {
      UserId: userId,
      RestaurantId: restaurantId
    });
  }
  removeFavoriteRestaurant(userId: any, restaurantId: any) {
    return this.http.post<any>(environment.apiUrl + 'api/favorite/remove', {
      UserId: userId,
      RestaurantId: restaurantId
    });
  }
  getFavorite(userId: any) {
    return this.http.get<any>(
      environment.apiUrl + 'api/favorite/GetAllById/' + userId
    );
  }
  getCurrentUserId() {
    return this.http.get<any>(environment.apiUrl + 'api/spuser/GetCurrentUser');
  }
  
}
