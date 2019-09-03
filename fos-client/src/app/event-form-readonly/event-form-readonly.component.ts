import { Component, OnInit } from '@angular/core';
// import { EventUser } from '../services/event-form-a/eventuser';
import { ThrowStmt } from '@angular/compiler';
import { HttpClient } from '@angular/common/http';
import { EventFormService } from '../services/event-form-a/event-form.service';
import { ViewChild } from '@angular/core';

import { MatDialog, MatTable } from '@angular/material';
import { Alert } from 'selenium-webdriver';
import { environment } from 'src/environments/environment';
import { EventList } from 'src/app/models/eventList';
import { FormControl } from '@angular/forms';
export interface userPicker {
  name: string;
  email: string;
}
export interface userLoginName {
  name: string;
  loginName: string;
}
export interface userPickerGroup {
  name: string;
  userpicker: userPicker[];
}
export interface User {
  name: string;
  email: string;
}

@Component({
  selector: 'app-event-form-readonly',
  templateUrl: './event-form-readonly.component.html',
  styleUrls: ['./event-form-readonly.component.less']
})
export class EventFormReadonlyComponent implements OnInit {

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  eventFormControl = new FormControl();
  selectedUser = 'admin';
  // eventusers: EventUser[] = [
  //   // { name: 'Salah', email: 'Liverpool' },
  //   // { name: 'Kane', email: 'Tottenham Hospur' },
  //   // { name: 'Hazard', email: 'Real Madrid' },
  //   // { name: 'Griezmann', email: 'Barcelona' },
  // ];
  items: any[] = [
    { id: 0, name: 'one' },
    { id: 1, name: 'two' },
    { id: 2, name: 'three' },
    { id: 3, name: 'four' },
    { id: 4, name: 'five' },
    { id: 5, name: 'six}' }
  ];
  EventTitle: string = "";
  Host: string = "Host";
  Restaurant: string = "Restaurant";
  maximunBudget: number = 0;
  // settings = {
  //   bigBanner: true,
  //   timePicker: true,
  //   format: 'dd-MM-yyyy HH:mm',
  //   // defaultOpen: true
  // }
  dropdownList = [];
  selectedItems = [
    // { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" }
  ];
  selectedNewUser = [];
  dropdownListNewUser = [
    // { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" },
    // { "id": "jan.oblak@gmail.com", "itemName": "Jan Oblak" },
    // { "id": "koke@gmail.com", "itemName": "Koke" },
    // { "id": "jimenez@gmail.com", "itemName": "Jimenez" },
    // { "id": "lemar@gmail.com", "itemName": "Thomas Lemar" },
    // { "id": "diego.costa@gmail.com", "itemName": "Diego Costa" },
  ];
  dropdownSettings = {};

  dateTimeToClose: string;
  dateToReminder: string;

  // eventforms: EventUser[];

  displayedColumns = ['name', 'email'];




  // dataSource: EventUser[] = [
  //   { name: 'Salah', email: 'Liverpool' },
  //   { name: 'Kane', email: 'Tottenham Hospur' },
  //   { name: 'Hazard', email: 'Real Madrid' },
  //   { name: 'Griezmann', email: 'Barcelona' },
  // ];


  users: User[] = [
    { name: 'admin', email: 'admin' }
  ];

  userSelect = [];

  userPickerGroups: userPickerGroup[] = [
    // {
    //   name: 'Office 365 group',
    //   userpicker: [
    //     { name: 'Group1', email: 'group1@gmail.com' },
    //     { name: 'Group2', email: 'group2@gmail.com' },
    //     { name: 'Group3', email: 'group3@gmail.com' },
    //   ],
    // },
    // {
    //   name: 'User',
    //   userpicker: [
    //     { name: 'user1', email: 'user1@gmail.com' },
    //     { name: 'user2', email: 'user2@gmail.com' },
    //     { name: 'user3', email: 'user3@gmail.com' },
    //   ],
    // },
  ];

  office365Group: userPicker[] = [];
  office365User: userPicker[] = [];
  userLogin: userLoginName[] = [];
  HostEmail: string = "";
  constructor(private http: HttpClient, private eventFormService: EventFormService) {
    // this.date = new Date().toLocaleString();
    this.dateTimeToClose = this.toDateString(new Date());
    this.dateToReminder = this.toDateString(new Date());


    // console.log("" + this.Host);
    this.http.get(environment.apiUrl + 'api/SPUser/GetCurrentUser').subscribe(data => {
      console.log("get current User");
      // console.log(data.displayName);
      var objects = JSON.stringify(data);
      // console.log(objects);
      var jsonData = JSON.parse(objects);

      this.selectedNewUser.push({ 'itemName': jsonData.displayName, 'id': jsonData.mail });
      this.HostEmail = jsonData.displayName
      this.selectedUser = jsonData.displayName;
      console.log('principal name' + jsonData.mail);
      this.selectedItems.push({ "id": jsonData.mail, "itemName": jsonData.displayName });
      // this.HostName = jsonData.

    });


    console.log("request data");

    this.http.get(environment.apiUrl + '/api/SPUser/GetUsers').subscribe(data => {
      console.log("request data");
      var objects = JSON.stringify(data);
      // console.log(objects);
      var jsonData = JSON.parse(objects);
      // console.log(jsonData);
      for (var i = 0; i < jsonData.value.length; i++) {
        var counter = jsonData.value[i];
        // console.log("check email: " + counter.displayName);
        if (Boolean(counter.mail)) {
          this.dropdownListNewUser.push({ 'itemName': counter.displayName, 'id': counter.mail });
          this.office365User.push({ name: counter.displayName, email: counter.mail });
          this.userLogin.push({ name: counter.displayName, loginName: counter.userPrincipalName });
        }
        else {
          // console.log("khong co email: " + counter.displayName);
        }
      }
    });

    this.http.get(environment.apiUrl + '/api/SPUser/GetGroups').subscribe(data => {
      console.log("request data");
      var objects = JSON.stringify(data);
      // console.log(objects);
      var jsonData = JSON.parse(objects);
      // console.log(jsonData);
      for (var i = 0; i < jsonData.value.length; i++) {
        var counter = jsonData.value[i];

        if (Boolean(counter.mail)) {
          console.log("check displayname: " + counter.displayName + "check email: " + counter.mail);
          // this.userPickerGroups.push({name: counter.displayName, userpicker: [{ name: counter.displayName, email: counter.mailp }] });
          // this.dropdownListNewUser.push({ 'itemName': counter.displayName, 'id': counter.mail });
          this.office365Group.push({ name: counter.displayName, email: counter.mail });
        }
        else {
          // console.log("khong co email: " + counter.displayName);
        }
      }
    });
    this.userPickerGroups.push({ name: 'Office 365', userpicker: this.office365Group });
    this.userPickerGroups.push({ name: 'User', userpicker: this.office365User });
  }
  private toDateString(date: Date): string {
    return (date.getFullYear().toString() + '-'
      + ("0" + (date.getMonth() + 1)).slice(-2) + '-'
      + ("0" + (date.getDate())).slice(-2))
      + 'T' + date.toTimeString().slice(0, 5);
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
      classes: "myclass custom-class",
    };

  }
  AddUserToTable(): void {
    console.log("Nhan add card");

    console.log(this.userSelect);
    // for (var s in this.selectedItems) {
    //   var flag = false;
    //   for (var e in this.eventusers) {
    //     if (this.selectedItems[s].itemName == this.eventusers[e].name) {
    //       flag = true
    //     }
    //   }
    //   if (flag == false) {
    //     this.eventusers.push({ 'name': this.selectedItems[s].itemName, 'email': this.selectedItems[s].id });
    //     this.table.renderRows();
    //   }
    // }

    // for (var s in this.userSelect) {
    //   var flag = false;
    //   for (var e in this.eventusers) {
    //     if (this.userSelect[s].name == this.eventusers[e].name) {
    //       flag = true
    //     }
    //   }
    //   if (flag == false) {
    //     // this.eventusers.push({ 'name': this.userSelect[s].name, 'email': this.userSelect[s].email });
    //     this.table.renderRows();
    //   }
    // }
  }

  DeleteUserInTable(): void {

    for (var i = 0; i < this.userSelect.length; i++) {
      // console.log(this.userSelect[i].nam);


      // for (var j = 0; j < this.eventusers.length; j++) {
      //   if (this.userSelect[i].name == this.eventusers[j].name) {
      //     this.eventusers.splice(j, 1);

      //     j--;
      //     this.table.renderRows();
      //   }
      // }

      // this.userSelect.splice(i, 1);
      // i--;
      // this.table.renderRows();
    }


    // this.eventusers = [];
    // this.selectedItems = [];
  }

  showSelectValue(id: string) {
    console.log("option thay doi");
  }
  SaveToSharePointEventList(): void {

    // var participantList = "";
    // for (var j = 0; j < this.eventusers.length; j++) {
    //   if (this.eventusers[j].email) {
    //     participantList += this.eventusers[j].email + ", ";
    //   }
    // }

    for (var i = 0; i < this.userLogin.length; i++) {
      console.log(this.HostEmail + " vs " + this.userLogin[i].loginName)
      if (this.HostEmail == this.userLogin[i].name) {
        this.HostEmail = this.userLogin[i].loginName;
      }
    }

    if (!Boolean(this.EventTitle) || !Boolean(this.Restaurant) || !Boolean(this.HostEmail)
      ) {
      console.log('Missing info to submit event');
      return;
    }

    console.log('nguoi host: ' + this.HostEmail);
    // var eventlistitem: EventList = {
    //   eventTitle: this.EventTitle,
    //   eventId: 'FIKA1',
    //   eventRestaurant: this.Restaurant,
    //   eventMaximumBudget: 0,
    //   eventTimeToClose: this.dateTimeToClose,
    //   eventTimeToReminder: this.dateToReminder,
    //   eventHost: this.HostEmail,
    //   // eventParticipants: participantList,
    // };
    // this.eventFormService.addEventListItem(eventlistitem)
  }
  onItemSelect(item: any) {
    console.log(item);
    console.log(this.selectedItems);
    for (var j = 0; j < this.selectedItems.length; j++) {
      console.log('select host name: ' + this.selectedItems[j].id);
      this.HostEmail = this.selectedItems[j].itemName;
    }
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



  userPickerSelected(event) {
    console.log('change value');
    let target = event.source.selected._element.nativeElement;
    let selectedData = {
      value: event.value,
      text: target.innerText.trim()
    };
    console.log(selectedData);
  }

  changeClient(event) {
    let target = event.source.selected._element.nativeElement;
    this.userSelect = [];
    this.userSelect.push({ 'name': target.innerText.trim(), 'email': event.value });
    // this.HostEmail = event.value;
    // console.log('host email ' + this.HostEmail );
  }

}
