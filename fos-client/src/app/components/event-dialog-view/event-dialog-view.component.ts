import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable
} from '@angular/material';
import { EventUser } from '../../models/eventuser';
@Component({
  selector: 'app-event-dialog-view',
  templateUrl: './event-dialog-view.component.html',
  styleUrls: ['./event-dialog-view.component.less']
})
export class EventDialogViewComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  // eventusers: EventUser[] = [
  //   // { name: 'Salah', email: 'Liverpool' },
  //   // { name: 'Kane', email: 'Tottenham Hospur' },
  //   // { name: 'Hazard', email: 'Real Madrid' },
  //   // { name: 'Griezmann', email: 'Barcelona' }
  // ];
  constructor(@Inject(MAT_DIALOG_DATA) public data: EventList) {}
  eventTitle = '';
  eventHost = '';
  eventRestaurant = '';
  maximumBudget = '';
  dateTimeToClose = '';
  dateToReminder = '';
  displayedColumns = ['avatar', 'name', 'email'];
  eventusers: EventUser[] = [
    // { name: 'Salah', email: 'Liverpool' },
    // { name: 'Kane', email: 'Tottenham Hospur' },
    // { name: 'Hazard', email: 'Real Madrid' },
    // { name: 'Griezmann', email: 'Barcelona' }
  ];
  ngOnInit() {
    console.log(this.data);
    this.eventTitle = this.data.eventTitle;
    this.eventHost = this.data.eventHost;
    this.eventRestaurant = this.data.eventRestaurant;
    this.maximumBudget = this.data.eventMaximumBudget.toString();
    this.dateTimeToClose = this.data.eventTimeToClose
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    this.dateToReminder = this.data.eventTimeToReminder
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    var emails = [];
    var users = [];
    var groups = [];
    var participants = this.data.eventParticipants;
    emails = participants.split(';#');
    var usersLocalStorage = JSON.parse(localStorage.getItem('users'));
    var groupsLocalStorage = JSON.parse(localStorage.getItem('groups'));

    emails.map(email => {
      // console.log(email);
      usersLocalStorage.map(_user => {
        // console.log(_user);
        if (_user.email === email) {
          users.push(_user);
        }
        // console.log(_user.email + '----------' + email);
      });
      groupsLocalStorage.map(_group => {
        if (_group.email === email) {
          groups.push(_group);
        }
      });
    });

    console.log(groups, users);

    // for (var j = 0; j < groups.length; j++) {
    //   this.eventusers.push({
    //     name: groups[j].name,
    //     email: groups[j].email,
    //     img: groups[j].img
    //   });
    // }
    // for (var j = 0; j < users.length; j++) {
    //   this.eventusers.push({
    //     name: users[j].name,
    //     email: users[j].email,
    //     img: users[j].img
    //   });
    // }
    // this.eventusers.push({
    //   name: 'abc',
    //   email: 'email',
    //   img: ''
    // });
    // this.table.renderRows();
    // if (this.event.id === undefined) {
    //   this.eventTitle = this.event.eventTitle;
    //   this.eventHost = this.event.eventHost;
    //   this.eventRestaurant = this.event.eventRestaurant;
    //   this.maximumBudget = this.event.eventMaximumBudget;
    //   this.dateTimeToClose = this.event.eventTimeToClose
    //     .replace('T', ' ')
    //     .replace('+07:00', '');
    //   this.dateToReminder = this.event.eventTimeToReminder
    //     .replace('T', ' ')
    //     .replace('+07:00', '');
    // }
  }
}
