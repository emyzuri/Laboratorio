import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { ButtonComponent } from './components/button/button';
import { CardComponent } from './components/card/card';
import { LogoComponent } from './components/logo/logo';
import { IconComponent } from './components/icon/icon';
import { LoadingView } from './components/loading-view/loading-view';
import { GlobalLayoutComponent } from './layout/global-layout/global-layout';
import { RouterModule } from '@angular/router';
@NgModule({
  declarations: [
    ButtonComponent,
    CardComponent,
    LogoComponent,
    IconComponent,
    LoadingView,
    GlobalLayoutComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    RouterModule
  ],
  exports: [
    ButtonComponent,
    CardComponent,
    LogoComponent,
    IconComponent,
    FontAwesomeModule,
    LoadingView,
    GlobalLayoutComponent
  ]
})
export class CoreModule { }
