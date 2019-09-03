import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {

  constructor(private http: HttpClient) {
    this.http.get(environment.apiUrl + '/api/oauth/CheckAuth').subscribe((data: authRespond) => {
      console.log("request data");
      console.log(data.redirect);
      if (data.redirect) {
        console.log(data.redirectUrl);
        window.location.href = data.redirectUrl;
      }
    }, error => {
        console.log(error)
    });

    this.http.get(environment.apiUrl + '/api/SPUser/GetAvatarById?Id=aaa7e581-b0aa-4367-879c-ef6a1745436d&size=48x48').subscribe((data:any) => {
      console.log("request data");
      console.log(data);
      var image = new Image();
      image.src = data.Data;
      document.body.appendChild(image);
    });
  }
  
  title = 'fos-client';
}
