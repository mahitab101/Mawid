import { httpResource } from '@angular/common/http';
import { computed, Injectable, Service } from '@angular/core';
import { ApiResponse } from '../models/api-response';
import { Business } from '../models/business';
import { environment } from '../../../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class BusinessStore {
  readonly business = httpResource<ApiResponse<Business>>(() => `${environment.apiUrl}/business`);
  readonly timeZone = computed(() => this.business.value()?.data?.timeZone ?? 'UTC');
}
