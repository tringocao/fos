import {
  Component,
  OnInit,
  Input,
  ViewChild,
  OnChanges,
  Inject
} from '@angular/core';
import {
  MatTableDataSource,
  MatSort,
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

  constructor(
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: RestaurantSummary,
    public dialogRef: MatDialogRef<DishesSummary>,
    public summaryService: SummaryService
  ) {}

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
