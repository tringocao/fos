import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  @Output() public sidenavToggle = new EventEmitter();
  title = 'angular-theme';
  appId = 'theme1';
  constructor() {}

  ngOnInit() {}

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
    // tslint:disable-next-line:semicolon
  };
  switchTheme(appId: string) {
    this.appId = appId;
  }

}
