import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CreateTicketRequest, Ticket } from '../models/ticket.model';

@Injectable({ providedIn: 'root' })
export class TicketApiService {
  private readonly http = inject(HttpClient);
  private readonly resourceUrl = `${environment.apiBaseUrl}/tickets`;

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.resourceUrl);
  }

  createTicket(payload: CreateTicketRequest): Observable<Ticket> {
    return this.http.post<Ticket>(this.resourceUrl, payload);
  }
}
