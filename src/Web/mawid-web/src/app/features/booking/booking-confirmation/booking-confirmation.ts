import { Component, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-booking-confirmation',
  styleUrl: './booking-confirmation.css',
  templateUrl: './booking-confirmation.html',
})
export class BookingConfirmation {

  serviceTitle = input.required<string>();
  providerName = input.required<string>();
  when = input.required<string>();
  bookAnother = output<void>();

}
