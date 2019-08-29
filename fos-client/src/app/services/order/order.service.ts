import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
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

  getAllOrder() {
    return this.http.get<any>(
      environment.apiUrl +
        'api/splist/getlist/c058e7df-84d1-47f8-8741-e4e3b65fa2b1'
    );
  }
}
