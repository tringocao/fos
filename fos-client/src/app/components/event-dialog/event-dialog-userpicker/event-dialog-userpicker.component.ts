import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
  ValidatorFn,
  FormGroupDirective,
  NgForm
} from "@angular/forms";
import { debounceTime, tap, switchMap, finalize } from "rxjs/operators";
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { Group } from 'src/app/models/group';
import { environment } from 'src/environments/environment';
export interface userPicker {
  Name: string;
  Email: string;
  Img: string;
  Id: string;
  IsGroup: number;
}

@Component({
  selector: 'app-event-dialog-userpicker',
  templateUrl: './event-dialog-userpicker.component.html',
  styleUrls: ['./event-dialog-userpicker.component.less']
})

export class EventDialogUserpickerComponent implements OnInit {
  @Output() ListenChildComponentEvent = new EventEmitter<Array<userPicker>>();
  @Input() formGroup: FormGroup;
  @Input() _pickupTitle: String;
  @Input() _formControlName: string;

  apiUrl = environment.apiUrl;
  displayUser(user: userPicker) {
    if (user) {
      return user.Name;
    }
  }
  _isHostLoading = false;
  _userHost: userPicker[];

  constructor(private eventFormService: EventFormService,) { }
 
  ngOnInit() {
    var self = this;

    self.formGroup
      .get(self._formControlName)
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this._isHostLoading = true)),
        switchMap(value => 
          this.eventFormService
            .SearchGroupByName(value)
            .pipe(finalize(() => {
              
              var finalCode = this.formGroup.get("userInputPicker").value;
              this.ListenChildComponentEvent.emit(finalCode);
              this._isHostLoading = false;
            }))
        )
      )
      .subscribe((data: ApiOperationResult<Array<Group>>) => {
        if (data && data.Data) {
          var dataSourceTemp: userPicker[] = [];
          console.log(data.Data);

          data.Data.map(user => {
            if (user.DisplayName) {
              dataSourceTemp.push({
                Name: user.DisplayName,
                Email: user.DisplayName,
                Img: "",
                Id: user.Id,
                IsGroup: 0
              });
            }
          });

          self._userHost = dataSourceTemp;
          self._isHostLoading = false;
          
        } 
      });
  }

}
