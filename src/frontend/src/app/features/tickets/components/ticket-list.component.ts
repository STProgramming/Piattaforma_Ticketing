import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Ticket } from '../models/ticket.model';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="list-wrapper">
      <article class="ticket-card" *ngFor="let ticket of tickets">
        <div class="ticket-head">
          <h3>{{ ticket.title }}</h3>
          <span>{{ ticket.status }}</span>
        </div>
        <p>{{ ticket.description }}</p>
        <small>{{ ticket.createdBy }} · {{ ticket.createdAt | date:'short' }}</small>

        <a
          *ngIf="ticket.azureDevOpsWorkItemId && ticket.azureDevOpsWorkItemUrl"
          class="devops-link"
          [href]="ticket.azureDevOpsWorkItemUrl"
          target="_blank"
          rel="noreferrer">
          User Story Azure DevOps #{{ ticket.azureDevOpsWorkItemId }}
        </a>
      </article>
    </section>
  `,
  styles: [`
    .list-wrapper { display: grid; gap: 1rem; }
    .ticket-card { border: 1px solid #d9e2ec; border-radius: 12px; padding: 1rem; background: white; }
    .ticket-head { display:flex; justify-content:space-between; gap:1rem; align-items:center; }
    span { background:#dbeafe; color:#1d4ed8; padding:.25rem .75rem; border-radius:999px; font-size:.85rem; }
    .devops-link { display: inline-block; margin-top: .75rem; color: #2563eb; font-weight: 600; }
  `]
})
export class TicketListComponent {
  @Input({ required: true }) tickets: Ticket[] = [];
}
