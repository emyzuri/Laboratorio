import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuscarEnsayosComponent } from './buscar-ensayos'; // Importa la clase del componente
import { BuscarEnsayosRoutingModule } from './buscar-ensayos-routing-module';

@NgModule({
  declarations: [
    // VACÍO: Nunca declares un componente standalone aquí
  ],
  imports: [
    CommonModule,
    FormsModule,
    BuscarEnsayosRoutingModule,
    BuscarEnsayosComponent // SE AGREGA AQUÍ
  ]
})
export class BuscarEnsayosModule { }
