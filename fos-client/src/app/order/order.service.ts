import { Injectable } from '@angular/core';
import { HttpClient } from 'selenium-webdriver/http';
import { EnvironmentService } from '../shared/service/environment.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl: string;

  constructor(private http: HttpClient, private envService: EnvironmentService) {
      this.baseUrl = envService.getApiUrl() + '/api/order';
   }

}
