import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { TicketApiService } from '../services/ticket-api.service';
import { CreateTicketRequest, Ticket } from '../models/ticket.model';
import { TicketFormComponent } from '../components/ticket-form.component';
import { TicketListComponent } from '../components/ticket-list.component';

@Component({
  selector: 'app-tickets-page',
  standalone: true,
  imports: [CommonModule, TicketFormComponent, TicketListComponent],
  template: `
    <section class="content-grid">
      <app-ticket-form (save)="createTicket($event)" />

      <section>
        <h2>Ticket aperti</h2>
        <app-ticket-list [tickets]="tickets()" />
      </section>
    </section>
  `,
  styles: [`
    .content-grid { display:grid; grid-template-columns: 360px 1fr; gap: 1.5rem; align-items:start; }
    @media (max-width: 900px) { .content-grid { grid-template-columns: 1fr; } }
  `]
})
export class TicketsPageComponent implements OnInit {
  private readonly ticketApi = inject(TicketApiService);

  readonly tickets = signal<Ticket[]>([]);

  ngOnInit(): void {
    this.loadTickets();
  }

  createTicket(request: CreateTicketRequest): void {
    this.ticketApi.createTicket(request).subscribe((ticket) => {
      this.tickets.update((items) => [ticket, ...items]);
    });
  }

  private loadTickets(): void {
    this.ticketApi.getTickets().subscribe((tickets) => this.tickets.set(tickets));
  }
}
