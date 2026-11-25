import { Component, input, model } from '@angular/core';
import { BookingService } from '../../../../core/models/service';

@Component({
  selector: 'app-service-step',
  templateUrl: './service-step.html',
})
export class ServiceStep {
  services = input<BookingService[]>([]);
  isLoading = input(false);
  hasError = input(false);
  selected = model<string | null>(null);
}