import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuscarEnsayosComponent } from './buscar-ensayos';
import { BuscarEnsayosRoutingModule } from './buscar-ensayos-routing-module';

@NgModule({
  declarations: [
    BuscarEnsayosComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BuscarEnsayosRoutingModule
  ]
})
export class BuscarEnsayosModule { }
