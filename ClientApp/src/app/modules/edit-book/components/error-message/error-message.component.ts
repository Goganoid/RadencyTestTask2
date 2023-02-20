import { Component, Input } from '@angular/core';
// general error messages under fields
@Component({
  selector: 'app-error-message',
  templateUrl: './error-message.component.html',
  styleUrls: ['./error-message.component.css']
})
export class ErrorMessageComponent {
  @Input("control") control: any;
}
