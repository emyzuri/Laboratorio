import { Component, Input } from '@angular/core';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { iconMap, IconName, iconPack } from '../../types/icons.types';

@Component({
  selector: 'sgc-icon',
  standalone: false,
  templateUrl: './icon.html',
})
export class IconComponent {
  @Input() icon: IconName | undefined;

  constructor(private library: FaIconLibrary) {
    this.library.addIconPacks(iconPack);
  }

  get iconToShow(): IconProp {
    if (!this.icon) {
      throw new Error('Icon is required');
    }
    return iconMap[this.icon];
  }
}
