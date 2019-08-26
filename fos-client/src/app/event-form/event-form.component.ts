import { Component, OnInit } from '@angular/core';
import { EventUser } from '../eventuser';
import { ThrowStmt } from '@angular/compiler';


@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.less']
})
export class EventFormComponent implements OnInit {
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
  constructor() {
    // this.date = new Date().toLocaleString();
    this.dateTimeToClose = this.toDateString(new Date());
    this.dateToReminder = this.toDateString(new Date());
  }
  private toDateString(date: Date): string {
    return (date.getFullYear().toString() + '-' 
       + ("0" + (date.getMonth() + 1)).slice(-2) + '-' 
       + ("0" + (date.getDate())).slice(-2))
       + 'T' + date.toTimeString().slice(0,5);
}

  ngOnInit() {
    var yearSelect = document.querySelector('#party');

    this.dropdownList = [
      { "id": "jao.felix@gmail.com", "itemName": "Jao Felix" },
      { "id": "jan.oblak@gmail.com", "itemName": "Jan Oblak" },
      { "id": "koke@gmail.com", "itemName": "Koke" },
      { "id": "jimenez@gmail.com", "itemName": "Jimenez" },
      { "id": "lemar@gmail.com", "itemName": "Thomas Lemar" },
      { "id": "diego.costa@gmail.com", "itemName": "Diego Costa" },
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
    for (var s in this.selectedItems) {
      var flag = false;
      for (var e in this.eventusers) {
        if (this.selectedItems[s].itemName == this.eventusers[e].name) {
          flag = true
        }
      }
      if (flag == false) {
        this.eventusers.push({ 'name': this.selectedItems[s].itemName, 'email': this.selectedItems[s].id });
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
        }
      }
      this.selectedItems.splice(i, 1);
      i--;
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

    // var dateSaveToSharePoint = this.dateTimeToClose.getFullYear()+'-'+(this.dateTimeToClose.getMonth()+1)+'-'+this.dateTimeToClose.getDate();
    // var timeSaveToSharePoint = this.dateTimeToClose.getHours() + ":" + this.dateTimeToClose.getMinutes();
    // var dateTimeSaveToSharePoint = dateSaveToSharePoint+' '+timeSaveToSharePoint;


    // var _dateTimeToReminder = this.dateTimeToReminder;

    console.log(this.dateToReminder);
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
