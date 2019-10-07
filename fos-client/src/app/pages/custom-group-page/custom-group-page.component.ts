import {
  Component,
  OnInit,
  ViewChild,
  Injectable,
  OnChanges
} from '@angular/core';
import { MatDrawer, MatSnackBar, MatDialog } from '@angular/material';
import { CustomGroupService } from './../../services/custom-group/custom-group.service';
import { UserService } from 'src/app/services/user/user.service';
import { CustomGroup } from './../../models/custom-group';
import { User } from 'src/app/models/user';
import { MatTableDataSource } from '@angular/material/table';
import { EventDialogConfirmComponent } from 'src/app/components/event-dialog-confirm/event-dialog-confirm.component';

@Component({
  selector: 'app-custom-group-page',
  templateUrl: './custom-group-page.component.html',
  styleUrls: ['./custom-group-page.component.less']
})
export class CustomGroupPageComponent implements OnInit {
  group: CustomGroup;
  isCreateGroup = false;
  allGroup: CustomGroup[];
  currentUser: User;
  displayedColumns: string[] = ['Name', 'Update', 'Delete'];
  dataSource: MatTableDataSource<CustomGroup>;
  isLoading = true;
  tempData = 0;

  @ViewChild(MatDrawer, { static: true }) public nav: MatDrawer;
  constructor(
    private drawer: DrawerService,
    private customGroupService: CustomGroupService,
    private userService: UserService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.dataSource = new MatTableDataSource([]);
    this.drawer.setDrawer(this.nav);
    this.userService.getCurrentUser().then(user => {
      this.currentUser = user;
      this.customGroupService.getAllGroup(user.Id).then(allGroup => {
        this.allGroup = allGroup;
        this.dataSource = new MatTableDataSource(allGroup);
        this.isLoading = false;
      });
    });
  }
  toast(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }
  createGroup() {
    this.tempData++;
    this.isCreateGroup = true;
    this.group = null;
    this.drawer.open();
  }
  updateGroup(element: CustomGroup) {
    this.tempData++;
    this.isCreateGroup = false;
    const groupUpdate = this.allGroup.find(item => {
      return item.ID === element.ID;
    });
    this.group = groupUpdate;
    this.drawer.open();
  }
  removeGroup(element: CustomGroup) {
    const dialogRef = this.dialog.open(EventDialogConfirmComponent, {
      width: '450px',
      data: 'Are you sure you want to delete this group?'
    });
    dialogRef.afterClosed().subscribe(event => {
      if (event) {
        this.customGroupService.deleteGroupById(element.ID).then(result => {
          if (!result) {
            this.toast('Remove group success', 'Dismiss');
          }
          this.loadAllGroup();
        });
      }
    });
  }
  loadAllGroup() {
    this.isLoading = true;
    this.customGroupService.getAllGroup(this.currentUser.Id).then(allGroup => {
      this.allGroup = allGroup;
      this.dataSource = new MatTableDataSource(allGroup);
      this.isLoading = false;
    });
  }
  closeDrawer($event) {
    if ($event.isClose) {
      this.drawer.close();
      if ($event.isChange) {
        this.loadAllGroup();
      }
    }
  }
}

@Injectable({
  providedIn: 'root'
})
export class DrawerService {
  private drawer: MatDrawer;

  public setDrawer(drawer: MatDrawer) {
    this.drawer = drawer;
  }

  public open() {
    return this.drawer.open();
  }

  public close() {
    return this.drawer.close();
  }

  public toggle(): void {
    this.drawer.toggle();
  }
}
