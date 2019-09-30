import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { OrdersPageComponent } from './pages/orders-page/orders-page.component';
import { SummaryPageComponent } from './pages/summary-page/summary-page.component';
import { OrderDetailComponent } from './components/order-detail/order-detail.component';
import { EventSummaryDialogComponent } from './components/event-summary-dialog/event-summary-dialog.component';
import { PrintLayoutComponent } from './print-layout/print-layout.component';
import { EventSummaryPrintComponent } from './components/event-summary-dialog/event-summary-print/event-summary-print.component';
import { NotParticipantComponent } from './components/not-participant/not-participant.component';

const routes: Routes = [
  {
    path: 'make-order/:id',
    component: OrderDetailComponent
  },
  {
    path: 'home',
    component: RestaurantsPageComponent
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'events',
    component: OrdersPageComponent
  },
  {
    path: 'summary',
    component: SummaryPageComponent
  },
  {
    path: 'events/summary/:id',
    component: EventSummaryDialogComponent
  },
  { path: 'print',
    outlet: 'print',
    component: PrintLayoutComponent,
    children: [
      { path: 'report/:id', component: EventSummaryPrintComponent }
    ]
  },
  {
    path: 'not-participant/:id',
    component: NotParticipantComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
