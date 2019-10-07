import {
  Component,
  OnInit,
  EventEmitter,
  Output,
  Input,
  forwardRef
} from '@angular/core';
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
} from '@angular/forms';
import { debounceTime, tap, switchMap, finalize } from 'rxjs/operators';
import { EventFormService } from 'src/app/services/event-form/event-form.service';
import { Group } from 'src/app/models/group';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/models/user';
export interface UserPicker {
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
  @Output() ListenChildComponentEvent = new EventEmitter<UserPicker>();
  @Input() formGroup: FormGroup;
  @Input() pickupTitle: string;
  @Input() formControlName: string;

  apiUrl = environment.apiUrl;
  isHostLoading = false;
  userHost: UserPicker[];
  displayUser(user: UserPicker) {
    if (user) {
      return user.Name;
    }
  }

  constructor(private eventFormService: EventFormService) {}

  ngOnInit() {
    const self = this;

    self.formGroup
      .get(self.formControlName)
      .valueChanges.pipe(
        debounceTime(300),
        tap(() => (this.isHostLoading = true)),
        switchMap(value =>
          self.eventFormService.SearchGroupOrUserByName(value).pipe(
            finalize(() => {
              const user: UserPicker = self.formGroup.get(self.formControlName)
                .value;
              if (user && user.Email) {
                this.ListenChildComponentEvent.emit(user);
              }

              this.isHostLoading = false;
            })
          )
        )
      )
      .subscribe((data: ApiOperationResult<Array<User>>) => {
        if (data && data.Data) {
          const dataSourceTemp: UserPicker[] = [];
          //console.log(data.Data);

          data.Data.map(user => {
            if (user.DisplayName) {
              dataSourceTemp.push({
                Name: user.DisplayName,
                Email: user.Mail,
                Img: '',
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
