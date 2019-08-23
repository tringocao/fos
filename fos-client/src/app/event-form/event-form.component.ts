import { Component, OnInit } from '@angular/core';
import { EventUser } from '../eventuser';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.less']
})
export class EventFormComponent implements OnInit {
  eventusers: EventUser[] = [
    { name: 'Salah', email: 'Liverpool' },
    { name: 'Kane', email: 'Tottenham Hospur' },
    { name: 'Hazard', email: 'Real Madrid' },
    { name: 'Griezmann', email: 'Barcelona' },
];
items: any[] = [
  {id: 9, name :''},
  { id: 0, name: 'one' },
  { id: 1, name: 'two' },
  { id: 2, name: 'three' },
  { id: 3, name: 'four' },
  { id: 4, name: 'five' },
  { id: 5, name: 'six}' }
];
  constructor() { }

  ngOnInit() {
    // var today = new Date();
    // var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    // var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    // var dateTime = date + ' ' + time;
    // console.log(dateTime);
    // // document.getElementById("datetimepicker1").value = "Johnny Bravo";
    // document.getElementById("datetimepicker1").click();
    // console.log("click date time picker");

    
  }
  add(name: string): void {
    console.log(name + "-----");
    this.eventusers.push({ 'name':"new name", 'email': "new email"});
  }

}
