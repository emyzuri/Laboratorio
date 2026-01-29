import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { PantallaInicial } from './pantalla-inicial';

const routes: Routes = [
  { path: '', component: PantallaInicial }
];

@NgModule({
  declarations: [PantallaInicial],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class PantallaInicialRoutingModule { }
