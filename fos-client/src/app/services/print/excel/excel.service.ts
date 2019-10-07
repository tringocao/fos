import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { UserOrder } from "src/app/models/user-order";
import { ExcelModel } from "src/app/models/excel-model";
import { OauthService } from "../../oauth/oauth.service";
@Injectable({
  providedIn: "root"
})
export class ExcelService {
  constructor(private http: HttpClient, private oauthService: OauthService) {}
  CreateCSV(excelModel: ExcelModel): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + "api/Excel/CreateCSV/",
          excelModel
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Success);
          } else {
            reject(new Error(JSON.stringify(result.ErrorMessage)));
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }
  DownloadCSV(eventId: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<void>>(
          environment.apiUrl +
            "api/Excel/DownloadCSV/eventId?=" +
            eventId.toString()
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else {
            reject(new Error(JSON.stringify(result.ErrorMessage)));
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }
}
