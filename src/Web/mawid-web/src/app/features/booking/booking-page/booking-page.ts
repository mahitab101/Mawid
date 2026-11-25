import { Component, computed, inject, linkedSignal, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { BusinessStore } from '../../../core/services/business-store';
import { BookingApi } from '../../../core/services/booking-api';
import { Appointment } from '../../../core/models/appointment';
import { formatDate, formatTime } from '../../../core/utils/date-format';
import { BookingConfirmation } from '../booking-confirmation/booking-confirmation';
import { ServiceStep } from '../steps/service-step/service-step';
import { ProviderStep } from '../steps/provider-step/provider-step';
import { DateStep, DayOption } from '../steps/date-step/date-step';
import { TimeStep } from '../steps/time-step/time-step';
import { CustomerDetails, DetailsStep } from '../steps/details-step/details-step';

@Component({
  imports: [BookingConfirmation, ServiceStep, ProviderStep, DateStep, TimeStep, DetailsStep],
  selector: 'app-booking-page',
  styleUrl: './booking-page.css',
  templateUrl: './booking-page.html',
})
export class BookingPage {
  protected businessStore = inject(BusinessStore);
  private api = inject(BookingApi);

  // ١. الخدمة
  services = this.api.getServices();
  selectedServiceId = signal<string | null>(null);
  selectedService = computed(() =>
    this.services.value()?.data?.find(s => s.id === this.selectedServiceId()));

  // ٢. الـ provider
  providers = this.api.getProviders(this.selectedServiceId);
  selectedProviderId = linkedSignal({ source: this.selectedServiceId, computation: () => null as string | null });
  selectedProvider = computed(() =>
    this.providers.value()?.data?.find(p => p.id === this.selectedProviderId()));

  // ٣. اليوم
  workingHours = this.api.getWorkingHours(this.selectedProviderId);
  openDays = computed(() => new Set(this.workingHours.value()?.data?.map(w => w.day) ?? []));

  days = computed<DayOption[]>(() => {
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

  selectedDate = linkedSignal({ source: this.selectedProviderId, computation: () => null as string | null });

  // ٤. الوقت
  availability = this.api.getAvailability(this.selectedProviderId, this.selectedServiceId, this.selectedDate);
  selectedSlot = linkedSignal({ source: this.selectedDate, computation: () => null as string | null });

  // ٥. الحجز
  isSubmitting = signal(false);
  bookingError = signal<string | null>(null);
  bookedAppointment = signal<Appointment | null>(null);

  book(c: CustomerDetails) {
    this.isSubmitting.set(true);
    this.bookingError.set(null);

    this.api.createAppointment({
      providerId: this.selectedProviderId()!,
      serviceId: this.selectedServiceId()!,
      startTime: this.selectedSlot()!,
      customerName: c.name,
      customerPhone: c.phone,
      customerEmail: c.email
    }).subscribe({
      next: res => {
        this.bookedAppointment.set(res.data);
        this.isSubmitting.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.isSubmitting.set(false);
        if (err.status === 409) {
          this.selectedSlot.set(null);
          this.availability.reload();
          this.bookingError.set(err.error?.message ?? 'Sorry, that time was just taken. Please pick another.');
        } else {
          this.bookingError.set('Something went wrong. Please try again.');
        }
      }
    });
  }

  // ٦. الإلغاء
  bookingCancelled = signal(false);
  isCancelling = signal(false);

  cancelBooking() {
    const appt = this.bookedAppointment();
    if (!appt) return;
    this.isCancelling.set(true);
    this.api.cancelAppointment(appt.id).subscribe({
      next: () => {
        this.bookingCancelled.set(true);
        this.isCancelling.set(false);
        this.availability.reload();
      },
      error: () => this.isCancelling.set(false)
    });
  }

  bookAnother() {
    this.bookedAppointment.set(null);
    this.bookingCancelled.set(false);   
    this.selectedServiceId.set(null);
  }

  // helpers
  formatTime(iso: string) { return formatTime(iso, this.businessStore.timeZone()); }
  formatDate(iso: string) { return formatDate(iso, this.businessStore.timeZone()); }
}