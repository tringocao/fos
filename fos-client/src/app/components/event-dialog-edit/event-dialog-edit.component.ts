import { Event } from './../../models/event';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable
} from '@angular/material';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder
} from '@angular/forms';
import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
import { userPickerGroup, userPicker } from '../event-dialog/event-dialog.component';
import { EventUser } from 'src/app/models/eventuser';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { debounceTime, tap, switchMap, finalize } from 'rxjs/operators';
import { User } from 'src/app/models/user';
@Component({
  selector: 'app-event-dialog-edit',
  templateUrl: './event-dialog-edit.component.html',
  styleUrls: ['./event-dialog-edit.component.less']
})
export class EventDialogEditComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  public ownerForm: FormGroup;
  constructor(@Inject(MAT_DIALOG_DATA) public data: Event,
  private eventFormService: EventFormService,
  private restaurantService: RestaurantService,
  public dialogRef: MatDialogRef<EventDialogEditComponent>) { }
  //global
  _eventSelected = 'Open';
  _createdUser = { id: '' };
  _dateEventTime: string;
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

  _eventUsers: EventUser[] = [
  ];

  _hostPickerGroup = [];

  _displayedColumns = ['avatar', 'name', 'email', 'order status', 'action'];

  _isLoading = false;
  _isHostLoading = false;
  _restaurant: DeliveryInfos[];
  _userHost: userPicker[];
  _office365User: userPicker[] = [];
  _office365Group: userPicker[] = [];

  displayFn(user: DeliveryInfos) {
    if (user) {
      return user.Name;
    }
  }

  displayUser(user: userPicker) {
    if (user) {
      return user.Name;
    }
  }


  ngOnInit() {
    var self = this;
    //get user
    this.eventFormService.GetUsers().toPromise().then(
      u => {
        u.Data.map(us => {
          if (us.Mail) {
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
    //get group
    this.eventFormService.GetGroups().toPromise().then(value => {
      value.Data.map(user => {

        if (user.Id && user.Mail) {
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
    this._userPickerGroups.push({
      Name: 'Office 365 Group',
      UserPicker: this._office365Group
    });

    this.ownerForm = new FormGroup({
      title: new FormControl('', [
        Validators.required
      ]),
      dateTimeToClose: new FormControl(''),
      participants: new FormControl(''),
      restaurant: new FormControl(''),
      userInput: new FormControl(''),
      userInputHost: new FormControl(''),
      EventType: new FormControl(''),
    });

    this.ownerForm.get('title').setValue(this.data.Name);
    this.ownerForm.get('EventType').setValue(this.data.EventType);

    var dataHostTemp: userPicker = {
      Name: this.data.HostName,
      Email: '',
      Img: '',
      Id: this.data.HostId,
      IsGroup: 0
    };
    this.ownerForm.get('userInputHost').setValue(dataHostTemp);
    this._maximumBudget = Number(this.data.MaximumBudget);

    var newCloseTime = new Date(this.data.CloseTime);
    var closeTime = this.ToDateString(newCloseTime);
    this._dateTimeToClose = closeTime;

    var newRemindTime = new Date(this.data.RemindTime);
    var remindTime = this.ToDateString(newRemindTime);
    this._dateToReminder = remindTime;

    var newEventDate = new Date(this.data.EventDate);
    var eventTime = this.ToDateString(newEventDate);
    this._dateEventTime = eventTime;
    
    //restaurant
    var restaurantTemp: DeliveryInfos = {
        CityId: '1',   
        RestaurantId: this.data.RestaurantId,   
        IsOpen: '',   
        IsFoodyDelivery: '',   
        Campaigns: '',   
        PromotionGroups: '',   
        Photo: '',   
        Operating: '',   
        Address: '',   
        DeliveryId: this.data.DeliveryId,   
        Categories: this.data.Category,   
        Name: this.data.Restaurant,   
        UrlRewriteName: '',
    }

    this.ownerForm.get('userInput').setValue(restaurantTemp);
      this.ownerForm
        .get('userInput')
        .valueChanges.pipe(
          debounceTime(300),
          tap(() => (this._isLoading = true)),
          switchMap(value =>
            this.restaurantService
              .SearchRestaurantName(value, 4)
              .pipe(finalize(() => (this._isLoading = true)))
          )
        )
        .subscribe(data =>
          this.restaurantService.getRestaurants(data.Data).then(result => {
            this._restaurant = result;
            this._isLoading = false;
          })
        );

    //Host
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
      );
      //load table
      var participants = JSON.parse(this.data.EventParticipantsJson);
      participants.map(value => {
        console.log('paricipant ', value);
        this._eventUsers.push({
          Id: value.id,
          Name: value.displayName,
          Img: '',
          Email: value.mail,
          IsGroup: 0
        });
      })
  }
  public HasError = (controlName: string, errorName: string) => {
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  OnNoClick(): void {
    this.dialogRef.close();
  }
}
