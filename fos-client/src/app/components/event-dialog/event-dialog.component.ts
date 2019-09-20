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
  MatSnackBar,
  ErrorStateMatcher
} from "@angular/material";
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
  ValidatorFn,
  FormGroupDirective,
  NgForm,
  FormControlName
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
import { Group } from 'src/app/models/group';
import { element } from 'protractor';
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
  public userInputPicker: any;
  constructor(
    public dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MoreInfo,
    private fb: FormBuilder,
    private eventFormService: EventFormService,
    private http: HttpClient,
    private restaurantService: RestaurantService,
    private _snackBar: MatSnackBar
  ) {
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
      EventType: new FormControl(""),
      userInputPicker: new FormControl(""),
      MaximumBudget: new FormControl(""),
    });
  }

  apiUrl = environment.apiUrl;
  matcher = new MyErrorStateMatcher();
  _eventSelected = "Open";
  _createdUser = { id: "" };
  _dateEventTime: string;
  _dateTimeToClose: string;
  _dateToReminder: string;
  _userSelect = [];
  _userPickerGroups: userPickerGroup[] = [];
  isInvalidCloseTime: boolean;
  isInvalidRemindTime: boolean;

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
  _isPickerLoading = false;
  _restaurant: DeliveryInfos[];
  _userHost: userPicker[];
  _userPicker: userPicker[];
  _office365User: userPicker[] = [];
  _office365Group: userPicker[] = [];
  _loading: boolean;
  _eviroment = environment.apiUrl;
  _listPickedUser: userPicker[];

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

  displayUserPicker(user: userPicker) {
    if (user) {
      return user.Name;
    }
  }

  ngOnInit() {
    var self = this;
    // this._dateEventTime = this.ToDateString(new Date());
    // this._dateTimeToClose = this.ToDateString(new Date());
    // this._dateToReminder = this.ToDateString(new Date());
    self.ownerForm.get("MaximumBudget").setValue(0);
    self.ownerForm.get("EventType").setValue('Open');
    //get currentUser
    self._loading = true;
    self.eventFormService
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

        self._eventUsers.push({
          Name: dataSourceTemp.Name,
          Email: dataSourceTemp.Email,
          Img: "",
          Id: dataSourceTemp.Id,
          IsGroup: dataSourceTemp.IsGroup,
          OrderStatus: "Not Order"
        });
        self.table.renderRows();
        self._loading = false;
      });

      self.ownerForm.get("EventType").setValue("Open");
      self.ownerForm.get("userInput").setValue(self.data.restaurant);
      self.ownerForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (self._isLoading = true)),
        switchMap(value =>
          self.restaurantService
            .SearchRestaurantName(value, 4, self.data.idService, 217)
            .pipe(finalize(() => (self._isLoading = true)))
        )
      )
      .subscribe(data =>
        self.restaurantService
          .getRestaurants(data.Data, self.data.idService, 217)
          .then(result => {
            self._restaurant = result;
            self._isLoading = false;
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

  AddUserToTable(): void {
    var self = this;
    console.log("Nhan add card");

    console.log(self._userSelect);

    var choosingUser = self.ownerForm.get("userInputPicker").value;
    console.log('choose User', choosingUser);

    var flag = false;
    self._eventUsers.forEach(element => {
      if (element.Name === choosingUser.Name) {
        flag = true
      }
    });
    if (flag === false) {
      self._eventUsers.push({
        Name: choosingUser.Name,
        Email: choosingUser.Email,
        Img: "",
        Id: choosingUser.Id,
        IsGroup: 0,
        OrderStatus: "Not Order"
      });
      self.table.renderRows();
    }
  }

  SaveToSharePointEventList(): void {
    var self = this;
    if (self._eventUsers.length == 0) {
      alert("Please choose participants!");
      return;
    }
   
    self._loading = true;
    
    var jsonParticipants: GraphUser[] = [];
    var numberParticipant = 0;

    let promises: Array<Promise<void>> = [];
    this._eventUsers.map(user => {
      let promise = this.eventFormService
        .GroupListMemers(user.Id)
        .toPromise()
        .then(value => {
          if (value.Data && value) {
            value.Data.map(u => {
              var participantList = jsonParticipants.filter(
                item => item.DisplayName === u.DisplayName);
                
              if(participantList.length === 0){
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
          }
           else {
            var participantList =  jsonParticipants.filter(
              item => item.DisplayName === user.Name);
              
            if(participantList.length === 0){
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
      promises.push(promise);
    });    

    var eventDate = this._dateEventTime;
    console.log("get eventDate: ", eventDate);

    var dateTimeToClose = this._dateTimeToClose.replace("T", " ");
    console.log("get dateTimeToClose: ", dateTimeToClose);

    var dateToReminder = this._dateToReminder ? this._dateToReminder.replace("T", " ") : '';
    console.log("get dateToReminder: ", dateToReminder);
    
    Promise.all(promises).then(function () {
      console.log("final", jsonParticipants);
      var myJSON = JSON.stringify(jsonParticipants);
      console.log("final", myJSON);

      var eventListitem: Event = {
        Name: self.ownerForm.get("title").value,
        EventId: self.ownerForm.get("title").value,
        Restaurant: self.ownerForm.get("userInput").value.Name,
        MaximumBudget:self.ownerForm.get("MaximumBudget").value,
        CloseTime: new Date(dateTimeToClose),
        RemindTime: new Date(dateToReminder),
        HostName: self.ownerForm.get("userInputHost").value.Name,
        Participants: numberParticipant.toString(),
        Category: self.ownerForm.get("userInput").value.Categories,
        RestaurantId: self.ownerForm.get("userInput").value.RestaurantId,
        ServiceId: self.data.idService.toString(),
        DeliveryId: self.ownerForm.get("userInput").value.DeliveryId,
        CreatedBy: self._createdUser.id,
        HostId: self.ownerForm.get("userInputHost").value.Id,
        EventDate: new Date(eventDate),
        EventParticipantsJson: myJSON,
        EventType: self.ownerForm.get("EventType").value,
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
          this.isInvalidCloseTime = true;
          this.ownerForm.controls["dateTimeToClose"].setErrors({ invalidCloseTime: true })
          this.ownerForm.controls["dateTimeToClose"].hasError('invalidCloseTime')
          return { invalidCloseTime: true };
        }
        return null;
      }
      return { datimeRequired: true };
    };
  }

  onDateTimeChange(value: string): void {
    if (this._dateToReminder && this._dateEventTime && moment(this._dateToReminder).isSameOrAfter(this._dateEventTime)) {
      this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be before event time");
    }
    if (this._dateToReminder && this._dateTimeToClose && moment(this._dateToReminder).isSameOrAfter(this._dateTimeToClose)) {
      this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be before event close time");
    }
    if (this._dateTimeToClose && this._dateEventTime && moment(this._dateTimeToClose).isSameOrAfter(this._dateEventTime)) {
      this.isInvalidCloseTime = true
      this.ownerForm.controls["dateTimeToClose"].setErrors({ invalidCloseTime: true })
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
  notifyMessage($event) {
    var self = this;
    console.log('event', $event);
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return (control && control.invalid);
  }

}
