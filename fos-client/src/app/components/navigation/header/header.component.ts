import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  OnChanges
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { SettingDialogComponent } from './setting-dialog/setting-dialog.component';
import { Overlay } from '@angular/cdk/overlay';
import { UserService } from 'src/app/services/user/user.service';
import { User } from 'src/app/models/user';
import { OauthService } from 'src/app/services/oauth/oauth.service';
import {
  Router,
  NavigationStart,
  NavigationEnd,
  NavigationError,
  Event
} from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  @Output() public sidenavToggle = new EventEmitter();
  title = 'angular-theme';
  appId = 'theme1';
  user: User;
  isUserLoaded: boolean = false;
  @Output() change = new EventEmitter();
  url: string;

  constructor(
    public dialog: MatDialog,
    private userService: UserService,
    private overlay: Overlay,
    private oauthService: OauthService,
    private router: Router
  ) {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this.url = event.url;
      }
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(SettingDialogComponent, {
      scrollStrategy: this.overlay.scrollStrategies.noop(),
      autoFocus: false,
      maxHeight: '98vh',
      width: '80%',
      data: this.user
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  ngOnInit() {
    this.userService.getCurrentUserId().then(user => {
      this.user = user;
      this.isUserLoaded = true;
    });
  }

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
    // tslint:disable-next-line:semicolon
  };
  switchTheme(appId: string) {
    this.appId = appId;
    this.change.emit({ theme: appId });
  }
  logOut() {
    this.oauthService.logOut();
  }
}
