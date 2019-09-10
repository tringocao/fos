import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Event } from './../../models/event';
// import { EnvironmentService } from "../shared/service/environment.service";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl: string;

  constructor(
    private http: HttpClient // , private envService: EnvironmentService
  ) {
    // this.baseUrl = envService.getApiUrl() + "/api/order";
  }

  getAllEvent(userId: string) {
    return this.http.get<any>(environment.apiUrl + 'api/splist/getallevent', {
      params: {
        userId
      }
    });
  }
}
