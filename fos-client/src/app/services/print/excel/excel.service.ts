import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserOrder } from 'src/app/models/user-order';
@Injectable({
  providedIn: 'root'
})
export class ExcelService {

  constructor(private http: HttpClient) { }
  CreateCSV(userOrder: Array<UserOrder>): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + 'api/Excel/CreateCSV/',
          userOrder
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Success);
          } else { reject(new Error(JSON.stringify(result.ErrorMessage))); }
        })
        .catch(alert => console.log(alert));
    });
  }
  DownloadCSV(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<void>>(
          environment.apiUrl + 'api/Excel/DownloadCSV/',
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else { reject(new Error(JSON.stringify(result.ErrorMessage))); }
        })
        .catch(alert => console.log(alert));
    });
  }
}
