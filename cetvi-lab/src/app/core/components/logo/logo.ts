import { Component } from '@angular/core';
import { Input } from '@angular/core';

@Component({
  selector: 'sgc-logo',
  standalone: false,
  templateUrl: './logo.html',
  styleUrls: ['./logo.scss'],
})
export class LogoComponent {
  @Input() type: 'main' | 'secondary' = 'main';

  get logoPath() {
    return this.type === 'main' ? '/cetvi.jpeg' : '/herramienta.png';
  }
}
