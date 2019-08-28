import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { EventFormComponent } from './event-form/event-form.component';

const routes: Routes = [
    {
        path: 'restaurants',
        component: RestaurantsPageComponent
    },
    {
        path: '',
        component: RestaurantsPageComponent
    },
    {
        path: 'event-form',
        component: EventFormComponent
    },
  {
    path: 'home',
    component: RestaurantsPageComponent
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  }
];



@NgModule({
  imports: [RouterModule.forRoot(routes),],
  exports: [RouterModule]
})
export class AppRoutingModule {}
