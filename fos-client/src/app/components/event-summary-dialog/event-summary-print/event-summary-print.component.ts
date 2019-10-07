import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
  Input
} from "@angular/core";
import { Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material";
import * as jsPDF from "jspdf";
// import * as printJs from 'printjs';
import html2canvas from "html2canvas";
import * as moment from "moment";
import "moment/locale/vi";
import { Event } from "src/app/models/event";

import { PrintService } from "src/app/services/print/print.service";
import { User } from 'src/app/models/user';

@Component({
  selector: "app-event-summary-print",
  templateUrl: "./event-summary-print.component.html",
  styleUrls: ["./event-summary-print.component.less"]
})
export class EventSummaryPrintComponent implements OnInit {
  @ViewChild("personGroupView", { static: false }) userGroupTab: ElementRef;

  @Input() eventData;

  constructor(private router: Router, private printService: PrintService) {
    this.data = this.router.getCurrentNavigation()
      ? this.router.getCurrentNavigation().extras.state.data
      : null;

    
  }
  data: any;
  eventDataAvailable: boolean;
  dishViewDataAvailable: boolean;
  personViewDataAvailable: boolean;
  printMode: boolean;
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);

  dishGroupViewDisplayedColumns: string[] = [
    "name",
    "amount",
    "price",
    "total",
    "totalComment",
    'showUsers'
  ];
  personGroupViewDisplayedColumns: string[] = [
    "user",
    "food",
    "price",
    "pay-extra",
    "comment"
  ];

  restaurant: any;
  users: User[];
  eventDetail: Event;
  foods: any[];
  orderByDish: any[] = [];
  orderByPerson: any[] = [];
  total: number;

  toStandardDate(date: Date) {
    return moment(date).format("MM/DD/YYYY HH:mm");
  }

  ngOnInit() {
    if (this.data == null && this.eventData) {
      this.data = this.eventData;
      this.printService.onDataReady(false);
    } else {
      this.printService.onDataReady(true);
    }
    this.eventDetail = this.data.eventDetail;
    this.restaurant = this.data.restaurant;
    this.dishGroupViewdataSource = this.data.foods;
    this.personGroupViewdataSource = this.data.orderByPerson;
    this.users = this.data.users;
    this.total = this.data.total;

    this.eventDataAvailable = true;
    this.restaurant.isLoaded = true;
    this.personViewDataAvailable = true;
    this.dishViewDataAvailable = true;
  }

  getOrderedUsersNames(userIds: string[]): string[] {
    let orderUsersNames: string[] = [];
    userIds.forEach(id => {
      this.users.forEach(user => {
        if (user.Id === id) {
          orderUsersNames.push(user.DisplayName);
        }
      });
    });
    return orderUsersNames;
  }

  numberWithCommas(x: Number) {
    if (x < 0) return 0;
    if (x != undefined) {
      var parts = x.toString().split(".");
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      return parts.join(".");
    }
  }
}
