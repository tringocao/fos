import { Component, OnInit } from '@angular/core';
import { EventUser } from '../services/event-form/eventuser';
import { ThrowStmt } from '@angular/compiler';
import { HttpClient } from '@angular/common/http';
import { EventFormService } from '../services/event-form/event-form.service';
import { ViewChild } from '@angular/core';
 
import { MatDialog, MatTable } from '@angular/material';
import { Alert } from 'selenium-webdriver';
import { environment } from 'src/environments/environment';

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

    // eventFormService.getCurrentUser(this.Host).then(_host => {
    //   console.log("1" + _host);
    //   this.Host = _host
    //   console.log("2" + this.Host);
    // });
    
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
        // console.log(counter);
        // console.log(counter.displayName);
        this.dropdownList.push({ 'itemName': counter.displayName, 'id': counter.mail });
        // this.users.push({'name': counter.displayName, 'email': counter.mail });
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

    // this.http.get('https://localhost:44398/api/SPUser/GetCurrentUser').subscribe(data => {
    //   console.log("request data");
    //   var objects = JSON.stringify(data);
    //   // console.log(objects);
    //   var jsonData = JSON.parse(objects);
    //   // console.log(jsonData);
    //   for (var i = 0; i < jsonData.value.length; i++) {
        
    //     var counter = jsonData.value[i];
    //     // console.log(counter);
    //     // console.log(counter.displayName);
    //     this.dropdownList.push({ 'itemName': counter.displayName, 'id': counter.mail });
    //     // this.table.renderRows();
    //   }
    // });
    
    // this.http.post(environment.apiUrl + 'api/web/ensureUser/',
    // {logonName: "admin@devpreciovn.onmicrosoft.com"})
    //   .subscribe(
    //     (val) => {
    //       console.log("POST call successful value returned in body",
    //         val);
    //     },
    //     response => {
    //       console.log("POST call in error", response);
    //     },
    //     () => {
    //       console.log("The POST observable is now completed.");
    //     });
    


    let a = JSON.stringify( {fields: {
      EventTitle: this.EventTitle,
      EventId: '1',
      EventRestaurant: this.Restaurant,
      EventMaximumBudget: this.maximunBudget,
      EventTimeToClose: this.dateTimeToClose.replace("T"," "),
      EventTimeToReminder: this.dateToReminder.replace("T"," "),
      // EventHostLookupId:[10,23],
    }}).toString();
    console.log(a)
    this.http.post(environment.apiUrl + 'api/SPList/AddListItem/3a8b82cb-655b-429c-a774-9a3d2af07289',
    {data: a})
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
