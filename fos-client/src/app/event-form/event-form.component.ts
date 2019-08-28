import { Component, OnInit } from '@angular/core';
import { EventUser } from '../services/event-form/eventuser';
import { ThrowStmt } from '@angular/compiler';
import { HttpClient } from '@angular/common/http';
import { EventFormService } from '../services/event-form/event-form.service';
import { ViewChild } from '@angular/core';
 
import { MatDialog, MatTable } from '@angular/material';
import { Alert } from 'selenium-webdriver';

import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.less']
})
export class EventFormComponent implements OnInit {
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;

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
  EventTitle: string ="Event Title";
  Host: string ="Host";
  Restaurant: string = "Restaurant";
  maximunBudget: number = 100000;
  // dateTimeToClose: Date = new Date();
  // dateTimeToReminder: Date = new Date();
  settings = {
    bigBanner: true,
    timePicker: true,
    format: 'dd-MM-yyyy HH:mm',
    // defaultOpen: true
  }
  dropdownList = [];
  selectedItems = [];
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

  constructor(private http: HttpClient,private eventFormService: EventFormService) {
    // this.date = new Date().toLocaleString();
    this.dateTimeToClose = this.toDateString(new Date());
    this.dateToReminder = this.toDateString(new Date());

    // this.eventusers = this.eventFormService.getUsers();

    // this.eventFormService.getUsers()
    // .subscribe(eventusers => this.eventusers = eventusers);

    console.log("request data");

    this.http.get(environment.apiUrl + 'api/SPUser/GetUsers').subscribe(data => {
      console.log("request data");
      var objects = JSON.stringify(data);
      // console.log(objects);
      var jsonData = JSON.parse(objects);
      // console.log(jsonData);
      for (var i = 0; i < jsonData.value.length; i++) {
        
        var counter = jsonData.value[i];
        // console.log(counter);
        // console.log(counter.displayName);
        this.dropdownList.push({ 'itemName': counter.displayName, 'id': counter.mail });
        // this.table.renderRows();
      }
    });
  }
  private toDateString(date: Date): string {
    // var dateSaveToSharePoint = date.getFullYear()+'-'+(date.getMonth()+1)+'-'+date.getDate();
    // var timeSaveToSharePoint = date.getHours() + ":" + date.getMinutes();
    // var dateTimeSaveToSharePoint = dateSaveToSharePoint+' '+timeSaveToSharePoint;
    // return dateTimeSaveToSharePoint;

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
    this.selectedItems = [
      // { "id": 2, "itemName": "Singapore" },
      // { "id": 3, "itemName": "Australia" },
      // { "id": 4, "itemName": "Canada" },
      // { "id": 5, "itemName": "South Korea" }
    ];
    this.dropdownSettings = {
      singleSelection: false,
      text: "Select user",
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      enableSearchFilter: true,
      classes: "myclass custom-class"
    };

  }
  AddUserToTable(): void {
    console.log("Nhan add card");
    // this.eventusers.push({ 'name': 'aaaa', 'email': 'aaa' });

    // this.eventusers.push({
    //   name:'abc',
    //   email:'abc'
    // });
    

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
    //getted from event
    console.log("option thay doi");

    //getted from binding
    // console.log(this.number)
  }
  SaveToSharePointEventList(): void {

    // this.http.post<any>('https://localhost:44398/api/SPList/AddListItem/3a8b82cb-655b-429c-a774-9a3d2af07289',
    //   {
    //     EventTitle: this.EventTitle,
    //     EventRestaurant: this.Restaurant,
    //     EventMaximumBudget: this.maximunBudget
    //   },
    // )
    let a = JSON.stringify( {fields: {
      EventTitle: this.EventTitle,
      EventRestaurant: this.Restaurant,
      EventMaximumBudget: this.maximunBudget, 
    }}).toString();
    console.log(a)
    this.http.post(environment.apiUrl + 'api/SPList/AddListItem/3a8b82cb-655b-429c-a774-9a3d2af07289',
    {data: a}
)
      .subscribe(
        (val) => {
          console.log("POST call successful value returned in body",
            val);
        },
        response => {
          console.log("POST call in error", response);
        },
        () => {
          console.log("The POST observable is now completed.");
        });


    console.log("add item to list");
    // var _dateTimeToReminder = this.dateTimeToReminder;

    // console.log(this.dateToReminder.replace("T", " "));
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
    // this.eventusers = [];
  }
}
