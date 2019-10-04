import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';
import { FeedBack } from 'src/app/models/feed-back';
import { environment } from 'src/environments/environment';
import { OauthService } from '../oauth/oauth.service';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  constructor(private http: HttpClient, private oauthService: OauthService) {}

  getFeedbackById(id: string): Promise<FeedBack> {
    return new Promise<FeedBack>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<FeedBack>>(
          environment.apiUrl + 'api/feedback/GetById/' + id
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

  feedBackEvent(feedback: FeedBack): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<void>>(
          environment.apiUrl + 'api/feedback/rate',
          feedback
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve();
          } else {
            reject(new Error(JSON.stringify(result.ErrorMessage)));
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }

  sendFeedbackEmail(eventId: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<void>>(
          environment.apiUrl + 'api/feedback/sendEmail/' + eventId
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve();
          } else {
            reject(result.ErrorMessage);
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }
}
