import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { ActivatedRoute } from "@angular/router";
import { OauthService } from "./services/oauth/oauth.service";
import { PrintService } from "./services/print/print.service";
import { DataRoutingService } from "./data-routing.service";
import { Subscription } from "rxjs";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  appId: string;
  idOrder: any;
  showIcon: boolean;
  isAuthenticated = false;

  constructor(
    private oauthService: OauthService,
    private route: ActivatedRoute,
    private printService: PrintService,
    private _dataService: DataRoutingService
  ) {
    this.appId = "theme1";
    this.showIcon = this.route.snapshot.paramMap.get("id") == null;
    this.oauthService.checkOauth().then((result: any) => {
      if (result.redirect) {
        window.location.href = result.redirectUrl;
      } else {
        this.isAuthenticated = true;
      }
    });
  }
  changeTheme($event) {
    this.appId = $event.theme;
    this._dataService.setNavTitle(this.appId);

    // this.getNavTitleSubscription = this._dataService.getNavTitle()
    // .subscribe((appId: string) => this.appId = appId);
  }
  title = "fos-client";
}
