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
    private snackBar: MatSnackBar
  ) {
    this.ownerForm = new FormGroup({
      title: new FormControl("", [Validators.required]),
      address: new FormControl("", []),
      host: new FormControl(""),
      dateTimeToClose: new FormControl('', [
        this.ValidateEventCloseTime(
          this.dateEventTime,
          this.dateToReminder,
          this.dateTimeToClose
        )
      ]),
      dateTimeEvent: new FormControl('', [
        this.ValidateEventTime(
          this.dateEventTime,
          this.dateToReminder,
          this.dateTimeToClose
        )
      ]),
      dateTimeRemind: new FormControl('', [
        this.ValidateEventRemindTime(
          this.dateEventTime,
          this.dateToReminder,
          this.dateTimeToClose
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
  eventSelected = "Open";
  createdUser = { id: "" };
  dateEventTime: string;
  dateTimeToClose: string;
  dateToReminder: string;
  userSelect = [];
  userPickerGroups: userPickerGroup[] = [];
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

  eventUsers: EventUser[] = [];

  hostPickerGroup = [];

  displayedColumns = ["avatar", "name", "email", "order status", "action"];

  isLoading = false;
  isHostLoading = false;
  isPickerLoading = false;
  restaurant: DeliveryInfos[];
  userHost: userPicker[];
  userPicker: userPicker[];
  office365User: userPicker[] = [];
  office365Group: userPicker[] = [];
  loading: boolean;
  eviroment = environment.apiUrl;
  listPickedUser: userPicker[];

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
    // this.dateEventTime = this.ToDateString(new Date());
    // this.dateTimeToClose = this.ToDateString(new Date());
    // this.dateToReminder = this.ToDateString(new Date());
    self.ownerForm.get("MaximumBudget").setValue(0);
    self.ownerForm.get("EventType").setValue('Open');
    //get currentUser
    self.loading = true;
    self.eventFormService
      .getCurrentUser()
      .toPromise()
      .then(value => {
        self.createdUser = { id: value.Data.Id };
        var dataSourceTemp: userPicker = {
          Name: value.Data.DisplayName,
          Email: value.Data.Mail,
          Img: "",
          Id: value.Data.Id,
          IsGroup: 0
        };

        console.log("curentuser", dataSourceTemp);
        self.ownerForm.get("userInputHost").setValue(dataSourceTemp);

        self.eventUsers.push({
          Name: dataSourceTemp.Name,
          Email: dataSourceTemp.Email,
          Img: "",
          Id: dataSourceTemp.Id,
          IsGroup: dataSourceTemp.IsGroup,
          OrderStatus: "Not Order"
        });
        self.table.renderRows();
        self.loading = false;
      });

      self.ownerForm.get("EventType").setValue("Open");
      self.ownerForm.get("userInput").setValue(self.data.restaurant);
      self.ownerForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (self.isLoading = true)),
        switchMap(value =>
          self.restaurantService
            .SearchRestaurantName(value, 4, self.data.idService, 217)
            .pipe(finalize(() => (self.isLoading = true)))
        )
      )
      .subscribe(data =>
        self.restaurantService
          .getRestaurants(data.Data, self.data.idService, 217)
          .then(result => {
            self.restaurant = result;
            self.isLoading = false;
          })
      );
  }

  // public OnCancel = () => {
  //   console.log('click cancel');
  //   if (this.ownerForm.valid && this.eventUsers.length > 0) {
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
    for (var j = 0; j < this.eventUsers.length; j++) {
      if (name == this.eventUsers[j].Name) {
        this.eventUsers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }

  AddUserToTable(): void {
    var self = this;
    console.log("Nhan add card");

    console.log(self.userSelect);

    var choosingUser = self.ownerForm.get("userInputPicker").value;

    if(!choosingUser){
      return;
    }
    console.log('choose User', choosingUser);
    var flag = false;
    self.eventUsers.forEach(element => {
      if (element.Name === choosingUser.Name) {
        flag = true
      }
    });
    if (flag === false) {
      self.eventUsers.push({
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
    if (self.eventUsers.length == 0) {
      alert("Please choose participants!");
      return;
    }
   
    self.loading = true;
    
    var jsonParticipants: GraphUser[] = [];
    var numberParticipant = 0;

    let promises: Array<Promise<void>> = [];
    this.eventUsers.map(user => {
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
    debugger;

    var eventDate = this.dateEventTime;
    console.log("get eventDate: ", eventDate);

    var dateTimeToClose = this.dateTimeToClose.replace("T", " ");
    console.log("get dateTimeToClose: ", dateTimeToClose);

    var dateToReminder = this.dateToReminder ? this.dateToReminder.replace("T", " ") : '';
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
        CreatedBy: self.createdUser.id,
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
    this.snackBar.open(message, action, {
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
    if (this.dateToReminder && this.dateEventTime && moment(this.dateEventTime).isSameOrAfter(this.dateToReminder)) {
      this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be before event time");
    }
    if (this.dateToReminder && this.dateTimeToClose && moment(this.dateToReminder).isSameOrAfter(this.dateTimeToClose)) {
      this.isInvalidRemindTime = true
      // this.ownerForm.controls["dateTimeToClose"].setErrors({invalidCloseTime: true})
      alert("Time to remind must be after event close time");
    }
    if (this.dateTimeToClose && this.dateEventTime && moment(this.dateTimeToClose).isSameOrAfter(this.dateEventTime)) {
      this.isInvalidCloseTime = true
      this.ownerForm.controls["dateTimeToClose"].setErrors({ invalidCloseTime: true })
      alert("Time to close must be before event time");
    }

    this.ownerForm.controls["dateTimeRemind"].setValidators([
      this.ValidateEventRemindTime(
        this.dateEventTime,
        this.dateToReminder,
        this.dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeRemind"].updateValueAndValidity();
    this.ownerForm.controls["dateTimeToClose"].setValidators([
      this.ValidateEventCloseTime(
        this.dateEventTime,
        this.dateToReminder,
        this.dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeToClose"].updateValueAndValidity();
    this.ownerForm.controls["dateTimeEvent"].setValidators([
      this.ValidateEventTime(
        this.dateEventTime,
        this.dateToReminder,
        this.dateTimeToClose
      )
    ]);
    this.ownerForm.controls["dateTimeEvent"].updateValueAndValidity();
    // console.log(this.ownerForm.get('dateTimeRemind').value)
    console.log(moment(this.dateEventTime).isSameOrAfter(this.dateToReminder));
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
