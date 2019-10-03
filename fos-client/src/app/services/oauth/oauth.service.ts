import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OauthService {

  constructor(private http: HttpClient) { }

  checkOauth() {
    this.http.get(environment.apiUrl + "/api/oauth/CheckAuth", {
      params: {
        redirectUrl: window.location.href
      }
    }).subscribe(
      (data: authRespond) => {
        console.log("request data");
        console.log(data.redirect);
        if (data.redirect) {
          console.log(data.redirectUrl);
          window.location.href = data.redirectUrl;
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  logOut() {
    this.http.get(environment.apiUrl + "/api/oauth/logout").subscribe(
      (data: authRespond) => {
        console.log("request data");
        console.log(data.redirect);
        if (data.redirect) {
          console.log(data.redirectUrl);
          window.location.href = data.redirectUrl;
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  checkAuthError(response) {
    if (response && response.error.Error === 401) {
      alert('Your session has expired. Please refresh the page and try again.');
    }
    console.log(response);
  }
}
