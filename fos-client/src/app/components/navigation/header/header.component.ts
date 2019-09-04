import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  OnChanges
} from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  @Output() public sidenavToggle = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
    // tslint:disable-next-line:semicolon
  };
}
