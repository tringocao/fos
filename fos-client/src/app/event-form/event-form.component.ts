import { Component, OnInit } from '@angular/core';
import { EventUser } from '../services/event-form/eventuser';
import { ThrowStmt } from '@angular/compiler';
import { HttpClient } from '@angular/common/http';
import { EventFormService } from '../services/event-form/event-form.service';
import { ViewChild } from '@angular/core';
 
import { MatDialog, MatTable } from '@angular/material';
import { Alert } from 'selenium-webdriver';
import { environment } from 'src/environments/environment';
import {EventList} from 'src/app/models/eventList';
export interface User {
  name: string;
  email: string;
}
@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.less']
})
export class EventFormComponent implements OnInit {
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;
  selectedUser = 'admin';
  eventusers: EventUser[] = [
    // { name: 'Salah', email: 'Liverpool' },
    // { name: 'Kane', email: 'Tottenham Hospur' },
    // { name: 'Hazard', email: 'Real Madrid' },
    // { name: 'Griezmann', email: 'Barcelona' },
  ];
  items: any[] = [
    { id: 0, name: 'one' },
    { id: 1, name: 'two' },
    { id: 2, name: 'three' },
    { id: 3, name: 'four' },
    { id: 4, name: 'five' },
    { id: 5, name: 'six}' }
  ];
  selected: number = 0;
  EventTitle: string ="";
  Host: string ="Host";
  Restaurant: string = "Restaurant";
  maximunBudget: number = 0;
  // dateTimeToClose: Date = new Date();
  // dateTimeToReminder: Date = new Date();
  settings = {
    bigBanner: true,
    timePicker: true,
    format: 'dd-MM-yyyy HH:mm',
    // defaultOpen: true
  }
  dropdownList = [];
  selectedItems = [
    { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" }
  ];
  selectedNewUser = [];
  dropdownListNewUser = [
    { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" },
      { "id": "jan.oblak@gmail.com", "itemName": "Jan Oblak" },
      { "id": "koke@gmail.com", "itemName": "Koke" },
      { "id": "jimenez@gmail.com", "itemName": "Jimenez" },
      { "id": "lemar@gmail.com", "itemName": "Thomas Lemar" },
      { "id": "diego.costa@gmail.com", "itemName": "Diego Costa" },
  ];
  dropdownSettings = {};

  dateTimeToClose: string;
  dateToReminder: string;

  eventforms: EventUser[];

  displayedColumns = ['name', 'email'];
  

 
  
  dataSource: EventUser[] = [
    { name: 'Salah', email: 'Liverpool' },
    { name: 'Kane', email: 'Tottenham Hospur' },
    { name: 'Hazard', email: 'Real Madrid' },
    { name: 'Griezmann', email: 'Barcelona' },
  ];

  
  users: User[] = [
    {name: 'admin', email: 'admin'}
  ];

  

  constructor(private http: HttpClient,private eventFormService: EventFormService) {
    // this.date = new Date().toLocaleString();
    this.dateTimeToClose = this.toDateString(new Date());
    this.dateToReminder = this.toDateString(new Date());

    
    // console.log("" + this.Host);
        this.http.get('https://localhost:44398/api/SPUser/GetCurrentUser').subscribe(data => {
            console.log("get current User");
            // console.log(data.displayName);
            var objects = JSON.stringify(data);
            // console.log(objects);
            var jsonData = JSON.parse(objects);
            this.selectedNewUser.push({'itemName':jsonData.displayName,'id': jsonData.mail});
            this.Host = jsonData.displayName
            this.selectedUser = jsonData.displayName;
        });


    console.log("request data");

    this.http.get('https://localhost:44398/api/SPUser/GetUsers').subscribe(data => {
      console.log("request data");
      var objects = JSON.stringify(data);
      // console.log(objects);
      var jsonData = JSON.parse(objects);
      // console.log(jsonData);
      for (var i = 0; i < jsonData.value.length; i++) {
        
        var counter = jsonData.value[i];
        this.dropdownList.push({ 'itemName': counter.displayName, 'id': counter.mail });
      }
    });
  }
  private toDateString(date: Date): string {
    return (date.getFullYear().toString() + '-' 
       + ("0" + (date.getMonth() + 1)).slice(-2) + '-' 
       + ("0" + (date.getDate())).slice(-2))
       + 'T' + date.toTimeString().slice(0,5);
}

  ngOnInit() {
    // var yearSelect = document.querySelector('#party');

    this.dropdownList = [
      // { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" },
      // { "id": "jan.oblak@gmail.com", "itemName": "Jan Oblak" },
      // { "id": "koke@gmail.com", "itemName": "Koke" },
      // { "id": "jimenez@gmail.com", "itemName": "Jimenez" },
      // { "id": "lemar@gmail.com", "itemName": "Thomas Lemar" },
      // { "id": "diego.costa@gmail.com", "itemName": "Diego Costa" },
    ];
    this.selectedNewUser = [
      // { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" },
      // { "id": 3, "itemName": "Australia" },
      // { "id": 4, "itemName": "Canada" },
      // { "id": 5, "itemName": "South Korea" }
    ];
    
    this.dropdownSettings = {
      singleSelection: true,
      text: "Select user",
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      enableSearchFilter: true,
      classes: "myclass custom-class"
    };

  }
  AddUserToTable(): void {
    console.log("Nhan add card");    

    console.log(this.eventusers);
    for (var s in this.selectedItems) {
      var flag = false;
      for (var e in this.eventusers) {
        if (this.selectedItems[s].itemName == this.eventusers[e].name) {
          flag = true
        }
      }
      if (flag == false) {
        this.eventusers.push({ 'name': this.selectedItems[s].itemName, 'email': this.selectedItems[s].id });
        this.table.renderRows();
      }
    }
  }

  DeleteUserInTable(): void {

    for (var i = 0; i < this.selectedItems.length; i++) {
      console.log(this.selectedItems[i].id);


      for (var j = 0; j < this.eventusers.length; j++) {
        if (this.selectedItems[i].itemName == this.eventusers[j].name) {
          this.eventusers.splice(j, 1);

          j--;
          this.table.renderRows();
        }
      }
      this.selectedItems.splice(i, 1);
      i--;
      // this.table.renderRows();
    }


    // this.eventusers = [];
    // this.selectedItems = [];
  }

  showSelectValue(id: string) {
    console.log("option thay doi");
  }
  SaveToSharePointEventList(): void {
    var eventlistitem: EventList = {
      eventTitle: "FIKA",
      eventId: 'FIKA1',
      eventRestaurant: 'Now',
      eventMaximumBudget: 0,
      eventTimeToClose: this.dateTimeToClose,
      eventTimeToReminder: this.dateToReminder,
      eventHost: 'i:0#.f|membership|admin@devpreciovn.onmicrosoft.com'
    };
    this.eventFormService.addEventListItem(eventlistitem)    
  }
  onItemSelect(item: any) {
    console.log(item);
    console.log(this.selectedItems);
  }
  OnItemDeSelect(item: any) {
    console.log(item);
    console.log(this.selectedItems);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
  onDeSelectAll(items: any) {
    console.log(items);
  }
}
