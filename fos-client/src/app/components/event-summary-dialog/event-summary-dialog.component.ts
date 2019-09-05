import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef  } from '@angular/material/dialog';
import { EventList } from 'src/app/models/eventList';

@Component({
  selector: 'app-event-summary-dialog',
  templateUrl: './event-summary-dialog.component.html',
  styleUrls: ['./event-summary-dialog.component.less'],
//   providers: [
//     { provide: MAT_DIALOG_DATA, useValue: {} },
//     { provide: MatDialogRef, useValue: {} }
// ]
})
export class EventSummaryDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: EventList) {
    console.log(data);
  }

  eventDetail: EventList;

  ngOnInit() {
    console.log('ass', this.data)
    this.eventDetail = this.data;
  }

}
