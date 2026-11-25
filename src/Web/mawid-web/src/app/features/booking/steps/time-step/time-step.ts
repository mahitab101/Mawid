import { Component, input, model } from '@angular/core';
import { Slot } from '../../../../core/models/slot';
import { formatTime } from '../../../../core/utils/date-format';

@Component({
  imports: [],
  selector: 'app-time-step',
  styleUrl: './time-step.css',
  templateUrl: './time-step.html',
})
export class TimeStep {
  slots = input<Slot[]>([]);
  isLoading = input(false);
  error = input<string | null>(null);
  timeZone = input.required<string>();
  selected = model<string | null>(null);

  format(iso: string) {
    return formatTime(iso, this.timeZone());
  }
}
