import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EventFormService } from 'src/app/services/event-form/event-form.service';

@Component({
  selector: 'app-restaurants-page',
  templateUrl: './restaurants-page.component.html',
  styleUrls: ['./restaurants-page.component.less']
})
export class RestaurantsPageComponent implements OnInit {
  constructor(private http: HttpClient, private eventFormServivce: EventFormService) {
    // this.http.get(environment.apiUrl + '/api/oauth/CheckAuth').subscribe((data: authRespond) => {
    //   //console.log("request data");
    //   //console.log(data.redirect);
    //   if (data.redirect) {
    //     //console.log(data.redirectUrl);
    //     window.location.href = data.redirectUrl;
    //   }
    // }, error => {
    //     //console.log(error)
    // });
    // this.http.get(environment.apiUrl + '/api/SPUser/GetUsers').subscribe(data => {
    //   //console.log("request data");
    //   //console.log(data);
    // });
  }

  ngOnInit() {
  }
}
