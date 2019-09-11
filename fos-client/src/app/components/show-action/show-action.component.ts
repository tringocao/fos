import {
  Component,
  OnInit,
  ElementRef,
  HostListener,
  Input
} from '@angular/core';
import { TooltipPosition } from '@angular/material';
import { FormControl } from '@angular/forms';
import { EventAction } from './../../models/event-action';

@Component({
  selector: 'app-show-action',
  templateUrl: './show-action.component.html',
  styleUrls: ['./show-action.component.less']
})
export class ShowActionComponent implements OnInit {
  isShowListAction: boolean;
  positionOptions: TooltipPosition[] = [
    'after',
    'before',
    'above',
    'below',
    'left',
    'right'
  ];
  position = new FormControl(this.positionOptions[2]);
  @Input() eventId: string;
  @Input() actionList: EventAction;

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.eRef.nativeElement.contains(event.target)) {
      console.log('click');
    } else {
      this.isShowListAction = false;
    }
  }

  constructor(private eRef: ElementRef) {}

  ngOnInit() {
    this.isShowListAction = false;
  }

  showListAction($event) {
    this.isShowListAction = !this.isShowListAction;
  }

  viewEvent($event) {}

  editEvent($event) {}

  createEvent($event) {}

  makeOrder() {}
}
