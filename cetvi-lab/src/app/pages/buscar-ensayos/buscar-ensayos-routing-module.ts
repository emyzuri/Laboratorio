import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BuscarEnsayosComponent } from './buscar-ensayos';

const routes: Routes = [
  {
    path: '',
    component: BuscarEnsayosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BuscarEnsayosRoutingModule { }
