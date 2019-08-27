import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-restaurants-page',
  templateUrl: './restaurants-page.component.html',
  styleUrls: ['./restaurants-page.component.less']
})
export class RestaurantsPageComponent implements OnInit {

  constructor(private http: HttpClient) {
    this.http.get('https://localhost:44366/api/oauth/CheckAuth').subscribe((data: authRespond) => {
      console.log("request data");
      console.log(data.redirect);
      if (data.redirect) {
        console.log(data.redirectUrl);
        window.location.href = data.redirectUrl;
      }
    }, error => {
        console.log(error)
    });
  }

  ngOnInit() {
  }

}
