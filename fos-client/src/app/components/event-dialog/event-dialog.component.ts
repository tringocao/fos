import { Component, Inject, OnInit, ViewChild, Input } from "@angular/core";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { Observable, Observer, Subscription } from "rxjs";
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
import { debounceTime, tap, switchMap, finalize } from "rxjs/operators";
import { RestaurantService } from "src/app/services/restaurant/restaurant.service";
import { parseSelectorToR3Selector } from "@angular/compiler/src/core";
import { User } from "src/app/models/user";
import { DeliveryInfos } from "src/app/models/delivery-infos";
import { GraphUser } from "src/app/models/graph-user";
import { Event } from "src/app/models/event";
import * as moment from "moment";
import { Group } from "src/app/models/group";
import { element } from "protractor";
import { OverlayContainer } from "@angular/cdk/overlay";
import { CustomGroupService } from "src/app/services/custom-group/custom-group.service";
import { UserService } from "src/app/services/user/user.service";
import { CustomGroup } from "src/app/models/custom-group";

import { Promotion } from "src/app/models/promotion";
import { EventPromotionService } from "src/app/services/event-promotion/event-promotion.service";
import { environment } from "src/environments/environment";
import { NoPromotionsNotificationComponent } from "./no-promotions-notification/no-promotions-notification.component";
import { Restaurant } from "src/app/models/restaurant";
import { OrderService } from "src/app/services/order/order.service";
import { Order } from "src/app/models/order";
import { DataRoutingService } from "src/app/data-routing.service";
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
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MoreInfo,
    @Inject(MAT_DIALOG_DATA) public clonedEventInfo: Event,
    private fb: FormBuilder,
    private eventFormService: EventFormService,
    private eventPromotionService: EventPromotionService,
    private http: HttpClient,
    private restaurantService: RestaurantService,
    private snackBar: MatSnackBar,
    overlayContainer: OverlayContainer,
    private userService: UserService,
    private dataRouting: DataRoutingService,
    private customGroupService: CustomGroupService,
    private orderService: OrderService
  ) {
    this.ownerForm = new FormGroup({
      title: new FormControl("", [Validators.required]),
      address: new FormControl("", []),
      host: new FormControl(""),
      eventDate: new FormControl("", [Validators.required]),
      eventTime: new FormControl("", [Validators.required]),
      closeDate: new FormControl("", [Validators.required]),
      closeTime: new FormControl("", [Validators.required]),
      remindDate: new FormControl(""),
      remindTime: new FormControl(""),

      participants: new FormControl(""),
      restaurant: new FormControl(""),
      userInput: new FormControl(""),
      userInputHost: new FormControl(""),
      EventType: new FormControl(""),
      userInputPicker: new FormControl(""),
      MaximumBudget: new FormControl("")
    });
    this.getNavTitleSubscription = this.dataRouting
      .getNavTitle()
      .subscribe((appTheme: string) => (this.appTheme = appTheme));
    overlayContainer
      .getContainerElement()
      .classList.add("app-" + this.appTheme + "-theme");
  }
  ngOnDestroy() {
    // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
    this.getNavTitleSubscription.unsubscribe();
  }
  private getNavTitleSubscription: Subscription;
  appTheme: string;
  apiUrl = environment.apiUrl;
  eventType: string = "Open";
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
  groups: CustomGroup[] = [];
  selectedGroup: string;
  promotions: Promotion[] = [];

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;

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

  toStandardDate(date: any) {
    return moment(date).format("YYYY-MM-DD");
  }

  ngOnInit() {
    var self = this;
    this.SetClonedEventData(this.clonedEventInfo, self);
  }

  selectGroup($event) {
    this.selectedGroup = $event.value;
  }
  addGroupToTable() {
    if (this.selectedGroup && this.selectedGroup.length > 0) {
      const group = this.groups.find(g => g.ID === this.selectedGroup);
      group.Users.forEach(user => {
        if (!this.eventUsers.find(u => u.Id === user.Id)) {
          const eventUser = {
            Name: user.DisplayName,
            Email: user.Mail,
            Img: "",
            Id: user.Id,
            IsGroup: 0,
            OrderStatus: "Not Order"
          };
          this.eventUsers.push(eventUser);
          this.table.renderRows();
        }
      });
    }
  }

  // public OnCancel = () => {
  //   //console.log('click cancel');
  //   if (this.ownerForm.valid && this.eventUsers.length > 0) {
  //     //console.log('pass');
  //   }
  // };

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      //console.log("pass");
    }
  };
  OnNoClick(): void {
    this.dialogRef.close();
  }

  DeleteUserInTable(name: string): void {
    //console.log("xoa ", name);
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
    //console.log("Nhan add card");

    //console.log(self.userSelect);

    var choosingUser = self.ownerForm.get("userInputPicker").value;

    if (!choosingUser.Email) {
      return;
    }
    //console.log("choose User", choosingUser);
    var flag = false;
    self.eventUsers.forEach(element => {
      if (element.Name === choosingUser.Name) {
        flag = true;
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
      self.toast("Please choose participants!", "Dismiss");
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
                item => item.DisplayName === u.DisplayName
              );

              if (participantList.length === 0) {
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
          } else {
            var participantList = jsonParticipants.filter(
              item => item.DisplayName === user.Name
            );

            if (participantList.length === 0) {
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

    var eventDate =
      this.toStandardDate(this.ownerForm.get("eventDate").value) +
      "T" +
      this.ownerForm.get("eventTime").value;
    //console.log("get eventDate: ", eventDate);

    var dateTimeToClose =
      this.toStandardDate(this.ownerForm.get("closeDate").value) +
      "T" +
      this.ownerForm.get("closeTime").value;
    //console.log("get dateTimeToClose: ", dateTimeToClose);

    var dateToReminder = this.ownerForm.get("remindDate").value
      ? this.toStandardDate(this.ownerForm.get("remindDate").value) +
        "T" +
        this.ownerForm.get("remindTime").value
      : "";
    //console.log("get dateToReminder: ", dateToReminder);

    Promise.all(promises).then(function() {
      //console.log("final", jsonParticipants);
      var myJSON = JSON.stringify(jsonParticipants);
      //console.log("final", myJSON);

      var eventListitem: Event = {
        Name: self.ownerForm.get("title").value,
        EventId: self.ownerForm.get("title").value,
        Restaurant: self.ownerForm.get("userInput").value.Name,
        MaximumBudget: self.ownerForm.get("MaximumBudget").value,
        CloseTime: new Date(dateTimeToClose),
        RemindTime: new Date(dateToReminder),
        HostName: self.ownerForm.get("userInputHost").value.Name,
        Participants: numberParticipant.toString(),
        Category: self.ownerForm.get("userInput").value.Categories,
        RestaurantId: self.ownerForm.get("userInput").value.RestaurantId,
        ServiceId: self.clonedEventInfo
          ? self.clonedEventInfo.ServiceId
          : self.data.idService.toString(),
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
          if (newId.Success === true) {
            //console.log("new Id", newId.Data);
            if (self.promotions) {
              self.eventPromotionService.AddEventPromotion(
                Number(newId.Data),
                self.promotions
              );
            }
            self.SendEmail(newId.Data);
            self.toast("Added new event!", "Dismiss");
            self.dialogRef.close();
          } else {
            self.toast(newId.ErrorMessage.toString(), "Dismiss");
            self.loading = false;
          }
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
    //console.log("Sent!");
  }
  fetchAllPromotions() {
    this.eventPromotionService
      .getPromotionsByExternalService(
        Number(this.ownerForm.get("userInput").value.DeliveryId),
        1
      )
      .then(promotions => {
        this.promotions = promotions;
        if (this.promotions.length == 0) {
          const dialogRef = this.dialog.open(
            NoPromotionsNotificationComponent,
            {
              maxWidth: "50%",
              data: this.ownerForm.get("userInput").value.Name
            }
          );

          dialogRef.afterClosed().subscribe(result => {
            //console.log("The dialog was closed");
          });
        }
        // this.promotionChanged.emit(this.promotions);
      });
  }
  removePromotion(promotion: Promotion) {
    this.promotions = this.promotions.filter(pr => pr !== promotion);
  }

  public HasError = (controlName: string, errorName: string) => {
    // //console.log(controlName + ' ' +this.ownerForm.controls[controlName].hasError(errorName) + ' ' + errorName);
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  ValidateCloseTime(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var eventTime = moment(
        this.ownerForm.controls["eventTime"].value,
        "hh:mm"
      );
      var closeTime = moment(
        this.ownerForm.controls["closeTime"].value,
        "hh:mm"
      );
      var remindTime = moment(
        this.ownerForm.controls["remindTime"].value,
        "hh:mm"
      );
      if (closeDate.isSame(eventDate)) {
        if (closeTime.isSameOrAfter(eventTime)) {
          this.toast("Close time must be before event time in same date", "Ok");
          return { closeTimeInvalid: true };
        }
      }
      if (closeDate.isSame(remindTime)) {
        if (remindTime.isSameOrAfter(closeTime)) {
          this.toast("Close time must be before event time in same date", "Ok");
          return { closeTimeInvalid: true };
        }
      }
      return null;
    };
  }

  ValidateCloseDate(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var remindDate = moment(this.ownerForm.controls["remindDate"].value);
      if (closeDate.isAfter(eventDate)) {
        this.toast("Close date must be before event date", "Ok");
        return { closeDateInvalid: true };
      }
      if (remindDate.isAfter(closeDate)) {
        this.toast("Close date must be before event date", "Ok");
        return { closeDateInvalid: true };
      }
      return null;
    };
  }

  ValidateRemindTime(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var remindDate = moment(this.ownerForm.controls["remindDate"].value);
      var eventTime = moment(
        this.ownerForm.controls["eventTime"].value,
        "hh:mm"
      );
      var closeTime = moment(
        this.ownerForm.controls["closeTime"].value,
        "hh:mm"
      );
      var remindTime = moment(
        this.ownerForm.controls["remindTime"].value,
        "hh:mm"
      );
      if (remindTime && remindDate) {
        if (remindDate.isSame(eventDate)) {
          if (remindTime.isSameOrAfter(eventTime)) {
            this.toast(
              "Remind time must be before event time in same date",
              "Ok"
            );
            return { remindTimeInvalid: true };
          }
        }
        if (remindDate.isSame(closeDate)) {
          if (remindTime.isSameOrAfter(closeTime)) {
            this.toast(
              "Remind time must be before close time in same date",
              "Ok"
            );
            return { remindTimeInvalid: true };
          }
        }
      }
      return null;
    };
  }

  ValidateRemindDate(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var remindDate = moment(this.ownerForm.controls["remindDate"].value);

      if (remindDate) {
        if (remindDate.isAfter(eventDate)) {
          this.toast("Remind date must be before event date", "Ok");
          return { remindDateInvalid: true };
        }
        if (remindDate.isAfter(closeDate)) {
          this.toast("Remind date must be before close date", "Ok");
          return { remindDateInvalid: true };
        }
      }

      return null;
    };
  }

  ValidateEventTime(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var remindDate = moment(this.ownerForm.controls["remindDate"].value);
      var eventTime = moment(
        this.ownerForm.controls["eventTime"].value,
        "hh:mm"
      );
      var closeTime = moment(
        this.ownerForm.controls["closeTime"].value,
        "hh:mm"
      );
      var remindTime = moment(
        this.ownerForm.controls["remindTime"].value,
        "hh:mm"
      );
      if (eventDate.isSame(remindDate)) {
        if (remindTime.isSameOrAfter(eventTime)) {
          this.toast(
            "Remind time must be before event time in same date",
            "Ok"
          );
          return { eventTimeInvalid: true };
        }
      }
      if (closeDate.isSame(eventDate)) {
        if (closeTime.isSameOrAfter(eventTime)) {
          this.toast(
            "Remind time must be before close time in same date",
            "Ok"
          );
          return { eventTimeInvalid: true };
        }
      }
      return null;
    };
  }

  ValidateEventDate(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var remindDate = moment(this.ownerForm.controls["remindDate"].value);
      if (remindDate.isAfter(eventDate)) {
        this.toast("Remind date must be before event date", "Ok");
        return { eventDateInvalid: true };
      }
      if (closeDate.isAfter(eventDate)) {
        this.toast("Remind date must be before close date", "Ok");
        return { eventDateInvalid: true };
      }
      return null;
    };
  }

  onDateTimeInputChange($event) {
    //console.log($event);
    this.checkDatetimeValidation();
  }

  getCurrentEventType() {
    return this.ownerForm.controls["EventType"]
      ? this.ownerForm.controls["EventType"].value
      : "Open";
  }

  checkDatetimeValidation() {
    this.ownerForm.controls["closeDate"].setErrors([]);
    this.ownerForm.controls["closeTime"].setErrors([]);
    this.ownerForm.controls["eventDate"].setErrors([]);
    this.ownerForm.controls["eventTime"].setErrors([]);
    this.ownerForm.controls["remindDate"].setErrors([]);
    this.ownerForm.controls["remindTime"].setErrors([]);
    this.ownerForm.controls["closeDate"].updateValueAndValidity();
    this.ownerForm.controls["closeTime"].updateValueAndValidity();
    this.ownerForm.controls["eventDate"].updateValueAndValidity();
    this.ownerForm.controls["eventTime"].updateValueAndValidity();
    this.ownerForm.controls["remindDate"].updateValueAndValidity();
    this.ownerForm.controls["remindTime"].updateValueAndValidity();
    this.ownerForm.controls["closeDate"].setValidators([
      Validators.required,
      this.ValidateCloseDate()
    ]);
    this.ownerForm.controls["closeTime"].setValidators([
      Validators.required,
      this.ValidateCloseTime()
    ]);
    this.ownerForm.controls["remindDate"].setValidators([
      this.ownerForm.controls["remindTime"].value
        ? Validators.required
        : Validators.nullValidator,
      this.ValidateRemindDate()
    ]);
    this.ownerForm.controls["remindTime"].setValidators([
      this.ownerForm.controls["remindDate"].value
        ? Validators.required
        : Validators.nullValidator,
      this.ValidateRemindTime()
    ]);
    this.ownerForm.controls["eventDate"].setValidators([
      Validators.required,
      this.ValidateEventDate()
    ]);
    this.ownerForm.controls["eventTime"].setValidators([
      Validators.required,
      this.ValidateEventTime()
    ]);
    this.ownerForm.controls["closeDate"].updateValueAndValidity();
    this.ownerForm.controls["closeTime"].updateValueAndValidity();
    this.ownerForm.controls["eventDate"].updateValueAndValidity();
    this.ownerForm.controls["eventTime"].updateValueAndValidity();
    this.ownerForm.controls["remindDate"].updateValueAndValidity();
    this.ownerForm.controls["remindTime"].updateValueAndValidity();
  }

  isValidEventClose(component: Component) {
    //console.log(component);
  }
  notifyMessage(eventHost: userPicker) {
    var self = this;
    //console.log("change picker", event);
    var newHost: userPicker[] = this.eventUsers.filter(
      u => u.Email === eventHost.Email
    );
    if (newHost.length == 0) {
      var Host: EventUser = {
        Email: eventHost.Email,
        Id: eventHost.Id,
        Img: "",
        IsGroup: 0,
        Name: eventHost.Name,
        OrderStatus: "Not ordered"
      };
      this.pushUserToParticipants(Host);
      self.table.renderRows();
    }
  }

  SetClonedEventData(clonnedEventData: Event, self) {
    //console.log(clonnedEventData);
    self.ownerForm
      .get("MaximumBudget")
      .setValue(
        clonnedEventData && clonnedEventData.MaximumBudget
          ? clonnedEventData.MaximumBudget
          : 0
      );
    self.ownerForm
      .get("EventType")
      .setValue(
        clonnedEventData && clonnedEventData.EventType
          ? clonnedEventData.EventType
          : "Open"
      );

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

        //console.log("curentuser", dataSourceTemp);
        self.ownerForm.get("userInputHost").setValue(dataSourceTemp);

        const host: EventUser = {
          Email: dataSourceTemp.Email,
          Name: dataSourceTemp.Name,
          OrderStatus: "Not Ordered",
          Img: "",
          IsGroup: dataSourceTemp.IsGroup,
          Id: dataSourceTemp.Id
        };
        this.pushUserToParticipants(host);
        self.table.renderRows();
        self.loading = false;
      });

    const restaurant = new DeliveryInfos();
    restaurant.Name =
      clonnedEventData && clonnedEventData.Restaurant
        ? clonnedEventData.Restaurant
        : self.data.restaurant.Name;
    restaurant.DeliveryId =
      clonnedEventData && clonnedEventData.DeliveryId
        ? clonnedEventData.DeliveryId
        : self.data.restaurant.DeliveryId;
    restaurant.Categories =
      clonnedEventData && clonnedEventData.Category
        ? clonnedEventData.Category
        : self.data.restaurant.Categories;
    restaurant.RestaurantId =
      clonnedEventData && clonnedEventData.RestaurantId
        ? clonnedEventData.RestaurantId
        : self.data.restaurant.RestaurantId;

    self.ownerForm
      .get("userInput")
      .setValue(
        clonnedEventData && clonnedEventData.Restaurant
          ? restaurant
          : self.data.restaurant
      );
    self.ownerForm
      .get("userInput")
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (self.isLoading = true)),
        switchMap(value =>
          self.restaurantService
            .SearchRestaurantName(
              value,
              4,
              clonnedEventData && clonnedEventData.ServiceId
                ? Number(clonnedEventData.ServiceId)
                : self.data.idService,
              217
            )
            .pipe(finalize(() => (self.isLoading = true)))
        )
      )
      .subscribe(data =>
        self.restaurantService
          .getRestaurants(
            data.Data,
            clonnedEventData && clonnedEventData.ServiceId
              ? Number(clonnedEventData.ServiceId)
              : self.data.idService,
            217
          )
          .then(result => {
            self.restaurant = result;
            self.isLoading = false;
            self.fetchAllPromotions();
          })
      );
    this.userService.getCurrentUser().then(user => {
      this.customGroupService.getAllGroup(user.Id).then(allGroup => {
        this.groups = allGroup;
      });
    });
    this.SetParticipantList(clonnedEventData);
    if (clonnedEventData) {
      this.loadDateTimeClone(clonnedEventData);
    }
  }

  SetParticipantList(clonnedEventData: Event) {
    const participants: Array<GraphUser> = JSON.parse(
      clonnedEventData.EventParticipantsJson
    );
    this.orderService
      .GetOrdersByEventId(clonnedEventData.EventId)
      .then((order: Array<Order>) => {
        order.forEach(o => {
          const userNotOrder: GraphUser[] = participants.filter(
            p => p.Mail === o.Email
          );
          if (o.OrderStatus === 0) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Not Ordered",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.pushUserToParticipants(UserNot);
          } else if (o.OrderStatus == 1) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Ordered",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.pushUserToParticipants(UserNot);
          } else if (o.OrderStatus == 2) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Not Participant",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.pushUserToParticipants(UserNot);
          }
        });
        this.table.renderRows();
      });
  }

  pushUserToParticipants(user: EventUser) {
    if (this.eventUsers.findIndex(_user => _user.Id === user.Id) === -1) {
      this.eventUsers.push(user);
    }
  }

  loadDateTimeClone(clonnedEventData: Event) {
    var newCloseTime = new Date(clonnedEventData.CloseTime);
    var closeTime = this.ToDateString(newCloseTime);
    this.dateTimeToClose = closeTime;

    //console.log(clonnedEventData.RemindTime);
    if (clonnedEventData.RemindTime) {
      var newRemindTime = new Date(clonnedEventData.RemindTime);
      // var remindTime = this.ToDateString(newRemindTime);
      // this._dateToReminder = remindTime;
      this.ownerForm.controls["remindTime"].setValue(
        moment(newRemindTime).format("HH:mm")
      );
      this.ownerForm.controls["remindDate"].setValue(new Date());
    }

    var newEventDate = new Date(clonnedEventData.EventDate);
    this.ownerForm.controls["eventTime"].setValue(
      moment(newEventDate).format("HH:mm")
    );
    this.ownerForm.controls["eventDate"].setValue(new Date());
    this.ownerForm.controls["closeTime"].setValue(
      moment(newCloseTime).format("HH:mm")
    );
    this.ownerForm.controls["closeDate"].setValue(new Date());

    this.checkDatetimeValidation();

    var eventTime = this.ToDateString(newEventDate);
    this.dateEventTime = eventTime;
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return control && control.invalid;
  }
}
