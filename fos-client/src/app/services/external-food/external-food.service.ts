import { Injectable } from "@angular/core";
import { HttpClient, XhrFactory } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { ExternalService } from 'src/app/models/external-service';
import { OauthService } from '../oauth/oauth.service';

@Injectable({
  providedIn: 'root'
})
export class ExternalFoodService {

  constructor(private http: HttpClient, private oauthService: OauthService) { }
  GetAllExternalService(): Promise<Array<ExternalService>> {
    return new Promise<Array<ExternalService>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<ExternalService>>>(environment.apiUrl + "api/External/GetAllExternalService")
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          } else reject(new Error(JSON.stringify(result.ErrorMessage)));
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }
}
