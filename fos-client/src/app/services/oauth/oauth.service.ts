import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class OauthService {

  constructor(private http: HttpClient, private _snackBar: MatSnackBar) { }

  checkOauth(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http
        .get<ApiOperationResult<boolean>>(environment.apiUrl + "/api/oauth/CheckAuth", {
          params: {
            redirectUrl: window.location.href
          }
        })
        .toPromise()
        .then((result: any) => {
          //console.log(result);
          if (result) {
            resolve(result);
          } else {
            reject(new Error(JSON.stringify(result.ErrorMessage)));
          }
        })
        .catch(alert => this.checkAuthError(alert));
    });
  }

  logOut() {
    this.http.get(environment.apiUrl + "/api/oauth/logout").subscribe(
      (data: authRespond) => {
        //console.log("request data");
        //console.log(data.redirect);
        if (data.redirect) {
          //console.log(data.redirectUrl);
          window.location.href = data.redirectUrl;
        }
      },
      error => {
        //console.log(error);
      }
    );
  }

  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 100000
    });
  }

  checkAuthError(response) {
    if (response && response.error.Error === 401) {
      this.toast('Your session has expired. Please refresh the page and try again.', 'Dismiss');
      this.checkOauth().then((result: any) => {
        if (result.redirect) {
          window.location.href = result.redirectUrl;
        }
      });
    }
    //console.log(response);
  }
}
