import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { OrdersPageComponent } from './pages/orders-page/orders-page.component';
import { MealsPageComponent } from './pages/meals-page/meals-page.component';
import { OrderDetailComponent } from './components/order-detail/order-detail.component';


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
    path: 'orders',
    component: OrdersPageComponent
  },
  {
    path: 'meals',
    component: MealsPageComponent
  }
];



@NgModule({
  imports: [RouterModule.forRoot(routes),],
  exports: [RouterModule]
})
export class AppRoutingModule {}
