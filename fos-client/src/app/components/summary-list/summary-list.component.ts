import { Component, OnInit, Input, ViewChild, OnChanges } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatDialog } from '@angular/material';
import { RestaurantSummary } from 'src/app/models/restaurant-summary';
import { SummaryService } from 'src/app/services/summary/summary.service';
import {
  trigger,
  state,
  style,
  transition,
  animate
} from '@angular/animations';
import { SummaryDishesDialogComponent } from '../summary-dishes-dialog/summary-dishes-dialog.component';

@Component({
  selector: 'app-summary-list',
  templateUrl: './summary-list.component.html',
  styleUrls: ['./summary-list.component.less'],
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
export class SummaryListComponent implements OnInit, OnChanges {
  @Input() index: number;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  displayedColumns: string[] = [
    'Rank',
    'Restaurant',
    'AverageRating',
    'Percent',
    'AppearTime',
    'RelativePercent'
  ];
  dataSource: MatTableDataSource<RestaurantSummary>;
  listRestaurant: RestaurantSummary[];
  isLoading = true;

  constructor(
    private summaryService: SummaryService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.dataSource = new MatTableDataSource<RestaurantSummary>([]);
    this.dataSource.paginator = this.paginator;
    this.summaryService.GetRestaurantSummary().then(result => {
      this.listRestaurant = result;
      if (result !== null && result.length > 0) {
        const tempData = result.filter(r => r.ServiceId === '1');
        this.setDataSource(tempData);
      }
      this.isLoading = false;
    });
  }

  setDataSource(data: RestaurantSummary[]) {
    this.dataSource = new MatTableDataSource(data);
    this.dataSource.paginator = this.paginator;
  }

  ngOnChanges() {
    if (this.index === 1) {
      const tempData = this.listRestaurant.filter(r => r.ServiceId === '1');
      this.setDataSource(tempData);
    }
  }

  showDishesSummary(row: RestaurantSummary) {
    //console.log('row: ', row);

    const dialogRef = this.dialog.open(SummaryDishesDialogComponent, {
      maxHeight: '98vh',
      width: '80%',
      data: row
    });

    dialogRef.afterClosed().subscribe(result => {
      //console.log('The dialog was closed');
    });
  }
}
