import { Component, Inject, OnInit, ViewChild, Input } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { Observable, Observer } from "rxjs";
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable,
  MatSnackBar
} from "@angular/material";
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
  ValidatorFn
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
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { GraphUser } from 'src/app/models/graph-user';
import * as moment from 'moment';

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
  selector: "app-event-dialog",
  templateUrl: "./event-dialog.component.html",
  styleUrls: ["./event-dialog.component.less"]
})
export class EventDialogComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  public ownerForm: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeliveryInfos,
    private fb: FormBuilder,
    private eventFormService: EventFormService,
    private http: HttpClient,
    private restaurantService: RestaurantService,
    private _snackBar: MatSnackBar
  ) {}

  _eventSelected = "Open";
  _createdUser = { id: "" };
  _dateEventTime: string;
  _dateTimeToClose: string;
  _dateToReminder: string;
  _eventType:string;
  _maximumBudget: number;
  _userSelect = [];
  _userPickerGroups: userPickerGroup[] = [];

  private ToDateString(date: Date): string {
    return (
      date.getFullYear().toString() +
      "-" +
      ("0" + (date.getMonth() + 1)).slice(-2) +
      "-" +
      ("0" + date.getDate()).slice(-2) +
      "T" +
      date.toTimeString().slice(0, 5)
    );
  }

  _eventUsers: EventUser[] = [];

  _hostPickerGroup = [];

  _displayedColumns = ["avatar", "name", "email", "order status", "action"];

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
    this.eventFormService
      .GetUsers()
      .toPromise()
      .then(u => {
        u.Data.map(us => {
          if (us.Mail) {
            this._office365User.push({
              Name: us.DisplayName,
              Email: us.Mail,
              Img: "",
              Id: us.Id,
              IsGroup: 0
            });
          }
        });
      });
    this._userPickerGroups.push({
      Name: "User",
      UserPicker: this._office365User
    });


    this._dateEventTime = this.ToDateString(new Date());
    this._dateTimeToClose = this.ToDateString(new Date());
    this._dateToReminder = this.ToDateString(new Date());
    this._maximumBudget = 0;
    this._eventType = 'Open';

    // -----
    this.ownerForm = new FormGroup({
      title: new FormControl('', [Validators.required]),
      address: new FormControl('', []),
      host: new FormControl(''),
      dateTimeToClose: new FormControl(new Date(), [this.ValidateEventCloseTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]),
      dateTimeEvent: new FormControl(new Date(), [this.ValidateEventTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]),
      dateTimeRemind: new FormControl(new Date(), [this.ValidateEventRemindTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]),
      participants: new FormControl(''),
      restaurant: new FormControl(''),
      userInput: new FormControl(''),
      userInputHost: new FormControl(''),
      EventType: new FormControl('')
    });

    // get Group
    this.eventFormService
      .GetGroups()
      .toPromise()
      .then(value => {
        value.Data.map(user => {
          if (user.Id && user.Mail) {
            this._office365Group.push({
              Name: user.DisplayName,
              Email: user.Mail,
              Img: "",
              Id: user.Id,
              IsGroup: 1
            });
          }
        });
      });
    this._userPickerGroups.push({
      Name: "Office 365 Group",
      UserPicker: this._office365Group
    });

    //get currentUser
    this.eventFormService
      .getCurrentUser()
      .toPromise()
      .then(value => {
        self._createdUser = { id: value.Data.id };

        var dataSourceTemp: userPicker = {
          Name: value.Data.displayName,
          Email: value.Data.mail,
          Img: "",
          Id: value.Data.id,
          IsGroup: 0
        };
        console.log("curentuser", dataSourceTemp);
        self.ownerForm.get("userInputHost").setValue(dataSourceTemp);
      });

    this.ownerForm.get("EventType").setValue("Open");
    this.ownerForm.get("userInput").setValue(this.data);
    // this.ownerForm.get('EventType').setValue('Open');
    var userHost2: userPicker[];

    this.ownerForm
      .get("userInputHost")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this._isHostLoading = true)),
        switchMap(value =>
          this.eventFormService
            .GetUsersByName(value)
            .pipe(finalize(() => (this._isHostLoading = false)))
        )
      )
      .subscribe((data: ApiOperationResult<Array<User>>) => {
        if (data && data.Data) {
          var dataSourceTemp: userPicker[] = [];
          console.log(data.Data);

          data.Data.map(user => {
            if (user.UserPrincipalName) {
              dataSourceTemp.push({
                Name: user.DisplayName,
                Email: user.UserPrincipalName,
                Img: "",
                Id: user.Id,
                IsGroup: 0
              });
            }
          });

          self._userHost = dataSourceTemp;
          console.log("loading", self._userHost);
          this._isHostLoading = false;
        }
      });

    this.ownerForm.get("userInput").setValue(this.data);
    this.ownerForm
      .get("userInput")
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
  }


  // public OnCancel = () => {
  //   console.log('click cancel');
  //   if (this.ownerForm.valid && this._eventUsers.length > 0) {
  //     console.log('pass');
  //   }
  // };

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      console.log("pass");
    }
  };
  OnNoClick(): void {
    this.dialogRef.close();
  }

  DeleteUserInTable(name: string): void {
    console.log("xoa ", name);
    for (var j = 0; j < this._eventUsers.length; j++) {
      if (name == this._eventUsers[j].Name) {
        this._eventUsers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }

  ChangeClient(event) {
    console.log("change client");

    let target = event.source.selected._element.nativeElement;
    this._userSelect = [];

    const toSelect = this._office365User.find(c => c.Email == event.value);
    const toSelectGroup = this._office365Group.find(
      c => c.Email == event.value
    );
    if (toSelect != null) {
      this._userSelect.push({
        Name: target.innerText.trim(),
        Email: event.value,
        Img: "",
        Id: toSelect.Id,
        IsGroup: 0
      });
    } else {
      this._userSelect.push({
        Name: target.innerText.trim(),
        Email: event.value,
        Img: "",
        IsGroup: 1,
        Id: toSelectGroup.Id
      });
    }
  }

  AddUserToTable(): void {
    console.log("Nhan add card");

    console.log(this._userSelect);

    for (var s in this._userSelect) {
      var flag = false;
      for (var e in this._eventUsers) {
        if (this._userSelect[s].Name == this._eventUsers[e].Name) {
          flag = true;
        }
      }

      if (flag == false) {
        console.log(this._userSelect[s]);

        this._eventUsers.push({
          Name: this._userSelect[s].Name,
          Email: this._userSelect[s].Email,
          Img: "",
          Id: this._userSelect[s].Id,
          IsGroup: this._userSelect[s].IsGroup
        });
        this.table.renderRows();
      }
    }
  }

  SaveToSharePointEventList(): void {
    if (this._eventUsers.length == 0) {
      alert("Please choose participants!");
      return;
    }
    var self = this;

    var host = this.ownerForm.get("userInputHost").value.Name;
    console.log("get host: ", host);

    var title = this.ownerForm.get("title").value;
    console.log("get title: ", title);

    var EventType = this.ownerForm.get("EventType").value;
    console.log("get title: ", EventType);

    var maximumBudget = this._maximumBudget;
    console.log("get maximumBudget: ", maximumBudget);

    var eventDate = this._dateEventTime;
    console.log("get eventDate: ", eventDate);

    var dateTimeToClose = this._dateTimeToClose.replace("T", " ");
    console.log("get dateTimeToClose: ", dateTimeToClose);

    var dateToReminder = this._dateToReminder.replace("T", " ");
    console.log("get dateToReminder: ", dateToReminder);

    var restaurant = this.ownerForm.get("userInput").value.Name;
    console.log("get restaurant: ");
    console.log(restaurant);

    var restaurantId = this.ownerForm.get("userInput").value.RestaurantId;
    console.log("get restaurantId: ");
    console.log(restaurantId);

    var category = this.ownerForm.get("userInput").value.Categories;
    console.log("get category: ");
    console.log(category);

    var deliveryId = this.ownerForm.get("userInput").value.DeliveryId;
    console.log("get deliveryId: ");
    console.log(deliveryId);

    var eventType = this._eventType;
    console.log('get eventType: ');
    console.log(eventType);

    var serciveId = 1;
    console.log("get serciveId: ");
    console.log(serciveId);

    var hostId = this.ownerForm.get("userInputHost").value.Id;
    console.log("get hostId: ");
    console.log(hostId);

    console.log("get createUserId: ");
    console.log(this._createdUser.id);
    var jsonParticipants: GraphUser[] = [];
    var numberParticipant = 0;

    for (var j = 0; j < this._eventUsers.length; j++) {
      if (this._eventUsers[j].Email) {
        console.log(this._eventUsers[j].Email, this._eventUsers[j].IsGroup);
        // get Group
        if (this._eventUsers[j].IsGroup == 1) {
          var participant: GraphUser = {
            id: self._eventUsers[j].Id,
            displayName: self._eventUsers[j].Name,
            mail: self._eventUsers[j].Email,
            userPrincipalName: self._eventUsers[j].Name
          };
          jsonParticipants.push(participant);

          this.eventFormService
            .GroupListMemers(this._eventUsers[j].Id)
            .toPromise()
            .then(value => {
              value.Data.map(user => {
                console.log("member in group", user.DisplayName);
                var flagCheck = false;
                jsonParticipants.map(check => {
                  if (check.displayName === user.DisplayName) {
                    flagCheck = true;
                  }
                });
                if (flagCheck === false) {
                  var p: GraphUser = {
                    id: user.Id,
                    displayName: user.DisplayName,
                    mail: user.Mail,
                    userPrincipalName: user.UserPrincipalName
                  };
                  jsonParticipants.push(p);
                  numberParticipant++;
                }
              });
            });
        } else {
          console.log("user khac", this._eventUsers[j].Name);
          var check = false;
          jsonParticipants.map(mem => {
            if (mem.displayName === this._eventUsers[j].Name) {
              check = true;
            }
          });
          if (check === false) {
            var participant: GraphUser = {
              id: self._eventUsers[j].Id,
              displayName: self._eventUsers[j].Name,
              mail: self._eventUsers[j].Email,
              userPrincipalName: self._eventUsers[j].Name
            };
            jsonParticipants.push(participant);
            numberParticipant++;
          }
        }
      }
    }

    console.log("participant list: ", jsonParticipants);

    var myJSON = JSON.stringify(jsonParticipants);
    console.log("final", myJSON);

    var eventListitem: EventList = {
      EventTitle: title,
      EventId: title,
      EventRestaurant: restaurant,
      EventMaximumBudget: maximumBudget,
      EventTimeToClose: dateTimeToClose,
      EventTimeToReminder: dateToReminder,
      EventHost: host,
      EventParticipants: numberParticipant,
      EventCategory: category,
      EventRestaurantId: restaurantId,
      EventServiceId: "1",
      EventDeliveryId: deliveryId,
      EventCreatedUserId: this._createdUser.id,
      EventHostId: hostId,
      EventDate: eventDate,
      EventParticipantsJson: myJSON,
      EventType: eventType,
    };

    this.eventFormService
      .AddEventListItem(eventListitem)
      .toPromise()
      .then(newId => {
        console.log("new Id", newId.Data);
        this.SendEmail(newId.Data);
        this.toast("added new event!", "Dismiss");
        this.dialogRef.close();
      });
  }
  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
  }
  SendEmail(id: string) {
    this.restaurantService.setEmail(id);
    console.log("Sent!");
  }
  ChangeHost(event) {
    let target = event.source.selected._element.nativeElement;
    console.log("host: " + target.innerText.trim() + " " + event.value.email);
  }
  ChangeRestaurant(event) {
    let target = event.source.selected._element.nativeElement;
    console.log("host: " + target.innerText.trim() + " " + event.value.id);
  }

  ChangeParticipants(user) {
    console.log(user);
  }

  
  public HasError = (controlName: string, errorName: string) => {
    // console.log(controlName + ' ' +this.ownerForm.controls[controlName].hasError(errorName) + ' ' + errorName);
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  ValidateEventRemindTime(eventDate:string, remindDate:string, closeDate:string):ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      console.log(remindDate + ' ' + moment(remindDate).isAfter(eventDate))
      if (remindDate && eventDate && closeDate) {
        if (moment(remindDate).isAfter(eventDate) || moment(remindDate).isAfter(closeDate)) {
          return { invalidRemindTime: true };
        }
        return null;
      }
      return {datimeRequired:true};
    }
  }

  ValidateEventTime(eventDate:string, remindDate:string, closeDate:string):ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      console.log(remindDate + ' ' + moment(remindDate).isAfter(eventDate))
      if (remindDate && eventDate && closeDate) {
        // if (moment(remindDate).isAfter(eventDate) || moment(remindDate).isAfter(closeDate)) {
        //   return { invalidRemindTime: true };
        // }
        return null;
      }
      return {datimeRequired:true};
    }
  }

  ValidateEventCloseTime(eventDate:string, remindDate:string, closeDate:string):ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      console.log(remindDate + ' ' + moment(remindDate).isAfter(eventDate))
      if (remindDate && eventDate && closeDate) {
        if (moment(remindDate).isAfter(closeDate) || moment(closeDate).isAfter(eventDate)) {
          console.log('eeee')
          return { invalidCloseTime: true };
        }
        return null;
      }
      return {datimeRequired:true};
    }
  }


  onDateTimeChange(value: string): void {
    this.ownerForm.controls['dateTimeRemind'].setValidators([this.ValidateEventRemindTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]);
    this.ownerForm.controls['dateTimeRemind'].updateValueAndValidity();
    this.ownerForm.controls['dateTimeToClose'].setValidators([this.ValidateEventCloseTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]);
    this.ownerForm.controls['dateTimeToClose'].updateValueAndValidity();
    this.ownerForm.controls['dateTimeEvent'].setValidators([this.ValidateEventTime(this._dateEventTime, this._dateToReminder, this._dateTimeToClose)]);
    this.ownerForm.controls['dateTimeEvent'].updateValueAndValidity();
    // console.log(this.ownerForm.get('dateTimeRemind').value)
    console.log(moment(this._dateEventTime).isAfter(this._dateToReminder))
  }

  isValidEventClose(component: Component) {
    console.log(component)
  }
}
