import { Component, OnInit, EventEmitter, Output, Input, forwardRef } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
  ValidatorFn,
  FormGroupDirective,
  NgForm,
  NG_VALUE_ACCESSOR
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
  @Input() pickupTitle: String;
  @Input() formControlName: string;

  apiUrl = environment.apiUrl;
  displayUser(user: userPicker) {
    if (user) {
      return user.Name;
    }
  }
  isHostLoading = false;
  userHost: userPicker[];

  constructor(private eventFormService: EventFormService) {
    
   }
 
  ngOnInit() {
    var self = this;
    self.formGroup
      .get(self.formControlName)
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this.isHostLoading = true)),
        switchMap(value => 
          self.eventFormService
            .SearchGroupByName(value)
            .pipe(finalize(() => {
              
              var finalCode = self.formGroup.get("userInputPicker").value;
              this.ListenChildComponentEvent.emit(finalCode);
              this.isHostLoading = false;
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

          self.userHost = dataSourceTemp;
          self.isHostLoading = false;
          
        } 
      });
  }

}
