import { Component, OnInit } from '@angular/core';
import { FeedbackService } from 'src/app/services/feedback/feedback.service';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.less']
})
export class FeedbackComponent implements OnInit {

  constructor(private feedbackService:FeedbackService) { }

  ngOnInit() {
    this.feedbackService.getFeedbackById('695')
  }

}
