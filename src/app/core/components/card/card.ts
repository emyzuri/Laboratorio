import { Component, Input } from '@angular/core';

@Component({
  selector: 'sgc-card',
  standalone: false,
  templateUrl: './card.html',
})
export class CardComponent {
  @Input() variant: 'shadow' | 'bordered' = 'shadow';
  @Input() align: 'center' | 'left' = 'left';
  @Input() spacing: 'xs' | 'sm' | 'md' | 'lg' | 'xl' = 'lg';
}
