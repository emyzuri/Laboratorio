import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { PantallaInicialComponent } from './pantalla-inicial';
const routes: Routes = [
  { path: '', component: PantallaInicialComponent }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class PantallaInicialRoutingModule { }
