import { HttpClient, httpResource } from '@angular/common/http';
import { inject, Service, Signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { BookingService } from '../models/service';
import { ServiceProvider } from '../models/serviceProvider';
import { WorkingHours } from '../models/working-hours';
import { Slot } from '../models/slot';
import { CreateAppointmentRequest } from '../models/create-appointment-request';
import { Appointment } from '../models/appointment';

@Service()
export class BookingApi {
  private http = inject(HttpClient);
  private base = environment.apiUrl;

  getServices() {
    return httpResource<ApiResponse<BookingService[]>>(() => `${this.base}/services`);
  }

  getProviders(serviceId: Signal<string | null>) {
    return httpResource<ApiResponse<ServiceProvider[]>>(() => {
      const id = serviceId();
      return id ? `${this.base}/services/${id}/providers` : undefined;
    });
  }

  getWorkingHours(providerId: Signal<string | null>) {
    return httpResource<ApiResponse<WorkingHours[]>>(() => {
      const id = providerId();
      return id ? `${this.base}/providers/${id}/working-hours` : undefined;
    });
  }

  getAvailability(providerId: Signal<string | null>, serviceId: Signal<string | null>, date: Signal<string | null>) {
    return httpResource<ApiResponse<Slot[]>>(() => {
      const p = providerId(), s = serviceId(), d = date();
      return p && s && d ? `${this.base}/providers/${p}/availability?date=${d}&serviceId=${s}` : undefined;
    });
  }

  createAppointment(request: CreateAppointmentRequest) {
    return this.http.post<ApiResponse<Appointment>>(`${this.base}/appointments`, request);
  }

  cancelAppointment(id: string) {
  return this.http.patch<void>(`${this.base}/appointments/${id}/cancel`, {});
}
}
