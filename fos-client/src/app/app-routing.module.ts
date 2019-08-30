import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { EventFormComponent } from './event-form/event-form.component';
import { EventFormReadonlyComponent } from './event-form-readonly/event-form-readonly.component';
import { OrdersPageComponent } from './pages/orders-page/orders-page.component';
import { MealsPageComponent } from './pages/meals-page/meals-page.component';


const routes: Routes = [
  {
    path: 'event-form',
    component: EventFormComponent
  },
  {
    path: 'event-form-readonly',
    component: EventFormReadonlyComponent
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
