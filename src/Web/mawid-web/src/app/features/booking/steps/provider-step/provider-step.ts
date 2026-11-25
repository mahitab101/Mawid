import { Component, input, model } from '@angular/core';
import { ServiceProvider } from '../../../../core/models/serviceProvider';

@Component({
  imports: [],
  selector: 'app-provider-step',
  styleUrl: './provider-step.css',
  templateUrl: './provider-step.html',
})
export class ProviderStep {
  providers = input<ServiceProvider[]>([]);
  isLoading = input(false);
  selected = model<string | null>(null);

  initials(name: string) {
    return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();
  }
}
