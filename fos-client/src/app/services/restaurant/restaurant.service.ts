import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  constructor(private http: HttpClient) {}

  getRestaurant() {
    const param = [
      '595',
      '42888',
      '90677',
      '152809',
      '110335',
      '900',
      '47168',
      '4336',
      '90018',
      '96530'
    ];

    return this.http.post<any>(
      'https://gappapi.deliverynow.vn/api/delivery/get_infos',
      {
        restaurant_ids: [595]
      },
      {
        headers: {
          'x-foody-api-version': '1',
          'x-foody-app-type': '1004',
          'x-foody-access-token': '',
          'x-foody-client-id': '',
          'x-foody-client-language': 'vi',
          'x-foody-client-type': '1',
          'x-foody-client-version': '3.0.0',
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Methods': '*',
          'Access-Control-Allow-Headers':
            'Content-Type, Authorization, Content-Length, X-Requested-With, Accept'
        }
      }
    );
  }
}
