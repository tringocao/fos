import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  appId: string;
  idOrder: any;
  showIcon: boolean;
  constructor(private http: HttpClient, private route: ActivatedRoute) {
    this.appId = "theme1";
    this.showIcon = this.route.snapshot.paramMap.get("id") == null;
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
  }
  changeTheme($event) {
    this.appId = $event.theme;
  }
  title = "fos-client";
}
