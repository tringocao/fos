import { Injectable } from '@angular/core';
import { CustomGroup } from './../../models/custom-group';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { OauthService } from '../oauth/oauth.service';

@Injectable({
  providedIn: 'root'
})
export class CustomGroupService {
  constructor(private http: HttpClient, private oauthService: OauthService) {}

  getAllGroup(ownerId: string): Promise<Array<CustomGroup>> {
    return new Promise<Array<CustomGroup>>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<Array<CustomGroup>>>(
          environment.apiUrl + 'api/CustomGroup/GetAllGroup',
          {
            params: {
              ownerId
            }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }

  deleteGroupById(groupId: string) {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .delete<ApiOperationResult<void>>(
          environment.apiUrl + 'api/CustomGroup/DeleteGroupById',
          {
            params: { groupId }
          }
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(null);
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }

  createGroup(customGroup: CustomGroup): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<ApiOperationResult<void>>>(
          environment.apiUrl + 'api/CustomGroup/CreateGroup',
          customGroup
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }

  updateGroup(customGroup: CustomGroup): Promise<ApiOperationResult<void>> {
    return new Promise<ApiOperationResult<void>>((resolve, reject) => {
      this.http
        .post<ApiOperationResult<ApiOperationResult<void>>>(
          environment.apiUrl + 'api/CustomGroup/UpdateGroup',
          customGroup
        )
        .toPromise()
        .then(result => {
          if (result.Success) {
            resolve(result.Data);
          }
        })
        .catch(alert => this.oauthService.checkAuthError(alert));
    });
  }
}
