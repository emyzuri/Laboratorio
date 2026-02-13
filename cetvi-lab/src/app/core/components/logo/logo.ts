import { Component, Input } from '@angular/core';

@Component({
  selector: 'sgc-logo',
  standalone: false,
  templateUrl: './logo.html',
  styleUrls: ['./logo.scss'],
})
export class LogoComponent {

  @Input() type: 'main' | 'secondary' = 'main';
  @Input() width: string = '160px';   

  get logoPath() {
    return this.type === 'main' ? '/cetvi-remove-bg.png' : '/herramienta.png';
  }
}
