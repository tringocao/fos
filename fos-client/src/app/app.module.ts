import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestComponent } from './components/test/test.component';
import { OrderService } from './services/order/order.service';
import { HttpClientModule } from '@angular/common/http';
import { ListRestaurantComponent } from './components/list-restaurant/list-restaurant.component';
import { ServiceTabComponent } from './components/service-tab/service-tab.component';
import { RestaurantsPageComponent } from './pages/restaurants-page/restaurants-page.component';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './auth/TokenInterceptor';

import { CookieService } from 'ngx-cookie-service';
import { AuthService } from './auth/auth.service';

import { HeaderComponent } from './components/navigation/header/header.component';
import { SidenavListComponent } from './components/navigation/sidenav-list/sidenav-list.component';

import { OrdersPageComponent } from './pages/orders-page/orders-page.component';
import { MealsPageComponent } from './pages/meals-page/meals-page.component';

import {
  MatTableModule,
  MatSortModule,
  MatPaginatorModule,
  MatTabsModule,
  MatSidenavModule,
  MatListModule,
  MatInputModule
} from '@angular/material';
import { EventFormComponent } from './event-form/event-form.component';
import { DlDateTimeDateModule, DlDateTimePickerModule } from 'angular-bootstrap-datetimepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularDateTimePickerModule } from 'angular2-datetimepicker';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { SearchComponent } from './components/search/search.component';

import {
  MatButtonModule, MatCardModule, MatDialogModule,
  MatToolbarModule, MatMenuModule,MatIconModule, MatProgressSpinnerModule
} from '@angular/material';

import {MatSelectModule} from '@angular/material/select';
import {MatGridListModule} from '@angular/material/grid-list';
import { SelectAutocompleteModule } from 'mat-select-autocomplete';


@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    ListRestaurantComponent,
    RestaurantsPageComponent,
    ServiceTabComponent,
    HeaderComponent,
    SidenavListComponent,
    EventFormComponent,
    OrdersPageComponent,
    MealsPageComponent,
    SearchComponent
  ],
    // declarations: [
    //     AppComponent,
    //     TestComponent,
    //     ListRestaurantComponent,
    //     RestaurantsPageComponent,
    //     ServiceTabComponent,
    //     EventFormComponent
    // ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DlDateTimeDateModule,
    DlDateTimePickerModule,
    FormsModule,
    AngularDateTimePickerModule,
    AngularMultiSelectModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatGridListModule,
    SelectAutocompleteModule,
    MatSortModule,
    MatPaginatorModule,
    MatTabsModule,
    HttpClientModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatTabsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule
  ],
  providers: [
    OrderService, 
    CookieService,
    AuthService,
    {
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  exports: [HeaderComponent, MatToolbarModule, MatButtonModule, MatIconModule],
  bootstrap: [AppComponent]
})
export class AppModule {}
