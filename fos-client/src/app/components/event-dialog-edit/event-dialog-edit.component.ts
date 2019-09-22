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
} from '../event-dialog/event-dialog.component';
import { EventUser } from 'src/app/models/eventuser';
import { DeliveryInfos } from 'src/app/models/delivery-infos';
import { debounceTime, tap, switchMap, finalize } from 'rxjs/operators';
import { User } from 'src/app/models/user';
import { GraphUser } from 'src/app/models/graph-user';
import { OrderService } from 'src/app/services/order/order.service';
import { EventDialogConfirmComponent } from '../event-dialog-confirm/event-dialog-confirm.component';
import { environment } from 'src/environments/environment';
import { debug } from 'util';
import { SummaryService } from 'src/app/services/summary/summary.service';

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
    private summaryService: SummaryService
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
      MaximumBudget: new FormControl(""),
    });
  }
  //global
  apiUrl = environment.apiUrl;
  eventType:string = 'Open';
  matcher = new MyErrorStateMatcher();
  eventSelectedBar = 'Open';
  createdUser = { id: '' };
  dateEventTime: string;
  dateTimeToClose: string;
  dateToReminder: string;
  userSelect = [];
  userPickerGroups: userPickerGroup[] = [];
  showCancelEventConfirmation = false;
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

    this.ownerForm.controls["closeDate"].setValidators([
      Validators.required,
      this.ValidateCloseDate()
    ]);
    this.ownerForm.controls["closeTime"].setValidators([
      Validators.required,
      this.ValidateCloseTime()
    ]);
    this.ownerForm.controls["remindDate"].setValidators([
      this.ValidateRemindDate()
    ]);
    this.ownerForm.controls["remindTime"].setValidators([
      this.ValidateRemindTime()
    ]);

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

    console.log(this.data.RemindTime);
    if (this.data.RemindTime) {
      var newRemindTime = new Date(this.data.RemindTime);
      // var remindTime = this.ToDateString(newRemindTime);
      // this._dateToReminder = remindTime;
      this.ownerForm.controls["remindTime"].setValue(moment(newRemindTime).format('HH:mm'));
      this.ownerForm.controls["remindDate"].setValue(newRemindTime);
    }

    var newEventDate = new Date(this.data.EventDate);
    this.ownerForm.controls["eventTime"].setValue(moment(newEventDate).format('HH:mm'));
    this.ownerForm.controls["eventDate"].setValue(newEventDate);
    this.ownerForm.controls["closeTime"].setValue(moment(newCloseTime).format('HH:mm'));
    this.ownerForm.controls["closeDate"].setValue(newEventDate);

    
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
          })
      );

    var participants = JSON.parse(self.data.EventParticipantsJson);
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
              Img: '',
              IsGroup: 0,
              OrderStatus: 'Not order'
            };
            self.eventUsers.push(userOrder);
          }
          this.table.renderRows();
        });
      });

    promise.then(function() {
      var p = participants;
      var e = self.eventUsers;
      participants.forEach(element => {
        var flag: Boolean = false;

        self.eventUsers.forEach(element2 => {
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
            Img: '',
            IsGroup: 0,
            OrderStatus: 'Order'
          };
          self.eventUsers.push(userOrder);
          self.table.renderRows();
        }
      });
    });
  }

  OnNoClick(): void {
    this.dialogRef.close();
  }

  UpdateToSharePointEventList(): void {
    var self = this;
    if (self.eventUsers.length == 0) {
      self.toast('Please choose participants!', 'Dismiss');
      return;
    }
    this.loading = true;

    var jsonParticipants: GraphUser[] = [];
    var numberParticipant = 0;
    console.log('check value', numberParticipant);

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

    var eventDate = this.dateEventTime;
    var dateTimeToClose = this.dateTimeToClose.replace('T', ' ');
    var dateToReminder = this.dateToReminder
      ? this.dateToReminder.replace('T', ' ')
      : '';

    Promise.all(promises).then(function() {
      var myJSON = JSON.stringify(jsonParticipants);
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
      debugger;
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
      width: '450px',
      data:
        'If you update event information, system will resend email to all attendees.'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (self.eventListItem) {
          self.loading = true;
          self.eventFormService
            .UpdateEventListItem(self.data.EventId, self.eventListItem)
            .toPromise()
            .then(result => {
              console.log('Update', result);
              self.SendEmail(self.data.EventId);
              window.location.reload();
            });
        }
      }
    });
  }

  SendEmail(id: string) {
    this.restaurantService.setEmail(id);
    console.log('Sent!');
  }

  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  AddUserToTable(): void {
    var self = this;
    console.log('Nhan add card');

    console.log(this.userSelect);

    var choosingUser = self.ownerForm.get("userInputPicker").value;
    if(!choosingUser){
      return;
    }
    console.log('choose User', choosingUser);

    var flag = false;
    self.eventUsers.forEach(element => {
      if (element.Name === choosingUser.Name) {
        flag = true;
      }
    });
    if (flag === false) {
      this.eventUsers.push({
        Name: choosingUser.Name,
        Email: choosingUser.Email,
        Img: '',
        Id: choosingUser.Id,
        IsGroup: 0,
        OrderStatus: 'Not Order'
      });
      this.table.renderRows();
    }
  }

  DeleteUserInTable(name: string): void {
    console.log('xoa ', name);
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
      console.log('pass');
    }
  };

  public HasError = (controlName: string, errorName: string) => {
    // console.log(controlName + ' ' +this.ownerForm.controls[controlName].hasError(errorName) + ' ' + errorName);
    return this.ownerForm.controls[controlName].hasError(errorName);
  };

  ValidateCloseTime(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      var eventDate = moment(this.ownerForm.controls["eventDate"].value);
      var closeDate = moment(this.ownerForm.controls["closeDate"].value);
      var eventTime = moment(this.ownerForm.controls["eventTime"].value, 'hh:mm');
      var closeTime = moment(this.ownerForm.controls["closeTime"].value, 'hh:mm');
      if (closeDate.isSame(eventDate)) {
        if (closeTime.isSameOrAfter(eventTime)) {
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
      console.log(closeDate.isAfter(eventDate))
      if (closeDate.isAfter(eventDate)) {
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
      var eventTime = moment(this.ownerForm.controls["eventTime"].value, 'hh:mm');
      var closeTime = moment(this.ownerForm.controls["closeTime"].value, 'hh:mm');
      var remindTime = moment(this.ownerForm.controls["remindTime"].value, 'hh:mm');
      if (remindDate.isSame(eventDate)) {
        if (remindTime.isSameOrAfter(eventTime)) {
          this.toast("Remind time must be before event time in same date", "Ok");
          return { remindTimeInvalid: true };
        }
      }
      if (remindDate.isSame(closeDate)) {
        if (remindTime.isSameOrAfter(closeTime)) {
          this.toast("Remind time must be before close time in same date", "Ok");
          return { remindTimeInvalid: true };
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
      console.log(closeDate.isAfter(eventDate))
      if (remindDate.isAfter(eventDate)) {
        this.toast("Remind date must be before event date", "Ok");
        return { remindDateInvalid: true };
      }
      if (remindDate.isAfter(closeDate)) {
        this.toast("Remind date must be before close date", "Ok");
        return { remindDateInvalid: true };
      }
      return null;
    };
  }

  isValidEventClose(component: Component) {
    console.log(component);
  }
  notifyMessage($event) {
    var self = this;
    console.log('event', $event);
  }
  cancelEvent() {
    const dialogRef = this.dialog.open(EventDialogConfirmComponent, {
      width: '450px',
      data: 'Are you sure you want to cancel this event?'
    });
    dialogRef.afterClosed().subscribe(event => {
      if (event) {
        this.loading = true;
        this.summaryService
          .updateEventStatus(this.data.EventId, 'Error')
          .then(result => {
            this.loading = false;
            window.location.reload();
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
