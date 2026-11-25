import { Component, input, model } from '@angular/core';

export interface DayOption {
  value: string;
  dayShort: string;
  dayNumber: number;
  isOpen: boolean;
}

@Component({
  imports: [],
  selector: 'app-date-step',
  styleUrl: './date-step.css',
  templateUrl: './date-step.html',
})
export class DateStep {
  days = input<DayOption[]>([]);
  selected = model<string | null>(null);

  dayClass(d: DayOption) {
    if (!d.isOpen) return 'border border-dashed border-slate-300 bg-slate-50 text-slate-500 cursor-not-allowed';
    if (this.selected() === d.value) return 'border-2 border-primary bg-primary text-white';
    return 'border border-slate-200 bg-white text-slate-900 hover:border-slate-400 cursor-pointer';
  }
}