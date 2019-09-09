import { Component, OnInit, ElementRef, HostListener } from '@angular/core';
import { TooltipPosition } from '@angular/material';
import { FormControl } from '@angular/forms';

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

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.eRef.nativeElement.contains(event.target)) {
      console.log('event: ', event);
    } else {
      this.isShowListAction = false;
    }
  }

  constructor(private eRef: ElementRef) {}

  ngOnInit() {
    this.isShowListAction = false;
  }

  showListAction($event) {
    event.stopPropagation();
    this.isShowListAction = !this.isShowListAction;
  }
}
