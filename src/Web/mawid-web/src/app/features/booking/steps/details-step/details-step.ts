import { Component, input, output, signal } from '@angular/core';
import { email, form, FormField, pattern, required } from '@angular/forms/signals';

export interface CustomerDetails { name: string; phone: string; email: string; }
@Component({
  imports: [FormField],
  selector: 'app-details-step',
  styleUrl: './details-step.css',
  templateUrl: './details-step.html',
})

export class DetailsStep {
  isSubmitting = input(false);
  submitted = output<CustomerDetails>();

  customer = signal<CustomerDetails>({ name: '', phone: '', email: '' });
  customerForm = form(this.customer, (c) => {
    required(c.name, { message: 'Name is required' });
    required(c.phone, { message: 'Mobile is required' });
    pattern(c.phone, /^\+?[0-9 ]{9,15}$/, { message: 'Enter a valid mobile number' });
    required(c.email, { message: 'Email is required' });
    email(c.email, { message: 'Enter a valid email' });
  });

  submit(event: Event) {
    event.preventDefault();
    if (this.customerForm().invalid()) {
      this.customerForm().markAsTouched();
      return;
    }
    this.submitted.emit(this.customer());
  }
}
