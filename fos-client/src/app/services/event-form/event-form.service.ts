import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { EventList } from 'src/app/models/eventList';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class EventFormService {
  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }

  constructor(private http: HttpClient, private _snackBar: MatSnackBar) {}
  getAvatar(userId: string): Promise<object> {
    return this.http
      .get(environment.apiUrl + 'api/SPUser/GetAvatarByUserId?Id=' + userId)
      .toPromise();
  }

  async setUserInfo(
    hostPickerGroup: any,
    office365User: any,
    userPickerGroups: any,
    currentDisplayName: any,
    ownerForm: any,
    createdUser: any
  ) {
    var currentDisplay = '';

    await this.http
      .get(environment.apiUrl + 'api/SPUser/GetCurrentUser')
      .subscribe((data: any) => {
        // console.log(data.Data.displayName);
        currentDisplay = data.Data.displayName;
        console.log('get current user');
        console.log(createdUser);
        createdUser.id = data.Data.id;
        console.log(createdUser.id);
      });

    console.log('user hien tai: ' + currentDisplay);
    // ownerForm.get('host').setValue(current);

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
            if (Boolean(user.mail)) {
              // AWAIT setaAVTAR(ID)
              await this.setAvatar(
                userId,
                user.displayName,
                user.mail,
                currentPrincipalName,
                hostPickerGroup,
                office365User
              );
            }
          })
        );
        setTimeout(() => {
          this.setCurrentser(
            userPickerGroups,
            office365User,
            hostPickerGroup,
            currentDisplay,
            ownerForm
          );
        }, 5000);
      });
  }

  async setCurrentser(
    userPickerGroups: any,
    office365User: any,
    hostPickerGroup: any,
    currentDisplayName: any,
    ownerForm: any
  ) {
    userPickerGroups.push({ name: 'User', userpicker: office365User });

    console.log('load danh sach user xong');
    console.log('tim duoc host: ' + currentDisplayName);
    var selectHost = hostPickerGroup.find(c => c.name == currentDisplayName);
    console.log(selectHost);
    ownerForm.get('host').setValue(selectHost);
  }

  async setAvatar(
    userId: string,
    userDisplayName: string,
    userMail: string,
    userPrincipalName: string,
    hostPickerGroup: any,
    office365User: any
  ) {
    await this.http
      .get(environment.apiUrl + 'api/SPUser/GetAvatarByUserId?Id=' + userId)
      .subscribe((data: any) => {
        var dataImg = 'data:image/png;base64,' + data.Data;
        console.log(dataImg);

        hostPickerGroup.push({
          id: userId,
          name: userDisplayName,
          email: userMail,
          img: dataImg,
          principalName: userPrincipalName
        });
        office365User.push({
          name: userDisplayName,
          email: userMail,
          img: dataImg
        });
      });
  }

  async addEventListItem(eventlist: EventList): Promise<any> {
    console.log('event list');
    console.log(eventlist);

    this.http
      .post(
        environment.apiUrl +
          'api/SPList/AddEventListItem?Id=d7415c0c-8295-4851-bbe8-6717e939f7f6',
        {
          eventTitle: eventlist.eventTitle,
          eventRestaurant: eventlist.eventRestaurant,
          eventMaximumBudget: eventlist.eventMaximumBudget,

          eventTimeToClose: eventlist.eventTimeToClose,
          eventTimeToReminder: eventlist.eventTimeToReminder,
          eventHost: eventlist.eventHost,
          eventParticipants: eventlist.eventParticipants,

          eventCategory: eventlist.eventCategory,
          eventRestaurantId: eventlist.eventRestaurantId,
          eventServiceId: eventlist.eventServiceId,
          eventDeliveryId: eventlist.eventDeliveryId,
          eventCreatedUserId: eventlist.eventCreatedUserId,
          eventHostId: eventlist.eventHostId
        }
      )
      .subscribe(
        val => {
          this.toast('Create event successfuly ', 'Dismiss');
        },
        response => {
          console.log('POST call in error', response);
        },
        () => {
          console.log('The POST observable is now completed.');
        }
      );
  }
}
