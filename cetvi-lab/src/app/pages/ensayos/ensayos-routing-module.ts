import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EnsayosComponent } from './ensayos';

const routes: Routes = [
  {
    path: '',
    component: EnsayosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EnsayosRoutingModule { }
