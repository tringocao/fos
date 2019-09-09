import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { Observable, Observer } from 'rxjs';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable
} from '@angular/material';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder
} from '@angular/forms';
import { EventUser } from '../../models/eventuser';
import { EventFormService } from '../../services/event-form/event-form.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EventList } from 'src/app/models/eventList';
import { debounceTime, tap, switchMap, finalize } from 'rxjs/operators';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';

interface Restaurant {
  id: string;
  stared: boolean;
  restaurant: string;
  category: string;
  address: string;
  promotion: string;
  open: string;
  delivery_id: string;
  url_rewrite_name: string;
}

export interface OwnerForCreation {
  title: string;
  host: string;
  dateTimeToClose: string;
  participants: string;
  restaurant: string;
}

interface SelectItem {
  label: string;
  value: any;
}

export interface userPicker {
  name: string;
  email: string;
  img: string;
}

export interface userPickerGroup {
  name: string;
  userpicker: userPicker[];
}
@Component({
  selector: 'app-event-dialog',
  templateUrl: './event-dialog.component.html',
  styleUrls: ['./event-dialog.component.less']
})
export class EventDialogComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  public ownerForm: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Restaurant,
    private fb: FormBuilder,
    private eventFormService: EventFormService,
    private http: HttpClient,
    private restaurantService: RestaurantService
  ) {}

  createdUser = { id: '' };
  dateTimeToClose: string;
  dateToReminder: string;
  maximumBudget: number;
  userSelect = [];
  userPickerGroups: userPickerGroup[] = [
    // {
    //   name: 'Office 365 group',
    //   userpicker: [
    //     { name: 'Group1', email: 'group1@gmail.com' },
    //     { name: 'Group2', email: 'group2@gmail.com' },
    //     { name: 'Group3', email: 'group3@gmail.com' }
    //   ]
    // },
    // {
    //   name: 'User',
    //   userpicker: [
    //     { name: 'user1', email: 'user1@gmail.com' },
    //     { name: 'user2', email: 'user2@gmail.com' },
    //     { name: 'user3', email: 'user3@gmail.com' }
    //   ]
    // }
  ];
  private toDateString(date: Date): string {
    return (
      date.getFullYear().toString() +
      '-' +
      ('0' + (date.getMonth() + 1)).slice(-2) +
      '-' +
      ('0' + date.getDate()).slice(-2) +
      'T' +
      date.toTimeString().slice(0, 5)
    );
  }

  eventusers: EventUser[] = [
    // { name: 'Salah', email: 'Liverpool' },
    // { name: 'Kane', email: 'Tottenham Hospur' },
    // { name: 'Hazard', email: 'Real Madrid' },
    // { name: 'Griezmann', email: 'Barcelona' }
  ];
  public restaurantPickerGroup = [
    {
      id: 'id1',
      name: 'restaurant 1'
    },
    {
      id: 'id2',
      name: 'restaurant 2'
    },
    {
      id: 'id3',
      name: 'restaurant 3'
    }
  ];

  hostPickerGroup = [
    // {
    //   name: 'name 1',
    //   email: 'description 1',
    // },
    // {
    //   name: 'name 2',
    //   email: 'description 2'
    // },
    // {
    //   name: 'name 3',
    //   email: 'description 3'
    // }
  ];

  displayedColumns = ['avatar', 'name', 'email', 'action'];
  isLoading = false;
  restaurant$: Restaurant[];

  office365User: userPicker[] = [];
  office365Group: userPicker[] = [];
  displayFn(user: Restaurant) {
    if (user) {
      return user.restaurant;
    }
  }
  ngOnInit() {
    var users = JSON.parse(localStorage.getItem('users'));
    console.log(users);

    users.map(user => {
      this.hostPickerGroup.push({
        id: user.id,
        name: user.name,
        email: user.email,
        img: user.img,
        principalName: user.principalName
      });
      this.office365User.push({
        name: user.name,
        email: user.email,
        img: user.img
      });
    });
    this.userPickerGroups.push({
      name: 'User',
      userpicker: this.office365User
    });
    console.log(this.hostPickerGroup);
    console.log(this.office365User);

    // this.office365User.push({ name: userDisplayName, email: userMail, img: dataImg});

    this.dateTimeToClose = this.toDateString(new Date());
    this.dateToReminder = this.toDateString(new Date());
    this.maximumBudget = 0;

    // -----
    this.ownerForm = new FormGroup({
      title: new FormControl('', [
        Validators.required
        // Validators.maxLength(60)
      ]),
      dateOfBirth: new FormControl(new Date()),
      address: new FormControl('', []),
      host: new FormControl(''),
      dateTimeToClose: new FormControl(''),
      participants: new FormControl(''),
      restaurant: new FormControl(''),
      userInput: new FormControl('')
    });

    // const toSelect = this.restaurantPickerGroup.find(c => c.id == this.data.id);
    // console.log(toSelect);
    // this.ownerForm.get('restaurant').setValue(toSelect);
    // let currentDisplayName = "";
    // find current user

    // console.log(this.createdUser);
    // this.eventFormService.setUserInfo(this.hostPickerGroup,this.office365User,this.userPickerGroups,currentDisplayName,this.ownerForm,this.createdUser);

    //get Group
    this.http
      .get(environment.apiUrl + '/api/SPUser/GetGroups')
      .subscribe((data: any) => {
        console.log('request data');
        var jsonData = JSON.parse(data.Data);
        console.log(jsonData);
        for (var i = 0; i < jsonData.value.length; i++) {
          var counter = jsonData.value[i];

          // console.log("check email: " + counter.displayName);
          if (Boolean(counter.mail)) {
            // this.dropdownListNewUser.push({ 'itemName': counter.displayName, 'id': counter.mail });
            // this.office365User.push({ name: counter.displayName, email: counter.mail });
            // this.userLogin.push({ name: counter.displayName, loginName: counter.userPrincipalName });
            this.office365Group.push({
              name: counter.displayName,
              email: counter.mail,
              img: ''
            });
          } else {
            // console.log("khong co email: " + counter.displayName);
          }
        }
      });
    this.userPickerGroups.push({
      name: 'Office 365 Group',
      userpicker: this.office365Group
    });

    this.http
      .get(environment.apiUrl + 'api/SPUser/GetCurrentUser')
      .subscribe((data: any) => {
        console.log(this.hostPickerGroup);
        var selectHost = this.hostPickerGroup.find(
          c => c.name == data.Data.displayName
        );
        console.log(selectHost);
        this.ownerForm.get('host').setValue(selectHost);
      });

    //avatar
    // console.log("get avatar");
    // this.http.get(environment.apiUrl + 'api/SPUser/GetAvatar').subscribe((data: any) => {
    //   console.log("get avatar");

    //   console.log(data);
    //   var dataImg = "data:image/png;base64," + data.Data;
    //   console.log(dataImg);
    // });

    this.ownerForm.get('userInput').setValue(this.data);
    this.ownerForm
      .get('userInput')
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this.isLoading = true)),
        switchMap(value =>
          this.restaurantService
            .SearchRestaurantName(value, 4)
            .pipe(finalize(() => (this.isLoading = true)))
        )
      )
      .subscribe(data =>
        this.restaurantService.getRestaurants(data.Data).then(result => {
          var dataSourceTemp = [];
          result.forEach((element, index) => {
            // tslint:disable-next-line:prefer-const
            // let restaurantItem: Restaurant = {
            //   id: element.restaurant_id,
            //   delivery_id: element.delivery_id,
            //   stared: false,
            //   restaurant: element.name,
            //   address: element.address,
            //   category:
            //     element.categories.length > 0 ? element.categories[0] : '',
            //   promotion:
            //     element.promotion_groups.length > 0
            //       ? element.promotion_groups[0].text
            //       : '',
            //   open:
            //     (element.operating.open_time || '?') +
            //     '-' +
            //     (element.operating.close_time || '?'),
            //   url_rewrite_name: ''
            // };
            // dataSourceTemp.push(restaurantItem);
          });
          this.restaurant$ = dataSourceTemp;
          this.isLoading = false;
        })
      );
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  public onCancel = () => {
    console.log('click cancel');
    if (this.ownerForm.valid && this.eventusers.length > 0) {
      console.log('pass');
    }
  };

  public createOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      // this.executeOwnerCreation(ownerFormValue);
      console.log('pass');
    }
  };
  onNoClick(): void {
    this.dialogRef.close();
  }

  // private executeOwnerCreation = ownerFormValue => {
  //   let owner: OwnerForCreation = {
  //     name: ownerFormValue.name,
  //     dateOfBirth: ownerFormValue.dateOfBirth,
  //     address: ownerFormValue.address,
  //     host: ownerFormValue.host,
  //     dateTimeToClose: this.toDateString(new Date()),
  //     hostNew: ownerFormValue.hostNew
  //   };
  // };

  deleteUserInTable(name: string): void {
    for (var j = 0; j < this.eventusers.length; j++) {
      if (name == this.eventusers[j].name) {
        this.eventusers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }

  changeClient(event) {
    console.log('change client');

    let target = event.source.selected._element.nativeElement;
    this.userSelect = [];

    const toSelect = this.office365User.find(c => c.email == event.value);
    if (toSelect != null) {
      this.userSelect.push({
        name: target.innerText.trim(),
        email: event.value,
        img: toSelect.img
      });
    } else {
      this.userSelect.push({
        name: target.innerText.trim(),
        email: event.value,
        img: ''
      });
    }
  }

  AddUserToTable(): void {
    console.log('Nhan add card');

    console.log(this.userSelect);

    for (var s in this.userSelect) {
      var flag = false;
      for (var e in this.eventusers) {
        if (this.userSelect[s].name == this.eventusers[e].name) {
          flag = true;
        }
      }
      if (flag == false) {
        this.eventusers.push({
          name: this.userSelect[s].name,
          email: this.userSelect[s].email,
          img: this.userSelect[s].img
        });
        this.table.renderRows();
      }
    }
  }

  SaveToSharePointEventList(): void {
    if (this.eventusers.length == 0) {
      alert('Please choose participants!');
      return;
    }

    var host = this.ownerForm.get('host').value.principalName;
    console.log('get host: ');
    console.log(host);

    var title = this.ownerForm.get('title').value;
    console.log('get title: ');
    console.log(title);

    var maximumBudget = this.maximumBudget;
    console.log('get maximumBudget: ');
    console.log(maximumBudget);

    var dateTimeToClose = this.dateTimeToClose.replace('T', ' ');
    console.log('get dateTimeToClose: ');
    console.log(dateTimeToClose);

    var dateToReminder = this.dateToReminder.replace('T', ' ');
    console.log('get dateToReminder: ');
    console.log(dateToReminder);

    var restaurant = this.ownerForm.get('userInput').value.restaurant;
    console.log('get restaurant: ');
    console.log(restaurant);

    var restaurantId = this.ownerForm.get('userInput').value.id;
    console.log('get restaurantId: ');
    console.log(restaurantId);

    var category = this.ownerForm.get('userInput').value.category;
    console.log('get category: ');
    console.log(category);

    var deliveryId = this.ownerForm.get('userInput').value.delivery_id;
    console.log('get deliveryId: ');
    console.log(deliveryId);

    var serciveId = 1;
    console.log('get serciveId: ');
    console.log(serciveId);

    var hostId = this.ownerForm.get('host').value.id;
    console.log('get hostId: ');
    console.log(hostId);

    console.log('get createUserId: ');
    console.log(this.createdUser.id);

    var participantList = '';
    for (var j = 0; j < this.eventusers.length; j++) {
      if (this.eventusers[j].email) {
        participantList = participantList.concat(
          this.eventusers[j].email + ';#'
        );
      }
    }

    let newParticipantList = participantList.slice(0, -2);

    console.log('participant list: ' + newParticipantList);

    var eventlistitem: EventList = {
      eventTitle: title,
      eventId: title,
      eventRestaurant: restaurant,
      eventMaximumBudget: maximumBudget,
      eventTimeToClose: dateTimeToClose,
      eventTimeToReminder: dateToReminder,
      eventHost: host,
      eventParticipants: newParticipantList,
      eventCategory: category,
      eventRestaurantId: restaurantId,
      eventServiceId: '1',
      eventDeliveryId: deliveryId,
      eventCreatedUserId: this.createdUser.id,
      eventHostId: hostId
    };
    this.eventFormService.addEventListItem(eventlistitem).then(r => {
      setTimeout(() => {
        this.SendEmail(title);
      }, 5000);
    });

    //send mail
  }
  SendEmail(title: string) {
    this.restaurantService.setEmail(title);
    console.log('Sent!');
  }
  changeHost(event) {
    let target = event.source.selected._element.nativeElement;
    console.log('host: ' + target.innerText.trim() + ' ' + event.value.email);
  }
  changeRestaurant(event) {
    let target = event.source.selected._element.nativeElement;
    console.log('host: ' + target.innerText.trim() + ' ' + event.value.id);
  }

  changeParticipants(user) {
    console.log(user);
  }
}
