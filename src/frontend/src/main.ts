import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter, Routes } from '@angular/router';
import { AppComponent } from './app/app.component';
import { TicketsPageComponent } from './app/features/tickets/pages/tickets-page.component';

const routes: Routes = [
  { path: '', component: TicketsPageComponent }
];

bootstrapApplication(AppComponent, {
  providers: [provideHttpClient(), provideRouter(routes)]
}).catch((err) => console.error(err));
