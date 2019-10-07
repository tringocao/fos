import { Event } from './../../models/event';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable,
  MatSnackBar,
  ErrorStateMatcher
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
  FormBuilder,
  AbstractControl,
  ValidatorFn,
  FormGroupDirective,
  NgForm
} from '@angular/forms';
import * as moment from 'moment';
import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { RestaurantService } from 'src/app/services/restaurant/restaurant.service';
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
import { environment } from "src/environments/environment";
import { debug } from "util";
import { SummaryService } from "src/app/services/summary/summary.service";
import { EventFormValidationService } from "src/app/services/event-form/event-form-validation/event-form-validation.service";
import { EventFormMailService } from "src/app/services/event-form/event-form-mail/event-form-mail.service";
import { UpdateEvent } from "src/app/models/update-event";
import { OverlayContainer } from "@angular/cdk/overlay";
import { CustomGroup } from "./../../models/custom-group";
import { UserService } from "src/app/services/user/user.service";
import { CustomGroupService } from "src/app/services/custom-group/custom-group.service";
import { Order } from "src/app/models/order";
import { EventUsers } from "src/app/models/event-users";

import { Promotion } from "src/app/models/promotion";
import { EventPromotionService } from "src/app/services/event-promotion/event-promotion.service";
import { EventPromotion } from "src/app/models/event-promotion";
import { NoPromotionsNotificationComponent } from "../event-dialog/no-promotions-notification/no-promotions-notification.component";
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-event-dialog-edit',
  templateUrl: './event-dialog-edit.component.html',
  styleUrls: ['./event-dialog-edit.component.less']
})
export class EventDialogEditComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  public ownerForm: FormGroup;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Event,
    private eventFormService: EventFormService,
    private restaurantService: RestaurantService,
    public dialogRef: MatDialogRef<EventDialogEditComponent>,
    private snackBar: MatSnackBar,
    private orderService: OrderService,
    public dialog: MatDialog,
    private summaryService: SummaryService,
    private eventValidationService: EventFormValidationService,
    private eventMail: EventFormMailService,
    private userService: UserService,
    private customGroupService: CustomGroupService,
    private eventPromotionService: EventPromotionService,
    overlayContainer: OverlayContainer    ,
    private dataRouting: DataRoutingService
  ) {
    this.ownerForm = new FormGroup({
      title: new FormControl("", [Validators.required]),
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
      userInputPicker: new FormControl(""),
      EventType: new FormControl(""),
      MaximumBudget: new FormControl("")
    });
    this.getNavTitleSubscription = this.dataRouting.getNavTitle()
    .subscribe((appTheme: string) => this.appTheme = appTheme);
    overlayContainer.getContainerElement().classList.add("app-"+this.appTheme+"-theme"); 
   }
    ngOnDestroy() {
      // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
      this.getNavTitleSubscription.unsubscribe();
    }
    private getNavTitleSubscription: Subscription;
    appTheme: string;
  //global
  apiUrl = environment.apiUrl;
  eventType: string = "Open";
  matcher = new MyErrorStateMatcher();
  eventSelectedBar = 'Open';
  createdUser = { id: '' };
  dateEventTime: string;
  dateTimeToClose: string;
  dateToReminder: string;
  userSelect = [];
  userPickerGroups: userPickerGroup[] = [];
  showCancelEventConfirmation = false;
  eventTime: string;
  closeTime: string;
  remindTime: string;
  displayFn(user: DeliveryInfos) {
    if (user) {
      return user.Name;
    }
  }
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

  eventUsers: EventUser[] = [];

  hostPickerGroup = [];

  displayedColumns = ['avatar', 'name', 'email', 'order status', 'action'];

  isLoading = false;
  isHostLoading = false;
  restaurant: DeliveryInfos[];
  userHost: userPicker[];
  loading: boolean;
  eventListItem: Event = null;
  enviroment = environment.apiUrl;

  checkRestaurant: Boolean = false;

  removeListUser: GraphUser[] = [];
  newListUser: GraphUser[] = [];
  groups: CustomGroup[] = [];
  selectedGroup: string;

  eventPromotion: EventPromotion = new EventPromotion();
  promotions: Promotion[];

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;

  ngOnInit() {
    var self = this;

    self.ownerForm.get('EventType').setValue(self.data.EventType);
    self.ownerForm.get('title').setValue(self.data.Name);
    self.ownerForm.get('EventType').setValue(self.data.EventType);
    self.ownerForm.get('MaximumBudget').setValue(self.data.MaximumBudget);

    self.ownerForm.get("EventType").setValue(self.data.EventType);
    self.ownerForm.get("title").setValue(self.data.Name);
    self.ownerForm.get("EventType").setValue(self.data.EventType);
    self.ownerForm.get("MaximumBudget").setValue(self.data.MaximumBudget);

    this.getDbPromotions(self.data.EventId);
    var dataHostTemp: userPicker = {
      Name: self.data.HostName,
      Email: '',
      Img: '',
      Id: self.data.HostId,
      IsGroup: 0
    };

    self.ownerForm.get('userInputHost').setValue(dataHostTemp);

    var newCloseTime = new Date(this.data.CloseTime);
    var closeTime = this.ToDateString(newCloseTime);
    this.dateTimeToClose = closeTime;

    //console.log(this.data.RemindTime);
    if (this.data.RemindTime) {
      var newRemindTime = new Date(this.data.RemindTime);
      // var remindTime = this.ToDateString(newRemindTime);
      // this._dateToReminder = remindTime;
      this.ownerForm.controls["remindTime"].setValue(
        moment(newRemindTime).format("HH:mm")
      );
      this.ownerForm.controls["remindDate"].setValue(newRemindTime);
      this.remindTime = moment(newRemindTime).toString();
    }

    var newEventDate = new Date(this.data.EventDate);
    this.ownerForm.controls["eventTime"].setValue(
      moment(newEventDate).format("HH:mm")
    );
    this.ownerForm.controls["eventDate"].setValue(newEventDate);
    this.ownerForm.controls["closeTime"].setValue(
      moment(newCloseTime).format("HH:mm")
    );
    this.ownerForm.controls["closeDate"].setValue(newCloseTime);

    this.eventTime = moment(newEventDate).toString();
    this.closeTime = moment(newCloseTime).toString();

    this.checkDatetimeValidation();

    var eventTime = this.ToDateString(newEventDate);
    this.dateEventTime = eventTime;

    //restaurant
    var restaurantTemp: DeliveryInfos = {
      CityId: '1',
      RestaurantId: self.data.RestaurantId,
      IsOpen: '',
      IsFoodyDelivery: '',
      Campaigns: '',
      PromotionGroups: '',
      Photo: '',
      Operating: '',
      Address: '',
      DeliveryId: self.data.DeliveryId,
      Categories: self.data.Category,
      Name: self.data.Restaurant,
      UrlRewriteName: '',
      IsFavorite: false
    };

    self.ownerForm.get('userInput').setValue(restaurantTemp);

    self.ownerForm
      .get('userInput')
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (self.isLoading = true)),
        switchMap(value =>
          self.restaurantService
            .SearchRestaurantName(value, 4, Number(self.data.ServiceId), 217)
            .pipe(finalize(() => (self.isLoading = true)))
        )
      )
      .subscribe(data =>
        self.restaurantService
          .getRestaurants(data.Data, Number(self.data.ServiceId), 217)
          .then(result => {
            self.restaurant = result;
            self.isLoading = false;
            this.fetchAllPromotions();
          })
      );

    // var participants = JSON.parse(self.data.EventParticipantsJson);
    const participants: Array<GraphUser> = JSON.parse(
      this.data.EventParticipantsJson
    );

    this.orderService
      .GetOrdersByEventId(this.data.EventId)
      .then((order: Array<Order>) => {
        order.forEach(o => {
          const userNotOrder: GraphUser[] = participants.filter(
            p => p.Mail == o.Email
          );
          if (o.OrderStatus == 0) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Not Ordered",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.eventUsers.push(UserNot);
          } else if (o.OrderStatus == 1) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Ordered",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.eventUsers.push(UserNot);
          } else if (o.OrderStatus == 2) {
            const UserNot: EventUser = {
              Email: userNotOrder[0].Mail,
              Name: userNotOrder[0].DisplayName,
              OrderStatus: "Not Participant",
              Img: "",
              IsGroup: 0,
              Id: userNotOrder[0].Id
            };
            this.eventUsers.push(UserNot);
          }
        });
        this.table.renderRows();
      });
    this.userService.getCurrentUser().then(user => {
      this.customGroupService.getAllGroup(user.Id).then(allGroup => {
        this.groups = allGroup;
      });
    });
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

  OnNoClick(): void {
    this.dialogRef.close();
  }

  toStandardDate(date: any) {
    return moment(date).format('YYYY-MM-DD');
  }

  UpdateToSharePointEventList(): void {
    var self = this;
    if (self.eventUsers.length == 0) {
      self.toast('Please choose participants!', 'Dismiss');
      return;
    }

    //check edit change
    self.checkRestaurant = self.eventValidationService.CheckEventChangeRestaurant(
      self.data,
      self.ownerForm.get("userInput").value
    );

    this.loading = true;

    var jsonParticipants: GraphUser[] = [];
    var numberParticipant = 0;
    //console.log('check value', numberParticipant);

    let promises: Array<Promise<void>> = [];
    this.eventUsers.map(user => {
      let promise = self.eventFormService
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
      //check list added user and removed user
      self.newListUser = self.eventValidationService.GetNewParticipants(
        self.data,
        jsonParticipants
      );
      self.removeListUser = self.eventValidationService.GetRemoveParticipants(
        self.data,
        jsonParticipants
      );
      var myJSON: string = JSON.stringify(jsonParticipants);

      self.eventListItem = {
        Name: self.ownerForm.get('title').value,
        EventId: self.ownerForm.get('title').value,
        Restaurant: self.ownerForm.get('userInput').value.Name,
        MaximumBudget: self.ownerForm.get('MaximumBudget').value,
        CloseTime: new Date(dateTimeToClose),
        RemindTime: new Date(dateToReminder),
        HostName: self.ownerForm.get('userInputHost').value.Name,
        Participants: numberParticipant.toString(),
        Category: self.ownerForm.get('userInput').value.Categories,
        RestaurantId: self.ownerForm.get('userInput').value.RestaurantId,
        ServiceId: self.data.ServiceId,
        DeliveryId: self.ownerForm.get('userInput').value.DeliveryId,
        CreatedBy: self.data.CreatedBy,
        HostId: self.ownerForm.get('userInputHost').value.Id,
        EventDate: new Date(eventDate),
        EventParticipantsJson: myJSON,
        EventType: self.ownerForm.get('EventType').value,
        Action: null,
        IsMyEvent: null,
        Status: 'Opened'
      };
      self.loading = false;
      self.openDialog();
    });
  }

  openDialog(): void {
    const self = this;
    const dialogRef = this.dialog.open(EventDialogConfirmComponent, {
      width: '450px',
      data:
        'If you change restaurant, system will resend email to all attendees.'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (self.eventListItem) {
          self.loading = true;
          if (this.checkRestaurant === false) {
            self.eventFormService
              .UpdateEventListItem(self.data.EventId, self.eventListItem)
              .toPromise()
              .then(result => {
                  if (result.Success === true) {
                      self.eventPromotion.EventId = Number(self.data.EventId);
                      self.eventPromotion.Promotions = self.promotions;
                      //console.log(self.eventPromotion);
                      self.eventPromotionService.UpdateEventPromotion(
                          self.eventPromotion
                      );
                  const updateEvent: UpdateEvent = {
                    IdEvent: self.data.EventId,
                    NewListUser: self.newListUser,
                    RemoveListUser: self.removeListUser
                  };
                  self.eventMail.SendMailUpdateEvent(updateEvent).then(value => {
                    self.toast('Update event successfuly!', 'Dismiss');
                    window.location.reload();
                  });
                } else {
                  self.loading = false;
                  self.toast(result.ErrorMessage.toString(), 'Dismiss');
                }
              });
          } else {
            self.eventFormService
              .UpdateListItemWhenRestaurantChanges(
                self.data.EventId,
                self.eventListItem
              )
              .toPromise()
              .then(result => {
                if (result.Success === true) {
                  //console.log('Update', result);
                  self.SendEmail(self.data.EventId);
                  self.toast('Update event successfuly!', 'Dismiss');
                  window.location.reload();
                } else {
                  self.loading = false;
                  self.toast(result.ErrorMessage.toString(), 'Dismiss');
                }
              });
          }
        }
      }
    });
  }

  SendEmail(id: string) {
    this.restaurantService.setEmail(id);
    //console.log('Sent!');
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

  getDbPromotions(eventId: string) {
    this.eventPromotionService.GetByEventId(Number(eventId)).then(promotion => {
      if (promotion) {
        this.eventPromotion = promotion;
        this.promotions = this.eventPromotion.Promotions;
      } else {
        this.eventPromotion = new EventPromotion();
        this.promotions = this.eventPromotion.Promotions;
      }
      // this.promotionChanged.emit(this.promotions);
    });
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  AddUserToTable(): void {
    var self = this;
    //console.log('Nhan add card');

    //console.log(this.userSelect);

    var choosingUser = self.ownerForm.get("userInputPicker").value;
    if (!choosingUser.Email) {
      return;
    }
    //console.log('choose User', choosingUser);

    var flag = false;
    self.eventUsers.forEach(element => {
      if (element.Name === choosingUser.Name) {
        flag = true;
      }
    });
    if (flag === false) {
      if (choosingUser.Email) {
        self.eventUsers.push({
          Name: choosingUser.Name,
          Email: choosingUser.Email,
          Img: '',
          Id: choosingUser.Id,
          IsGroup: 0,
          OrderStatus: 'Not Order'
        });
        self.table.renderRows();
      }
    }
  }

  DeleteUserInTable(name: string): void {
    //console.log('xoa ', name);
    for (var j = 0; j < this.eventUsers.length; j++) {
      if (name == this.eventUsers[j].Name) {
        this.eventUsers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      //console.log('pass');
    }
  };

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
        Img: '',
        IsGroup: 0,
        Name: eventHost.Name,
        OrderStatus: "Not order"
      };
      this.eventUsers.push(Host);
      self.table.renderRows();
    }
  }
  cancelEvent() {
    const dialogRef = this.dialog.open(EventDialogConfirmComponent, {
      width: "450px",
      data: "Are you sure you want to cancel this event?"
    });
    dialogRef.afterClosed().subscribe(event => {
      if (event) {
        this.loading = true;
        this.summaryService
          .updateEventStatus(this.data.EventId, "Error")
          .then(result => {
            const ListUserCancel: EventUsers[] = [];

            this.eventUsers.forEach((u: EventUser) => {
              const testUser: EventUsers = {
                UserMail: u.Email,
                EventId: this.data.EventId,
                EventTitle: this.ownerForm.get('title').value.toString(),
                UserName: u.Name
              };
              ListUserCancel.push(testUser);
            });
            this.eventMail
              .SendCancelEventMail(ListUserCancel)
              .then(() => {
                this.loading = false;
                window.location.reload();
              })
              .catch(error => this.toast(error, 'Dismiss'));
          });
      }
    });
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
