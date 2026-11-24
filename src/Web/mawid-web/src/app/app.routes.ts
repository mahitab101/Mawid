import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/booking/booking-page/booking-page').then(m => m.BookingPage) }
];