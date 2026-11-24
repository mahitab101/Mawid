import { Component, linkedSignal, signal } from '@angular/core';
import { ApiResponse } from '../../../core/models/api-response';
import { environment } from '../../../../environments/environment.development';
import { ServiceProvider } from '../../../core/models/serviceProvider';
import { httpResource } from '@angular/common/http';
import { BookingService } from '../../../core/models/service';
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
}
