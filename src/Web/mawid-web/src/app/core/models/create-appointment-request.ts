export interface CreateAppointmentRequest {
  providerId: string;
  serviceId: string;
  startTime: string;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
}