import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { OrdersPageComponent } from './pages/orders-page/orders-page.component';
import { MealsPageComponent } from './pages/meals-page/meals-page.component';
import { OrderDetailComponent } from './components/order-detail/order-detail.component';
import { EventSummaryDialogComponent } from './components/event-summary-dialog/event-summary-dialog.component';
import { PrintLayoutComponent } from './print-layout/print-layout.component';

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
    path: 'meals',
    component: MealsPageComponent
  },
  {
    path: 'events/summary/:id',
    component: EventSummaryDialogComponent
  },
  { path: 'print',
    outlet: 'print',
    component: PrintLayoutComponent,
    children: [
      { path: 'report', component: EventSummaryDialogComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
