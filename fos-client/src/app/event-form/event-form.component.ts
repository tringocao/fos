import { Component, OnInit } from '@angular/core';
import { EventUser } from '../eventuser';

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

  dateTimeToClose: Date = new Date();
  dateTimeToReminder: Date = new Date();
  settings = {
    bigBanner: true,
    timePicker: true,
    format: 'dd-MM-yyyy hh:mm',
    // defaultOpen: true
  }
  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};
  constructor() {

  }

  ngOnInit() {
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
    for (var k in this.selectedItems) {
      var flag = false;
      for (var i in this.eventusers) {
        if (this.selectedItems[k].itemName == this.eventusers[i].name) {
          flag = true
        }
      }
      if(flag == false){
        this.eventusers.push({ 'name': this.selectedItems[k].itemName, 'email': this.selectedItems[k].id });
      }
    }
  }
  showSelectValue(id: string) {
    //getted from event
    console.log("option thay doi");

    //getted from binding
    // console.log(this.number)
  }
  showValue(): void {

    var _dateTimeToClose = this.dateTimeToClose;
    var _dateTimeToReminder = this.dateTimeToReminder;

    console.log(this.dateTimeToClose);
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
