import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PrintService {

  isPrinting = false;
  constructor(private router:Router) { }

  printDocument(documentName: string, documentData: string[], data) {
    this.isPrinting = true;
    this.router.navigate(['/',
      { outlets: {
        'print': ['print', documentName, documentData.join()]
      }}], {
        state: {
          data
        }
      });
  }

  onDataReady(print:boolean) {
    setTimeout(() => {
      if (print) {
        window.print();
        this.isPrinting = false;
        this.router.navigate([{ outlets: { print: null }}]);
      }
    });
  }
}
