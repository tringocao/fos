import { Event } from "./../../models/event";
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable,
  MatSnackBar,
  ErrorStateMatcher
} from "@angular/material";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
  ValidatorFn,
  FormGroupDirective,
  NgForm,
} from "@angular/forms";
import * as moment from "moment";
import { Component, Inject, OnInit, ViewChild, Input } from "@angular/core";
import { EventFormService } from "src/app/services/event-form/event-form.service";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import {
  userPickerGroup,
  userPicker
} from "../event-dialog/event-dialog.component";
import { EventUser } from "src/app/models/eventuser";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { debounceTime, tap, switchMap, finalize } from "rxjs/operators";
import { User } from "src/app/models/user";
import { GraphUser } from "src/app/models/graph-user";
import { OrderService } from "src/app/services/order/order.service";
import { EventDialogConfirmComponent } from "../event-dialog-confirm/event-dialog-confirm.component";
import { environment } from 'src/environments/environment';

@Component({
  selector: "app-event-dialog-edit",
  templateUrl: "./event-dialog-edit.component.html",
  styleUrls: ["./event-dialog-edit.component.less"]
})
export class EventDialogEditComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  public ownerForm: FormGroup;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Event,
    private eventFormService: EventFormService,
    private restaurantService: RestaurantService,
    public dialogRef: MatDialogRef<EventDialogEditComponent>,
    private _snackBar: MatSnackBar,
    private orderService: OrderService,
    public dialog: MatDialog
  ) {}
  //global
  apiUrl = environment.apiUrl;
  matcher = new MyErrorStateMatcher();
  
  _eventSelected = "Open";
  _createdUser = { id: "" };
  _dateEventTime: string;
  _dateTimeToClose: string;
  _dateToReminder: string;
  _maximumBudget: number;
  _userSelect = [];
  _userPickerGroups: userPickerGroup[] = [];
  _eventType: string;
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
  _eventListItem: Event = null;

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
    //get group
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

    this.ownerForm = new FormGroup({
      title: new FormControl("", [Validators.required]),
      address: new FormControl("", []),
      host: new FormControl(""),
      dateTimeToClose: new FormControl(''),
      dateTimeEvent: new FormControl(''),
      dateTimeRemind: new FormControl(''),
      participants: new FormControl(""),
      restaurant: new FormControl(""),
      userInput: new FormControl(""),
      userInputHost: new FormControl(""),
      EventType: new FormControl("")
    });

    this.ownerForm.get("title").setValue(this.data.Name);
    this.ownerForm.get("EventType").setValue(this.data.EventType);

    var dataHostTemp: userPicker = {
      Name: this.data.HostName,
      Email: "",
      Img: "",
      Id: this.data.HostId,
      IsGroup: 0
    };
    this.ownerForm.get("userInputHost").setValue(dataHostTemp);
    this._maximumBudget = Number(this.data.MaximumBudget);

    var newCloseTime = new Date(this.data.CloseTime);
    var closeTime = this.ToDateString(newCloseTime);
    this._dateTimeToClose = closeTime;

    console.log(this.data.RemindTime)
    if (this.data.RemindTime) {
      var newRemindTime = new Date(this.data.RemindTime);
      var remindTime = this.ToDateString(newRemindTime);
      this._dateToReminder = remindTime;
    }

    var newEventDate = new Date(this.data.EventDate);
    var eventTime = this.ToDateString(newEventDate);
    this._dateEventTime = eventTime;
    self._eventType = self.data.EventType;

    //restaurant
    var restaurantTemp: DeliveryInfos = {
      CityId: "1",
      RestaurantId: this.data.RestaurantId,
      IsOpen: "",
      IsFoodyDelivery: "",
      Campaigns: "",
      PromotionGroups: "",
      Photo: "",
      Operating: "",
      Address: "",
      DeliveryId: this.data.DeliveryId,
      Categories: this.data.Category,
      Name: this.data.Restaurant,
      UrlRewriteName: "",
      IsFavorite: false
    };

    this.ownerForm.get("userInput").setValue(restaurantTemp);
    this.ownerForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this._isLoading = true)),
        switchMap(value =>
          this.restaurantService
            .SearchRestaurantName(value, 4, Number(self.data.ServiceId), 217)
            .pipe(finalize(() => (this._isLoading = true)))
        )
      )
      .subscribe(data =>
        this.restaurantService
          .getRestaurants(data.Data, Number(self.data.ServiceId), 217)
          .then(result => {
            this._restaurant = result;
            this._isLoading = false;
          })
      );

    //Host
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

    //load table
    var participants = JSON.parse(this.data.EventParticipantsJson);
    // participants.map(value => {
    //   console.log('paricipant ', value);
    //   this._eventUsers.push({
    //     Id: value.id,
    //     Name: value.displayName,
    //     Img: '',
    //     Email: value.mail,
    //     IsGroup: 0,
    //     OrderStatus: 'Not Order'
    //   });
    // });
    let promise = self.orderService
      .GetUserNotOrdered(self.data.EventId)
      .then(result => {
        result.forEach(element => {
          var participant = participants.filter(
            item => item.Id === element.UserId
          );
          if (participant) {
            const userOrder: EventUser = {
              Name: participant[0].DisplayName,
              Email: participant[0].Mail,
              Id: participant[0].Id,
              Img: "",
              IsGroup: 0,
              OrderStatus: "Not order"
            };
            self._eventUsers.push(userOrder);
          }
          this.table.renderRows();
        });
      });

    promise.then(function() {
      var p = participants;
      var e = self._eventUsers;
      participants.forEach(element => {
        var flag: Boolean = false;

        self._eventUsers.forEach(element2 => {
          if (element.Id === element2.Id) {
            flag = true;
          }
        });

        if (flag === false) {
          console.log(element.DisplayName);
          const userOrder: EventUser = {
            Name: element.DisplayName,
            Email: element.Mail,
            Id: element.Id,
            Img: "",
            IsGroup: 0,
            OrderStatus: "Order"
          };
          self._eventUsers.push(userOrder);
          self.table.renderRows();
        }
      });
    });
  }

  OnNoClick(): void {
    this.dialogRef.close();
  }

  UpdateToSharePointEventList(): void {
    if (this._eventUsers.length == 0) {
      self.toast("Please choose participants!", "Dismiss");
      return;
    }
    this._loading = true;
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

    var dateToReminder = this._dateToReminder ? this._dateToReminder.replace("T", " ") : '';
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

    var eventType = self._eventType;
    console.log("get eventType: ");
    console.log(eventType);

    var serciveId = 1;
    console.log("get serciveId: ");
    console.log(serciveId);

    var hostId = this.ownerForm.get("userInputHost").value.Id;
    console.log("get hostId: ");
    console.log(hostId);

    console.log("get createUserId: ");
    console.log(this.data.CreatedBy);
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

      self._eventListItem = {
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
        CreatedBy: self.data.CreatedBy,
        HostId: hostId,
        EventDate: new Date(eventDate),
        EventParticipantsJson: myJSON,
        EventType: eventType,
        Action: null,
        IsMyEvent: null,
        Status: "Opened"
      };
      self._loading = false;
      self.openDialog();

      // self.eventFormService
      //   .UpdateEventListItem(self.data.EventId, eventListitem)
      //   .toPromise()
      //   .then(result => {
      //     console.log('Update', result);
      //     self.SendEmail(self.data.EventId);
      //     self.toast('update new event!', 'Dismiss');
      //     self.dialogRef.close();
      //   });
    });
  }

  openDialog(): void {
    var self = this;
    const dialogRef = this.dialog.open(EventDialogConfirmComponent, {
      width: "450px",
      data: Event
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (self._eventListItem) {
          self._loading = true;
          self.eventFormService
            .UpdateEventListItem(self.data.EventId, self._eventListItem)
            .toPromise()
            .then(result => {
              console.log("Update", result);
              self.SendEmail(self.data.EventId);
              self.toast("update new event!", "Dismiss");
              self.dialogRef.close();
            });
        }
      }
    });
  }

  SendEmail(id: string) {
    this.restaurantService.setEmail(id);
    console.log("Sent!");
  }

  toast(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000
    });
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

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      console.log("pass");
    }
  };

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
      console.log(remindDate + " " + moment(remindDate).isSameOrAfter(eventDate));
      if (remindDate && eventDate && closeDate) {
        if (
          moment(remindDate).isSameOrAfter(eventDate) ||
          moment(remindDate).isSameOrAfter(closeDate)
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
      console.log(remindDate + " " + moment(remindDate).isSameOrAfter(eventDate));
      if (eventDate && closeDate) {
        if (
          moment(closeDate).isSameOrAfter(eventDate)
        ) {
          console.log("eeee");
          // this.isInvalidCloseTime = true;
          this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
          this.ownerForm.controls["dateTimeToClose"].hasError('invalidCloseTime')
          return { invalidCloseTime: true };
        }
        return null;
      }
      return { datimeRequired: true };
    };
  }

  onDateTimeChange(value: string): void {
    if (this._dateToReminder && this._dateEventTime && moment(this._dateEventTime).isSameOrAfter(this._dateToReminder)) {
      // this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be before event time");
    }
    if (this._dateToReminder && this._dateTimeToClose && moment(this._dateToReminder).isSameOrAfter(this._dateTimeToClose)) {
      // this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be after event close time");
    }
    if (this._dateTimeToClose && this._dateEventTime && moment(this._dateTimeToClose).isSameOrAfter(this._dateEventTime)) {
      // this.isInvalidCloseTime = true
      this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to close must be before event time");
    }
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
    console.log(moment(this._dateEventTime).isSameOrAfter(this._dateToReminder));
  }

  isValidEventClose(component: Component) {
    console.log(component);
  }
}
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return (control && control.invalid);
  }
}