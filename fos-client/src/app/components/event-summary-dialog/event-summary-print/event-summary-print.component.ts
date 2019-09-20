import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef
} from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import * as jsPDF from 'jspdf';
// import * as printJs from 'printjs';
import html2canvas from 'html2canvas';
import * as moment from 'moment';
import 'moment/locale/vi';
import { Event } from "src/app/models/event";

import { PrintService } from 'src/app/services/print/print.service';

@Component({
  selector: 'app-event-summary-print',
  templateUrl: './event-summary-print.component.html',
  styleUrls: ['./event-summary-print.component.less']
})
export class EventSummaryPrintComponent implements OnInit {

  @ViewChild('personGroupView', { static: false }) userGroupTab: ElementRef;
 
  constructor(
    private router: Router,
    private printService: PrintService
  ) {
    this.data = this.router.getCurrentNavigation().extras.state.data;
  }
  data:any;
  eventDataAvailable:boolean;
  dishViewDataAvailable:boolean;
  personViewDataAvailable:boolean;
  printMode:boolean;
  dishGroupViewdataSource: any = new MatTableDataSource([]);
  personGroupViewdataSource: any = new MatTableDataSource([]);
 
  dishGroupViewDisplayedColumns: string[] = [
    'name',
    'amount',
    'price',
    'total',
    'totalComment'
  ];
  personGroupViewDisplayedColumns: string[] = [
    'user',
    'food',
    'price',
    'pay-extra',
    'comment'
  ];
 
  restaurant: any;
 
  eventDetail: Event;
  foods: any[];
  orderByDish: any[] = [];
  orderByPerson: any[] = [];
 
  toStandardDate(date: Date) {
    return moment(date).format('DD/MM/YYYY HH:mm');
  }
 
  ngOnInit() {
    this.eventDetail = this.data.eventDetail;
    this.restaurant = this.data.restaurant;
    this.dishGroupViewdataSource = this.data.foods;
    this.personGroupViewdataSource = this.data.orderByPerson;

    this.printService.onDataReady();

    this.eventDataAvailable = true;
    this.restaurant.isLoaded = true;
    this.personViewDataAvailable = true;
    this.dishViewDataAvailable = true;
  }
}
