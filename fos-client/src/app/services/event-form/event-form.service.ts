import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class EventFormService {

  constructor(private http: HttpClient) { }
  getAvatar(userId: string): Promise<object> {
    // var img = "";
    return this.http.get(environment.apiUrl + 'api/SPUser/GetAvatarByUserId?Id=' + userId).toPromise();
    // return await img;
  }

// async getCurrentUSer() (Promise: any){
  
// }

  async setUserInfo(hostPickerGroup: any, office365User: any, userPickerGroups: any, currentDisplayName: any, ownerForm: any) {
    
    var current = "";

    await this.http.get(environment.apiUrl + 'api/SPUser/GetCurrentUser').subscribe((data: any) => {
      console.log(data.Data.displayName);
      current = data.Data.displayName;
    });

    console.log("user hien tai: " +current);
    // ownerForm.get('host').setValue(current);

    this.http.get(environment.apiUrl + '/api/SPUser/GetUsers').toPromise().then(async (data: any) => {
      console.log("request data");
      var jsonData = JSON.parse(data.Data).value;
      console.log(jsonData);
      await Promise.all(jsonData.map(async (user) => {

        if (Boolean(user.mail)) {
          // AWAIT setaAVTAR(ID)
          await this.setAvatar(user.id,user.displayName,user.mail,hostPickerGroup, office365User);
        }

      }));
      setTimeout(() => {
        this.setCurrentser(userPickerGroups, office365User, hostPickerGroup, current, ownerForm);
      }, 5000);
      
    });
  }

  async setCurrentser(userPickerGroups: any, office365User: any, hostPickerGroup: any, currentDisplayName: any, ownerForm: any) {
    userPickerGroups.push({ name: 'User', userpicker: office365User });

    console.log("load danh sach user xong");
    console.log('tim duoc host: '+ currentDisplayName);
    var selectHost = hostPickerGroup.find(c => c.name == currentDisplayName);
    console.log(selectHost);
    ownerForm.get('host').setValue(selectHost);
  }
  
  async setAvatar(userId: string, userDisplayName: string, userMail: string , hostPickerGroup: any, office365User: any) {
    await this.http.get(environment.apiUrl + 'api/SPUser/GetAvatarByUserId?Id=' + userId).subscribe((data: any) => {
      var dataImg = "data:image/png;base64," + data.Data;
      console.log(dataImg);

      hostPickerGroup.push({ name: userDisplayName, email: userMail, img: dataImg});
      office365User.push({ name: userDisplayName, email: userMail, img: dataImg});
    });
  }

  //"data:image/png;base64,"
}
