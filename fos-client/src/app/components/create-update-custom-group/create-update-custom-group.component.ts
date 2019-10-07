import {
  Component,
  OnInit,
  ViewChild,
  Input,
  OnChanges,
  Output,
  EventEmitter
} from '@angular/core';
import { MatTable, MatSnackBar } from '@angular/material';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EventUser } from 'src/app/models/eventuser';
import { userPicker } from 'src/app/components/event-dialog/event-dialog.component';
import { environment } from 'src/environments/environment';
import { CustomGroupService } from 'src/app/services/custom-group/custom-group.service';
import { CustomGroup } from './../../models/custom-group';
import { User } from 'src/app/models/user';
import { GraphUser } from 'src/app/models/graph-user';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'app-create-update-custom-group',
  templateUrl: './create-update-custom-group.component.html',
  styleUrls: ['./create-update-custom-group.component.less']
})
export class CreateUpdateCustomGroupComponent implements OnInit, OnChanges {
  @Input() isCreate: boolean;
  @Input() group: CustomGroup;
  @Input() currentUser: User;
  @Input() tempData: number;
  panelOpenState = false;
  displayedColumns = ['avatar', 'name', 'email', 'action'];
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  ownerForm: FormGroup;
  nullString = '';
  eventUsers: EventUser[] = [];
  apiUrl = environment.apiUrl;
  groupName: string;
  @Output() isCloseDrawer = new EventEmitter();

  constructor(
    private customGroupService: CustomGroupService,
    private eventFormService: EventFormService,
    private snackBar: MatSnackBar,
    private overlayContainer: OverlayContainer
  ) {
    this.ownerForm = new FormGroup({
      userInputPicker: new FormControl(''),
      groupName: new FormControl('', [Validators.required])
    });
    this.overlayContainer
      .getContainerElement()
      .classList.add('app-theme1-theme');
  }

  ngOnInit() {
    if (this.group) {
      this.groupName = this.group.Name;
      this.eventUsers = new Array<EventUser>();
      this.group.Users.forEach(item => {
        const user = new EventUser();
        user.Email = item.Mail;
        user.Id = item.Id;
        user.Name = item.DisplayName;
        this.eventUsers.push(user);
      });
    } else {
      this.groupName = '';
      this.eventUsers = [];
    }
  }

  ngOnChanges(): void {
    if (this.group) {
      this.groupName = this.group.Name;
      this.eventUsers = new Array<EventUser>();
      this.group.Users.forEach(item => {
        const user = new EventUser();
        user.Email = item.Mail;
        user.Id = item.Id;
        user.Name = item.DisplayName;
        this.eventUsers.push(user);
      });
    } else {
      this.groupName = '';
      this.eventUsers = [];
    }
  }

  public CreateOwner = ownerFormValue => {
    if (this.ownerForm.valid) {
      //console.log('pass');
    }
    // tslint:disable-next-line:semicolon
  };
  public HasError = (controlName: string, errorName: string) => {
    return this.ownerForm.controls[controlName].hasError(errorName);
    // tslint:disable-next-line:semicolon
  };
  AddUserToTable(): void {
    const self = this;

    const choosingUser = self.ownerForm.get('userInputPicker').value;
    if (!choosingUser.Email) {
      return;
    }
    //console.log('choose User', choosingUser);

    let flag = false;
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
  notifyMessage(eventHost: userPicker) {
    const self = this;
    //console.log('change picker', event);
    const newHost: userPicker[] = this.eventUsers.filter(
      u => u.Email === eventHost.Email
    );
    if (newHost.length === 0) {
      const Host: EventUser = {
        Email: eventHost.Email,
        Id: eventHost.Id,
        Img: '',
        IsGroup: 0,
        Name: eventHost.Name,
        OrderStatus: 'Not order'
      };
      this.eventUsers.push(Host);
      self.table.renderRows();
    }
  }
  save(users: GraphUser[]) {
    if (!this.isCreate) {
      const customGroup = new CustomGroup();
      customGroup.ID = this.group.ID;
      customGroup.Name = this.groupName;
      customGroup.Owner = this.currentUser.Id;
      customGroup.Users = users;
      this.customGroupService.updateGroup(customGroup).then(data => {
        if (!data) {
          this.toast('Update group success', 'Dismiss');
        }
        this.isCloseDrawer.emit({
          isClose: true,
          isChange: true
        });
      });
    } else {
      this.createGroup(users);
    }
  }
  createGroup(users: GraphUser[]) {
    const customGroup = new CustomGroup();
    customGroup.Name = this.groupName;
    customGroup.Owner = this.currentUser.Id;
    customGroup.Users = users;
    this.customGroupService.createGroup(customGroup).then(result => {
      if (!result) {
        this.toast('Create group success', 'Dismiss');
      }
      this.isCloseDrawer.emit({
        isClose: true,
        isChange: true
      });
    });
  }
  removeDuplicateUserAndSave() {
    const participants: GraphUser[] = [];
    const promises: Array<Promise<void>> = [];
    this.eventUsers.map(user => {
      const promise = this.eventFormService
        .GroupListMemers(user.Id)
        .toPromise()
        .then(value => {
          if (value.Data && value) {
            value.Data.map(u => {
              const participantList = participants.filter(
                item => item.DisplayName === u.DisplayName
              );

              if (participantList.length === 0) {
                const participant: GraphUser = {
                  Id: u.Id,
                  DisplayName: u.DisplayName,
                  Mail: u.Mail,
                  UserPrincipalName: u.DisplayName
                };
                participants.push(participant);
              }
            });
          } else {
            const participantList = participants.filter(
              item => item.DisplayName === user.Name
            );

            if (participantList.length === 0) {
              const participant: GraphUser = {
                Id: user.Id,
                DisplayName: user.Name,
                Mail: user.Email,
                UserPrincipalName: user.Name
              };
              participants.push(participant);
            }
          }
        });
      promises.push(promise);
    });
    Promise.all(promises).then(result => {
      this.save(participants);
    });
  }
  DeleteUserInTable(name: string): void {
    for (let j = 0; j < this.eventUsers.length; j++) {
      if (name === this.eventUsers[j].Name) {
        this.eventUsers.splice(j, 1);

        j--;
        this.table.renderRows();
      }
    }
  }
  closeDrawer() {
    this.isCloseDrawer.emit({ isClose: true, isChange: false });
  }
  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
}
