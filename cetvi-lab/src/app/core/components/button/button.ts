import { Component, Input } from '@angular/core';

@Component({
  selector: 'sgc-button',
  standalone: false, // Recuerda declararlo en un SharedModule
  templateUrl: './button.html',
  styleUrls: ['./button.scss'],
})
export class ButtonComponent {
  @Input() variant: 'default' | 'primary' | 'secondary' = 'default';
  @Input() fullWidth: boolean = false;
  @Input() withoutMargin: boolean = false;
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false; // FALTABA ESTO
}
