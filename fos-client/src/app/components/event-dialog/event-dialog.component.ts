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
} from "@angular/forms";
import { EventUser } from "../../models/eventuser";
import { EventFormService } from "../../services/event-form/event-form.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { debounceTime, tap, switchMap, finalize } from "rxjs/operators";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { parseSelectorToR3Selector } from "@angular/compiler/src/core";
import { User } from "src/app/models/user";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { GraphUser } from "src/app/models/graph-user";
import { Event } from "src/app/models/event";
import * as moment from "moment";
interface MoreInfo {
  restaurant: DeliveryInfos;
  idService: number;
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
    @Inject(MAT_DIALOG_DATA) public data: MoreInfo,
    private fb: FormBuilder,
    private eventFormService: EventFormService,
    private http: HttpClient,
    private restaurantService: RestaurantService,
    private _snackBar: MatSnackBar
  ) {}

  apiUrl = environment.apiUrl;

  _eventSelected = "Open";
  _createdUser = { id: "" };
  _dateEventTime: string;
  _dateTimeToClose: string;
  _dateToReminder: string;
  _eventType: string;
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
  _loading: boolean;
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

    // this._dateEventTime = this.ToDateString(new Date());
    // this._dateTimeToClose = this.ToDateString(new Date());
    // this._dateToReminder = this.ToDateString(new Date());
    this._maximumBudget = 0;
    this._eventType = "Open";

    // -----
    this.ownerForm = new FormGroup({
      title: new FormControl("", [Validators.required]),
      address: new FormControl("", []),
      host: new FormControl(""),
      dateTimeToClose: new FormControl('', [
        this.ValidateEventCloseTime(
          this._dateEventTime,
          this._dateToReminder,
          this._dateTimeToClose
        )
      ]),
      dateTimeEvent: new FormControl('', [
        this.ValidateEventTime(
          this._dateEventTime,
          this._dateToReminder,
          this._dateTimeToClose
        )
      ]),
      dateTimeRemind: new FormControl('', [
        this.ValidateEventRemindTime(
          this._dateEventTime,
          this._dateToReminder,
          this._dateTimeToClose
        )
      ]),
      participants: new FormControl(""),
      restaurant: new FormControl(""),
      userInput: new FormControl(""),
      userInputHost: new FormControl(""),
      EventType: new FormControl("")
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
        self._createdUser = { id: value.Data.Id };

        var dataSourceTemp: userPicker = {
          Name: value.Data.DisplayName,
          Email: value.Data.Mail,
          Img: "",
          Id: value.Data.Id,
          IsGroup: 0
        };
        console.log("curentuser", dataSourceTemp);
        self.ownerForm.get("userInputHost").setValue(dataSourceTemp);
      });

    this.ownerForm.get("EventType").setValue("Open");
    //debugger;
    // this.ownerForm.get("userInput").setValue(this.data.restaurant);
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

    this.ownerForm.get("userInput").setValue(this.data.restaurant);
    this.ownerForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this._isLoading = true)),
        switchMap(value =>
          this.restaurantService
            .SearchRestaurantName(value, 4, self.data.idService, 217)
            .pipe(finalize(() => (this._isLoading = true)))
        )
      )
      .subscribe(data =>
        this.restaurantService
          .getRestaurants(data.Data, self.data.idService, 217)
          .then(result => {
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
    console.log("change client", event.value);

    let target = event.source.selected._element.nativeElement;
    this._userSelect = [];

    const toSelect = this._office365User.find(
      c => c.Email == event.value.Email
    );
    const toSelectGroup = this._office365Group.find(
      c => c.Email == event.value.Email
    );

    console.log("toSelect", toSelect);
    console.log("toSelectGroup", toSelectGroup);
    if (toSelect != null) {
      this._userSelect.push({
        Name: toSelect.Name,
        Email: toSelect.Email,
        Img: "",
        Id: toSelect.Id,
        IsGroup: 0
      });
    } else {
      this._userSelect.push({
        Name: toSelectGroup.Name,
        Email: toSelectGroup.Email,
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
          IsGroup: this._userSelect[s].IsGroup,
          OrderStatus: "Not Order"
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
    this._loading = true;
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
    console.log("get eventType: ");
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

    this._eventUsers.map(user => {
      if (user.IsGroup === 0) {
        var check = false;
        jsonParticipants.map(mem => {
          if (mem.DisplayName === user.Name) {
            check = true;
          }
        });
        if (check === false) {
          var participant: GraphUser = {
            Id: user.Id,
            DisplayName: user.Name,
            Mail: user.Email,
            UserPrincipalName: user.Name
          };
          jsonParticipants.push(participant);
          numberParticipant++;
        }
      }
    });
    let promises: Array<Promise<void>> = [];
    this._eventUsers.map(user => {
      if (user.IsGroup === 1) {
        let promise = this.eventFormService
          .GroupListMemers(user.Id)
          .toPromise()
          .then(value => {
            value.Data.map(u => {
              var check = false;
              jsonParticipants.map(mem => {
                if (mem.DisplayName === u.DisplayName) {
                  check = true;
                }
              });
              if (check === false) {
                var participant: GraphUser = {
                  Id: u.Id,
                  DisplayName: u.DisplayName,
                  Mail: u.Mail,
                  UserPrincipalName: u.DisplayName
                };
                jsonParticipants.push(participant);
                numberParticipant++;
              }
            });
          });
        promises.push(promise);
      }
    });

    Promise.all(promises).then(function() {
      console.log("final", jsonParticipants);
      var myJSON = JSON.stringify(jsonParticipants);
      console.log("final", myJSON);

      var eventListitem: Event = {
        Name: title,
        EventId: title,
        Restaurant: restaurant,
        MaximumBudget: maximumBudget.toString(),
        CloseTime: new Date(dateTimeToClose),
        RemindTime: new Date(dateToReminder),
        HostName: host,
        Participants: numberParticipant.toString(),
        Category: category,
        RestaurantId: restaurantId,
        ServiceId: "1",
        DeliveryId: deliveryId,
        CreatedBy: self._createdUser.id,
        HostId: hostId,
        EventDate: new Date(eventDate),
        EventParticipantsJson: myJSON,
        EventType: eventType,
        Action: null,
        IsMyEvent: null,
        Status: "Opened"
      };

      self.eventFormService
        .AddEventListItem(eventListitem)
        .toPromise()
        .then(newId => {
          console.log("new Id", newId.Data);

          self.SendEmail(newId.Data);

          self.toast("added new event!", "Dismiss");
          self.dialogRef.close();
        });
    });

    // console.log('participant list: ', jsonParticipants);
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

  ValidateEventRemindTime(
    eventDate: string,
    remindDate: string,
    closeDate: string
  ): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      console.log(remindDate + " " + moment(remindDate).isAfter(eventDate));
      if (remindDate && eventDate && closeDate) {
        if (
          moment(remindDate).isAfter(eventDate) ||
          moment(remindDate).isAfter(closeDate)
        ) {
          return { invalidRemindTime: true };
        }
        return null;
      }
      return null;
    };
  }

  ValidateEventTime(
    eventDate: string,
    remindDate: string,
    closeDate: string
  ): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (eventDate) {
        return null;
      }
      return { datimeRequired: true };
    };
  }

  ValidateEventCloseTime(
    eventDate: string,
    remindDate: string,
    closeDate: string
  ): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      console.log(remindDate + " " + moment(remindDate).isAfter(eventDate));
      if (eventDate && closeDate) {
        if (
          moment(closeDate).isAfter(eventDate)
        ) {
          console.log("eeee");
          return { invalidCloseTime: true };
        }
        return null;
      }
      return { datimeRequired: true };
    };
  }

  onDateTimeChange(value: string): void {
    this.ownerForm.controls["dateTimeRemind"].setValidators([
      this.ValidateEventRemindTime(
        this._dateEventTime,
        this._dateToReminder,
        this._dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeRemind"].updateValueAndValidity();
    this.ownerForm.controls["dateTimeToClose"].setValidators([
      this.ValidateEventCloseTime(
        this._dateEventTime,
        this._dateToReminder,
        this._dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeToClose"].updateValueAndValidity();
    this.ownerForm.controls["dateTimeEvent"].setValidators([
      this.ValidateEventTime(
        this._dateEventTime,
        this._dateToReminder,
        this._dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeEvent"].updateValueAndValidity();
    // console.log(this.ownerForm.get('dateTimeRemind').value)
    console.log(moment(this._dateEventTime).isAfter(this._dateToReminder));
  } 

  isValidEventClose(component: Component) {
    console.log(component);
  }
}
