import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CoreModule } from './core/core-module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CoreModule,
  ],
  templateUrl: './app.html',
})
export class AppComponent {
  title = 'cetvi-lab';
}
