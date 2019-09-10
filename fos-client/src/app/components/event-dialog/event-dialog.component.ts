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
import { parseSelectorToR3Selector } from '@angular/compiler/src/core';
import { User } from 'src/app/models/user';

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

export interface userPicker {
  Name: string;
  Email: string;
  Img: string;
  Id: string;
  IsGroup: number;
}

export interface userPickerGroup {
  Name: string;
  UserPicker: userPicker[];
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

  _eventSelected = 'FIKA';
  _createdUser = { id: '' };
  _dateTimeToClose: string;
  _dateToReminder: string;
  _maximumBudget: number;
  _userSelect = [];
  _userPickerGroups: userPickerGroup[] = [];

  private ToDateString(date: Date): string {
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

  _eventUsers: EventUser[] = [];

  _hostPickerGroup = [];

  _displayedColumns = ['avatar', 'name', 'email', 'order status', 'action'];

  _isLoading = false;
  _isHostLoading = false;
  _restaurant: Restaurant[];
  _userHost: userPicker[];
  _office365User: userPicker[] = [];
  _office365Group: userPicker[] = [];

  displayFn(user: Restaurant) {
    if (user) {
      return user.restaurant;
    }
  }

  displayUser(user: userPicker) {
    if (user) {
      return user.Name;
    }
  }

  ngOnInit() {
    var self = this;
    var users = JSON.parse(localStorage.getItem('users'));

    // users.map(user => {
    //   // this._hostPickerGroup.push({
    //   //   id: user.id,
    //   //   name: user.name,
    //   //   email: user.email,
    //   //   principalName: user.principalName
    //   // });
    //   this._office365User.push({
    //     Name: user.name,
    //     Email: user.email,
    //     Img: '',
    //     Id: user.id,
    //     IsGroup: 0
    //   });
    // });
    
    this.eventFormService.GetUsers().toPromise().then(
      u =>{
        console.log('get user ', u.Data);
        u.Data.map( us =>{
          if(us.Mail){
            this._office365User.push({
              Name: us.DisplayName,
              Email: us.Mail,
              Img: '',
              Id: us.Id,
              IsGroup: 0
            });
          }
          
        })
      }
    )


    this._userPickerGroups.push({
      Name: 'User',
      UserPicker: this._office365User
    });

    this._dateTimeToClose = this.ToDateString(new Date());
    this._dateToReminder = this.ToDateString(new Date());
    this._maximumBudget = 0;

    // -----
    this.ownerForm = new FormGroup({
      title: new FormControl('', [
        Validators.required
      ]),
      dateOfBirth: new FormControl(new Date()),
      address: new FormControl('', []),
      host: new FormControl(''),
      dateTimeToClose: new FormControl(''),
      participants: new FormControl(''),
      restaurant: new FormControl(''),
      userInput: new FormControl(''),
      userInputHost: new FormControl('')
    });

    // get User


    // get Group
      this.eventFormService.GetGroups().toPromise().then(value =>{
        value.Data.map(user =>{
          
          if(user.Id && user.Mail){
            this._office365Group.push({
              Name: user.DisplayName,
              Email: user.Mail,
              Img: '',
              Id: user.Id,
              IsGroup: 1
            });
          }
        })
      })
      console.log('group ', this._office365Group)
    this._userPickerGroups.push({
      Name: 'Office 365 Group',
      UserPicker: this._office365Group
    });

    //get currentUser
    this.eventFormService
      .getCurrentUser()
      .toPromise()
      .then(function(value: any) {
        var dataSourceTemp: userPicker = {
          Name: value.Data.displayName,
          Email: value.Data.mail,
          Img: value.Data.AvatarUrl,
          Id: value.Data.id,
          IsGroup: 0
        };
        console.log('curentuser', dataSourceTemp);
        self.ownerForm.get('userInputHost').setValue(dataSourceTemp);
      });

    this.ownerForm.get('userInput').setValue(this.data);

    var userHost2: userPicker[];

    this.ownerForm
      .get('userInputHost')
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this._isHostLoading = true)),
        switchMap(value =>
          this.eventFormService
            .GetUsersByName(value)
            .pipe(finalize(() => (this._isHostLoading = false)))
        )
      )
      .subscribe(
        (data: ApiOperationResult<Array<User>>) => {
          if (data && data.Data) {
            var dataSourceTemp: userPicker[] = [];
            console.log(data.Data);

            data.Data.map(user => {
              if (user.UserPrincipalName) {
                dataSourceTemp.push({
                  Name: user.DisplayName,
                  Email: user.UserPrincipalName,
                  Img: '',
                  Id: user.Id,
                  IsGroup: 0
                });
              }
            })

            self._userHost = dataSourceTemp;
            console.log( 'loading', self._userHost);
            this._isHostLoading = false;
          }
        }

        // this.restaurantService.getRestaurants(data.Data).then(result => {
        //   var dataSourceTemp = [];
        //   result.forEach((element, index) => {
        //     // tslint:disable-next-line:prefer-const
        //     let restaurantItem: Restaurant = {
        //       id: element.restaurant_id,
        //       delivery_id: element.delivery_id,
        //       stared: false,
        //       restaurant: element.name,
        //       address: element.address,
        //       category:
        //         element.categories.length > 0 ? element.categories[0] : '',
        //       promotion:
        //         element.promotion_groups.length > 0
        //           ? element.promotion_groups[0].text
        //           : '',
        //       open:
        //         (element.operating.open_time || '?') +
        //         '-' +
        //         (element.operating.close_time || '?'),
        //       url_rewrite_name: ''
        //     };
        //     dataSourceTemp.push(restaurantItem);
        //   });
        //   this.restaurant$ = dataSourceTemp;
        //   this.isHostLoading = false;
        // })
      );

    // this.ownerForm
    //   .get('userInput')
    //   .valueChanges.pipe(
    //     debounceTime(300),
    //     tap(() => (this.isLoading = true)),
    //     switchMap(value =>
    //       this.restaurantService
    //         .SearchRestaurantName(value, 4)
    //         .pipe(finalize(() => (this.isLoading = true)))
    //     )
    //   )
    //   .subscribe(data =>
    //     this.restaurantService.getRestaurants(data.Data).then(result => {
    //       var dataSourceTemp = [];
    //       result.forEach((element, index) => {
    //         // tslint:disable-next-line:prefer-const
    //         let restaurantItem: Restaurant = {
    //           id: element.restaurantId,
    //           delivery_id: element.deliveryId,
    //           stared: false,
    //           restaurant: element.name,
    //           address: element.address,
    //           category:
    //             element.categories.length > 0 ? element.categories[0] : '',
    //           promotion:
    //             element.promotionGroups.length > 0
    //               ? element.promotionGroups[0]
    //               : '',
    //           open:
    //             (element. || '?') +
    //             '-' +
    //             (element.operating.close_time || '?'),
    //           url_rewrite_name: ''
    //         };
    //         dataSourceTemp.push(restaurantItem);
    //       });
    //       this.restaurant$ = dataSourceTemp;
    //       this.isLoading = false;
    //     })
    //   );
  }
  public HasError = (controlName: string, errorName: string) => {
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  public OnCancel = () => {
    console.log('click cancel');
    if (this.ownerForm.valid && this._eventUsers.length > 0) {
      console.log('pass');
    }
  };

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      console.log('pass');
    }
  };
  OnNoClick(): void {
    this.dialogRef.close();
  }

  DeleteUserInTable(name: string): void {
    for (var j = 0; j < this._eventUsers.length; j++) {
      if (name == this._eventUsers[j].Name) {
        this._eventUsers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }

  ChangeClient(event) {
    console.log('change client');

    let target = event.source.selected._element.nativeElement;
    this._userSelect = [];

    const toSelect = this._office365User.find(c => c.Email == event.value);
    const toSelectGroup = this._office365Group.find(c => c.Email == event.value);
    if (toSelect != null) {
      this._userSelect.push({
        name: target.innerText.trim(),
        email: event.value,
        img: '',
        id: toSelect.Id,
        isGroup: 0
      });
    } else {
      this._userSelect.push({
        name: target.innerText.trim(),
        email: event.value,
        img: '',
        isGroup: 1,
        id: toSelectGroup.Id
      });
    }
  }

  AddUserToTable(): void {
    console.log('Nhan add card');

    // console.log(this.userSelect);

    for (var s in this._userSelect) {
      var flag = false;
      for (var e in this._eventUsers) {
        if (this._userSelect[s].name == this._eventUsers[e].Name) {
          flag = true;
        }
      }
      if (flag == false) {


        this._eventUsers.push({
          Name: this._userSelect[s].Name,
          Email: this._userSelect[s].Email,
          Img: '',
          Id: this._userSelect[s].id,
          IsGroup: this._userSelect[s].isGroup,
        });
        this.table.renderRows();
      }
    }
  }

  SaveToSharePointEventList(): void {
    if (this._eventUsers.length == 0) {
      alert('Please choose participants!');
      return;
    }

    var host = this.ownerForm.get('host').value.principalName;
    console.log('get host: ');
    console.log(host);

    var title = this.ownerForm.get('title').value;
    console.log('get title: ');
    console.log(title);

    var maximumBudget = this._maximumBudget;
    console.log('get maximumBudget: ');
    console.log(maximumBudget);

    var dateTimeToClose = this._dateTimeToClose.replace('T', ' ');
    console.log('get dateTimeToClose: ');
    console.log(dateTimeToClose);

    var dateToReminder = this._dateToReminder.replace('T', ' ');
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
    console.log(this._createdUser.id);

    var participantList = '';
    for (var j = 0; j < this._eventUsers.length; j++) {
      if (this._eventUsers[j].Email) {
        console.log(this._eventUsers[j].Email, this._eventUsers[j].IsGroup);
      }
    }

    let newParticipantList = participantList.slice(0, -2);

    console.log('participant list: ' + newParticipantList);

    var eventListitem: EventList = {
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
      eventCreatedUserId: this._createdUser.id,
      eventHostId: hostId
    };


    // this.eventFormService.addEventListItem(eventlistitem).then(r => {
    //   setTimeout(() => {
    //     this.SendEmail(title);
    //   }, 5000);
    // });
  }
  SendEmail(title: string) {
    this.restaurantService.setEmail(title);
    console.log('Sent!');
  }
  ChangeHost(event) {
    let target = event.source.selected._element.nativeElement;
    console.log('host: ' + target.innerText.trim() + ' ' + event.value.email);
  }
  ChangeRestaurant(event) {
    let target = event.source.selected._element.nativeElement;
    console.log('host: ' + target.innerText.trim() + ' ' + event.value.id);
  }

  ChangeParticipants(user) {
    console.log(user);
  }
}
