import { Component, computed, inject, linkedSignal, signal } from '@angular/core';
import { ApiResponse } from '../../../core/models/api-response';
import { environment } from '../../../../environments/environment.development';
import { ServiceProvider } from '../../../core/models/serviceProvider';
import { httpResource } from '@angular/common/http';
import { BookingService } from '../../../core/models/service';
import { WorkingHours } from '../../../core/models/working-hours';
import { BusinessStore } from '../../../core/services/business-store';
@Component({
  imports: [],
  selector: 'app-booking-page',
  styleUrl: './booking-page.css',
  templateUrl: './booking-page.html',
})
export class BookingPage {

providers = httpResource<ApiResponse<ServiceProvider[]>>(() => `${environment.apiUrl}/providers`);
selectedProviderId = signal<string | null>(null);
selectedServiceId = linkedSignal({
  source: this.selectedProviderId,
  computation: () => null as string | null
});

initials(name: string) {
  return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();
}
services = httpResource<ApiResponse<BookingService[]>>(() => {
  const id = this.selectedProviderId();
  return id ? `${environment.apiUrl}/providers/${id}/services` : undefined;
});

workingHours = httpResource<ApiResponse<WorkingHours[]>>(() => {
  const id = this.selectedProviderId();
  return id ? `${environment.apiUrl}/providers/${id}/working-hours` : undefined;
});

openDays = computed(() =>
  new Set(this.workingHours.value()?.data?.map(w => w.day) ?? [])
);

days = computed(() => {
  const names = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'];
  const today = new Date();
  return Array.from({ length: 14 }, (_, i) => {
    const d = new Date(today.getFullYear(), today.getMonth(), today.getDate() + i);
    const name = names[d.getDay()];
    return {
      value: `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`,
      dayShort: name.slice(0, 3),
      dayNumber: d.getDate(),
      isOpen: this.openDays().has(name)
    };
  });
});

selectedDate = linkedSignal({
  source: this.selectedServiceId,
  computation: () => null as string | null
});

dayClass(d: { value: string; isOpen: boolean }) {
  if (!d.isOpen) return 'border border-dashed border-slate-300 bg-slate-50 text-slate-500 cursor-not-allowed';
  if (this.selectedDate() === d.value) return 'border-2 border-primary bg-primary text-white';
  return 'border border-slate-200 bg-white text-slate-900 hover:border-slate-400 cursor-pointer';
}
availability = httpResource<ApiResponse<string[]>>(() => {
  const providerId = this.selectedProviderId();
  const serviceId = this.selectedServiceId();
  const date = this.selectedDate();
  return providerId && serviceId && date
    ? `${environment.apiUrl}/providers/${providerId}/availability?date=${date}&serviceId=${serviceId}`
    : undefined;
});

selectedSlot = linkedSignal({
  source: this.selectedDate,
  computation: () => null as string | null
});

private businessStore = inject(BusinessStore);

formatTime(iso: string) {
  return new Intl.DateTimeFormat('en-US', {
    hour: 'numeric', minute: '2-digit', timeZone: this.businessStore.timeZone()
  }).format(new Date(iso));
}
}
