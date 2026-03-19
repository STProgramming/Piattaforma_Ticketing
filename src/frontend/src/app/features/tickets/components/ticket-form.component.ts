import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateTicketRequest } from '../models/ticket.model';

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="submit()" class="ticket-form">
      <label>
        Titolo ticket
        <input formControlName="title" />
      </label>

      <label>
        Descrizione ticket
        <textarea formControlName="description"></textarea>
      </label>

      <label>
        Creato da
        <input formControlName="createdBy" />
      </label>

      <label class="toggle">
        <input type="checkbox" formControlName="createUserStory" />
        Crea anche una User Story in Azure DevOps
      </label>

      <section class="azure-devops-box" *ngIf="form.controls.createUserStory.value">
        <label>
          Titolo User Story
          <input formControlName="azureTitle" />
        </label>

        <label>
          Descrizione User Story
          <textarea formControlName="azureDescription"></textarea>
        </label>

        <label>
          Area Path
          <input formControlName="areaPath" />
        </label>

        <label>
          Iteration Path
          <input formControlName="iterationPath" />
        </label>

        <label>
          Assegnatario Azure DevOps
          <input formControlName="assignedTo" />
        </label>
      </section>

      <button type="submit" [disabled]="form.invalid">Apri ticket</button>
    </form>
  `,
  styles: [`
    .ticket-form { display: grid; gap: 1rem; background: #f7fafc; padding: 1.5rem; border-radius: 12px; }
    .toggle { grid-template-columns: auto 1fr; align-items: center; gap: .75rem; }
    .azure-devops-box { display: grid; gap: 1rem; padding: 1rem; border: 1px solid #bfdbfe; border-radius: 12px; background: #eff6ff; }
    label { display: grid; gap: .5rem; font-weight: 600; }
    input, textarea { padding: .75rem; border: 1px solid #cbd5e0; border-radius: 8px; }
    button { width: fit-content; padding: .75rem 1rem; border: 0; border-radius: 8px; background: #2563eb; color: #fff; }
  `]
})
export class TicketFormComponent {
  @Output() save = new EventEmitter<CreateTicketRequest>();

  private readonly fb = new FormBuilder();

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(120)]],
    description: ['', [Validators.required, Validators.maxLength(2000)]],
    createdBy: ['', [Validators.required]],
    createUserStory: false,
    azureTitle: '',
    azureDescription: '',
    areaPath: '',
    iterationPath: '',
    assignedTo: ''
  });

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    const value = this.form.getRawValue();
    this.save.emit({
      title: value.title,
      description: value.description,
      createdBy: value.createdBy,
      azureDevOps: value.createUserStory
        ? {
            createUserStory: true,
            title: value.azureTitle || null,
            description: value.azureDescription || null,
            areaPath: value.areaPath || null,
            iterationPath: value.iterationPath || null,
            assignedTo: value.assignedTo || null
          }
        : null
    });

    this.form.reset({
      title: '',
      description: '',
      createdBy: '',
      createUserStory: false,
      azureTitle: '',
      azureDescription: '',
      areaPath: '',
      iterationPath: '',
      assignedTo: ''
    });
  }
}
