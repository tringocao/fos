import { Injectable, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
// import decode from 'jwt-decode';

@Injectable()
export class AuthService {

// cookieValue = 'UNKNOWN';
 
  constructor( private cookieService: CookieService ) { }

//   ngOnInit(): void {
//     // this.cookieService.set( 'Test', 'Hello World' );
//     this.cookieValue = this.cookieService.get('Test');
//   }

  public getToken(): string {
    var token = this.cookieService.get('token_key');
    return token || "";
  }

  public isAuthenticated(): boolean {
    // get the token
    const token = this.getToken();
    // return a boolean indicating whether or not the token is expired
    return true;
  }

}