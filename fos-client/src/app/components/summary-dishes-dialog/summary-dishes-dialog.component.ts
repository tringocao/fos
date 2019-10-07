import { Component, OnInit, ViewChild, Inject, OnDestroy } from '@angular/core';
import {
  MatTableDataSource,
  MatPaginator,
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef
} from '@angular/material';
import { RestaurantSummary } from 'src/app/models/restaurant-summary';
import { SummaryService } from 'src/app/services/summary/summary.service';
import {
  trigger,
  state,
  style,
  transition,
  animate
} from '@angular/animations';
import { DishesSummary } from './../../models/dishes-summary';
import { OverlayContainer } from '@angular/cdk/overlay';
import { TablePaging } from 'src/app/models/table-paging';
import { DataRoutingService } from 'src/app/data-routing.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-summary-dishes-dialog',
  templateUrl: './summary-dishes-dialog.component.html',
  styleUrls: ['./summary-dishes-dialog.component.less'],
  animations: [
    trigger('changeState', [
      state(
        'hasData',
        style({
          width: '100%'
        })
      ),
      transition('* => hasData', animate('500ms'))
    ])
  ]
})
export class SummaryDishesDialogComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  displayedColumns: string[] = [
    'Rank',
    'Food',
    'Percent',
    'AppearTime',
    'RelativePercent'
  ];
  dataSource: MatTableDataSource<DishesSummary>;
  isLoading = true;
  pageSize: number;
  constructor(
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: RestaurantSummary,
    public dialogRef: MatDialogRef<DishesSummary>,
    public summaryService: SummaryService,
    private overlayContainer: OverlayContainer
    ,
    private dataRouting: DataRoutingService
  ) {
    this.getNavTitleSubscription = this.dataRouting
      .getNavTitle()
      .subscribe((appTheme: string) => (this.appTheme = appTheme));
    overlayContainer
      .getContainerElement()
      .classList.add("app-" + this.appTheme + "-theme");
  }
  ngOnDestroy() {
    // You have to `unsubscribe()` from subscription on destroy to avoid some kind of errors
    this.getNavTitleSubscription.unsubscribe();
  }
  private getNavTitleSubscription: Subscription;
  appTheme: string;

  ngOnInit() {
    this.summaryService
      .getDishesSummary(
        this.data.RestaurantId.toString(),
        this.data.DeliveryId.toString(),
        this.data.ServiceId
      )
      .then(result => {
        this.dataSource = new MatTableDataSource<DishesSummary>(result);
        this.dataSource.paginator = this.paginator;
        this.isLoading = false;
      });
  }

  closeDialog($event) {
    this.dialogRef.close();
  }
}
