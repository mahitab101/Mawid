import { Component } from '@angular/core';

import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment.development';
import { Business } from './core/models/business';
import { httpResource } from '@angular/common/http';
import { ApiResponse } from './core/models/api-response';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [RouterOutlet]
})
export class App {
  business = httpResource<ApiResponse<Business>>(() => `${environment.apiUrl}/business`);
}