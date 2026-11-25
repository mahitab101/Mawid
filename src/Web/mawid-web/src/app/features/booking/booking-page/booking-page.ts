import { Component, computed, inject, linkedSignal, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { form, FormField, required, email, pattern } from '@angular/forms/signals';
import { BusinessStore } from '../../../core/services/business-store';
import { BookingApi } from '../../../core/services/booking-api';
import { Appointment } from '../../../core/models/appointment';
import { BookingConfirmation } from '../booking-confirmation/booking-confirmation';

@Component({
  imports: [FormField, BookingConfirmation],
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

  selectedDate = linkedSignal({ source: this.selectedProviderId, computation: () => null as string | null });

  // ٤. الوقت
  availability = this.api.getAvailability(this.selectedProviderId, this.selectedServiceId, this.selectedDate);
  selectedSlot = linkedSignal({ source: this.selectedDate, computation: () => null as string | null });

  // ٥. البيانات
  customer = signal({ name: '', phone: '', email: '' });
  customerForm = form(this.customer, (c) => {
    required(c.name, { message: 'Name is required' });
    required(c.phone, { message: 'Mobile is required' });
    pattern(c.phone, /^\+?[0-9 ]{9,15}$/, { message: 'Enter a valid mobile number' });
    required(c.email, { message: 'Email is required' });
    email(c.email, { message: 'Enter a valid email' });
  });

  // الحجز
  isSubmitting = signal(false);
  bookingError = signal<string | null>(null);
  bookedAppointment = signal<Appointment | null>(null);

  selectSlot(slot: string) {
    this.selectedSlot.set(slot);
    this.bookingError.set(null);
  }

  book(event: Event) {
    event.preventDefault();

    if (this.customerForm().invalid()) {
      this.customerForm().markAsTouched();
      return;
    }

    const c = this.customer();
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

  bookAnother() {
    this.bookedAppointment.set(null);
    this.selectedServiceId.set(null);
    this.customer.set({ name: '', phone: '', email: '' });
  }

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