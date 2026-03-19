import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <main class="shell">
      <header class="hero">
        <h1>Ticketing Platform</h1>
        <p>Frontend Angular per la gestione ticket integrata con backend .NET, Azure e Azure DevOps.</p>
      </header>
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    :host { font-family: Arial, sans-serif; color: #102a43; }
    .shell { max-width: 1080px; margin: 0 auto; padding: 2rem; }
    .hero { margin-bottom: 2rem; }
  `]
})
export class AppComponent {}
