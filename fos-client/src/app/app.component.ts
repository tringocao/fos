import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { ActivatedRoute } from "@angular/router";
import { OauthService } from './services/oauth/oauth.service';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  appId: string;
  idOrder: any;
  showIcon: boolean;
  constructor(private oauthService: OauthService, private route: ActivatedRoute) {
    this.appId = "theme1";
    this.showIcon = this.route.snapshot.paramMap.get("id") == null;
    this.oauthService.checkOauth();
  }
  changeTheme($event) {
    this.appId = $event.theme;
  }
  title = "fos-client";
}
