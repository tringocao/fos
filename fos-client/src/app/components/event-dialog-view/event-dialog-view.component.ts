import { Component, Inject, OnInit, ViewChild, Input } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import {
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatTable
} from '@angular/material';
import { Event } from './../../models/event';
import { EventUser } from 'src/app/models/eventuser';
@Component({
  selector: 'app-event-dialog-view',
  templateUrl: './event-dialog-view.component.html',
  styleUrls: ['./event-dialog-view.component.less']
})
export class EventDialogViewComponent implements OnInit {
  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  eventusers: EventUser[] = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data: Event,public dialogRef: MatDialogRef<EventDialogViewComponent>) {}
  eventTitle = '';
  eventHost = '';
  eventRestaurant = '';
  maximumBudget = '';
  dateTimeToClose = '';
  dateToReminder = '';
  displayedColumns = ['avatar', 'Name', 'Email'];
  EventTime = '';
  EventStatus ='';
  EventType = '';
  ngOnInit() {
    console.log(this.data);

    this.eventTitle = this.data.Name;
    this.eventHost = this.data.HostName;
    this.eventRestaurant = this.data.Restaurant;
    this.maximumBudget = this.data.MaximumBudget.toString();
    this.dateTimeToClose = this.data.CloseTime
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
    this.dateToReminder = this.data.RemindTime
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');

      this.EventTime = this.data.EventDate
      .toString()
      .replace('T', ' ')
      .replace('+07:00', '');
      this.EventStatus = this.data.Status;
      this.EventType = this.data.EventType;

    var participants = JSON.parse(this.data.EventParticipantsJson);
    participants.map(value => {
      console.log('paricipant ', value);
      this.eventusers.push({
        Id: value.id,
        Name: value.displayName,
        Img: '',
        Email: value.mail,
        IsGroup: 0
      });
    })

    
  }
  OnNoClick(): void {
    this.dialogRef.close();
  }
}
