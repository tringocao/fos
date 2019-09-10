import { Injectable } from '@angular/core';
import { HttpClient, XhrFactory } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SummaryService {

  constructor(private http:HttpClient) { }

  sendEmail(html:string) : Promise<void> {
    return new Promise<void>((resolve,reject)=> {
      this.http.post<ApiOperationResult<void>>(environment.apiUrl + 'api/summary/sendreport', 
        {html}
      )
      .toPromise().then(result => {
      if(result.Success){resolve(result.Data)}
      else reject(new Error(JSON.stringify(result.ErrorMessage)));        
    }).catch(alert => console.log(alert))
  });
  }

  getEventById(id:string):Promise<any> {
    return new Promise<any>((resolve,reject)=> {
      this.http.get<ApiOperationResult<any>>(environment.apiUrl + 'api/splist/getevent/' + id)
      .toPromise().then(result => {
      if(result.Success){resolve(result.Data)}
      else reject(new Error(JSON.stringify(result.ErrorMessage)));        
    }).catch(alert => console.log(alert))
  });
  }
}
