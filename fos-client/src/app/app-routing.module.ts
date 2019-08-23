import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';

const routes: Routes = [
  {
    path: 'restaurants',
    component: RestaurantsPageComponent
  },
  {
    path: '',
    component: RestaurantsPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
