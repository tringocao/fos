import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  appId: "theme1";
  constructor(private http: HttpClient) {
    this.appId = "theme1";

    this.http.get(environment.apiUrl + "/api/oauth/CheckAuth").subscribe(
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

    this.http
      .get(environment.apiUrl + "/api/SPUser/GetUsers")
      .subscribe(data => {
        console.log("request data");
        console.log(data);
      });
  }
  changeTheme($event) {
    this.appId = $event.theme;
  }
  title = "fos-client";
}
