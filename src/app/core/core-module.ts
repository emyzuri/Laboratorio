import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { ButtonComponent } from './components/button/button';
import { CardComponent } from './components/card/card';
import { LogoComponent } from './components/logo/logo';
import { IconComponent } from './components/icon/icon';
import { LoadingView } from './components/loading-view/loading-view';
@NgModule({
  declarations: [
    ButtonComponent,
    CardComponent,
    LogoComponent,
    IconComponent,
    LoadingView
  ],
  imports: [
    CommonModule,
    FontAwesomeModule
  ],
  exports: [
    ButtonComponent,
    CardComponent,
    LogoComponent,
    IconComponent,
    FontAwesomeModule,
    LoadingView
  ]
})
export class CoreModule { }
