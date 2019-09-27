import { Component, OnInit, Inject } from '@angular/core';
import { User } from 'src/app/models/user';
import { environment } from 'src/environments/environment';
import { MatTableDataSource } from '@angular/material';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'app-users-ordered-food-dialog',
  templateUrl: './users-ordered-food-dialog.component.html',
  styleUrls: ['./users-ordered-food-dialog.component.less']
})
export class UsersOrderedFoodDialogComponent implements OnInit {
  graphUserNotOrder: User[] = [];
  displayedColumns = ['avatar', 'Name', 'Email'];
  dataSource: MatTableDataSource<User>;
  apiUrl = environment.apiUrl;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<UsersOrderedFoodDialogComponent>,
    private overlayContainer: OverlayContainer
  ) {
    this.overlayContainer
      .getContainerElement()
      .classList.add('app-theme1-theme');
  }

  ngOnInit() {
    console.log('data: ', this.data);
    this.dataSource = new MatTableDataSource(this.data.users);
  }

  closeDialog($event) {
    this.dialogRef.close();
  }
}
