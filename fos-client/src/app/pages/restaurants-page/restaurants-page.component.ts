import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-restaurants-page',
  templateUrl: './restaurants-page.component.html',
  styleUrls: ['./restaurants-page.component.less']
})
export class RestaurantsPageComponent implements OnInit {
  constructor(private http: HttpClient) {
    // this.http.get(environment.apiUrl + '/api/oauth/CheckAuth').subscribe((data: authRespond) => {
    //   console.log("request data");
    //   console.log(data.redirect);
    //   if (data.redirect) {
    //     console.log(data.redirectUrl);
    //     window.location.href = data.redirectUrl;
    //   }
    // }, error => {
    //     console.log(error)
    // });
    // this.http.get(environment.apiUrl + '/api/SPUser/GetUsers').subscribe(data => {
    //   console.log("request data");
    //   console.log(data);
    // });
  }

  users: any[];
  groups: any[];
  currentUser: {};

  ngOnInit() {
    var users = JSON.parse(localStorage.getItem('users'));
    var groups = JSON.parse(localStorage.getItem('groups'));
    if (!users || !groups) {
      this.users = [];
      this.groups = [];

      this.http
        .get(environment.apiUrl + '/api/SPUser/GetGroups')
        .subscribe((data: any) => {
          console.log('request data');
          var jsonData = JSON.parse(data.Data);
          console.log(jsonData);
          for (var i = 0; i < jsonData.value.length; i++) {
            var counter = jsonData.value[i];

            // console.log("check email: " + counter.displayName);
            if (counter.mail) {
              // this.dropdownListNewUser.push({ 'itemName': counter.displayName, 'id': counter.mail });
              // this.office365User.push({ name: counter.displayName, email: counter.mail });
              // this.userLogin.push({ name: counter.displayName, loginName: counter.userPrincipalName });
              this.groups.push({
                name: counter.displayName,
                email: counter.mail,
                img: ''
              });
            } else {
              // console.log("khong co email: " + counter.displayName);
            }
          }
        });

      this.http
        .get(environment.apiUrl + '/api/SPUser/GetUsers')
        .toPromise()
        .then(async (data: any) => {
          console.log('request data');
          var jsonData = JSON.parse(data.Data).value;
          console.log(jsonData);

          await Promise.all(
            jsonData.map(async user => {
              var userId = user.id;
              var currentPrincipalName = user.userPrincipalName;
              if (user.mail) {
                // console.log(user);
                await this.http
                  .get(
                    environment.apiUrl +
                      'api/SPUser/GetAvatarByUserId?Id=' +
                      userId
                  )
                  .subscribe((data: any) => {
                    var dataImg = 'data:image/png;base64,' + data.Data;
                    // console.log(dataImg);
                    this.users.push({
                      id: userId,
                      name: user.displayName,
                      email: user.mail,
                      img: dataImg,
                      principalName: user.userPrincipalName
                    });
                  });
              }
            })
          );
          setTimeout(() => {
            // console.log(this.users);
            localStorage.setItem('users', JSON.stringify(this.users));
            localStorage.setItem('groups', JSON.stringify(this.groups));
          }, 15000);
          // console.log(this.users);
        });
    }
  }
}
