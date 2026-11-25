import { Component, computed, inject, linkedSignal, signal } from '@angular/core';
import { httpResource } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response';
import { ServiceProvider } from '../../../core/models/serviceProvider';
import { BookingService } from '../../../core/models/service';
import { WorkingHours } from '../../../core/models/working-hours';
import { BusinessStore } from '../../../core/services/business-store';
import { Slot } from '../../../core/models/slot';

@Component({
  imports: [],
  selector: 'app-booking-page',
  styleUrl: './booking-page.css',
  templateUrl: './booking-page.html',
})
export class BookingPage {
  protected businessStore = inject(BusinessStore);

  // ١. الخدمة
  services = httpResource<ApiResponse<BookingService[]>>(() => `${environment.apiUrl}/services`);
  selectedServiceId = signal<string | null>(null);

  // ٢. الـ provider
  providers = httpResource<ApiResponse<ServiceProvider[]>>(() => {
    const id = this.selectedServiceId();
    return id ? `${environment.apiUrl}/services/${id}/providers` : undefined;
  });
  selectedProviderId = linkedSignal({
    source: this.selectedServiceId,
    computation: () => null as string | null
  });

  // ٣. اليوم
  workingHours = httpResource<ApiResponse<WorkingHours[]>>(() => {
    const id = this.selectedProviderId();
    return id ? `${environment.apiUrl}/providers/${id}/working-hours` : undefined;
  });

  openDays = computed(() =>
    new Set(this.workingHours.value()?.data?.map(w => w.day) ?? [])
  );

  days = computed(() => {
    const names = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
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
    source: this.selectedProviderId,
    computation: () => null as string | null
  });

  // ٤. الوقت
  availability = httpResource<ApiResponse<Slot[]>>(() => {
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

  // helpers
  initials(name: string) {
    return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();
  }

  dayClass(d: { value: string; isOpen: boolean }) {
    if (!d.isOpen) return 'border border-dashed border-slate-300 bg-slate-50 text-slate-500 cursor-not-allowed';
    if (this.selectedDate() === d.value) return 'border-2 border-primary bg-primary text-white';
    return 'border border-slate-200 bg-white text-slate-900 hover:border-slate-400 cursor-pointer';
  }

  formatTime(iso: string) {
    return new Intl.DateTimeFormat('en-US', {
      hour: 'numeric', minute: '2-digit', timeZone: this.businessStore.timeZone()
    }).format(new Date(iso));
  }
}