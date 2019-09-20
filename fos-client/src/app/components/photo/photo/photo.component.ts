import { Component, OnInit, Input } from '@angular/core';
import { Lightbox, LightboxConfig } from 'ngx-lightbox';

@Component({
  selector: 'app-photo',
  templateUrl: './photo.component.html',
  styleUrls: ['./photo.component.less']
})
export class PhotoComponent implements OnInit {

  constructor(private _lightBox:Lightbox, private _lightBoxConfig:LightboxConfig) {
    _lightBoxConfig.centerVertically = true;
   }

  @Input("src") src:string;
  @Input("width") width:string;
  @Input("height") height:string;

  image:any;

  ngOnInit() {
    this.image = {
      src: this.src.search('s60x60') !== -1 ? this.src.replace('s60x60', 's640x400') : this.src.replace('s120x120', 's640x400'),
      caption: '',
      thumb: this.src
   };
  }
  open(): void {
    // open lightbox
    this._lightBox.open([this.image], 0);
  }
  close(): void {
    // close lightbox programmatically
    this._lightBox.close();
  }

}
