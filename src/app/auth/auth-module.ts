import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing-module';
import { CoreModule } from '../core/core-module';
import { LoginComponent } from './pages/login/login';
@NgModule({
  declarations: [
    LoginComponent
],
  imports: [
    CommonModule,
    AuthRoutingModule,
    CoreModule,
  ]
})
export class AuthModule { }
