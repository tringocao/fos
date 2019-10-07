import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable()
export class DataRoutingService {
  private navTitle$: BehaviorSubject<string> = new BehaviorSubject<string>(
    "theme1"
  );

  public setNavTitle(newNavTitle: string): void {
    // Sets new value, every entity, which is subscribed to changes (`getNavTitle().subscribe(...)`) will get new value every time it changes
    this.navTitle$.next(newNavTitle);
  }

  public getNavTitle(): Observable<string> {
    // Allow to `subscribe` on changes and get the value every time it changes
    return this.navTitle$.asObservable();
  }
}
