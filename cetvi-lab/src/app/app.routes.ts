import { Routes } from '@angular/router';
import { GlobalLayoutComponent } from './core/layout/global-layout/global-layout';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: 'principal',
    component: GlobalLayoutComponent,
    children: [ //pagina principal marco
      { path: '', redirectTo: 'clientes', pathMatch: 'full' },
      {
        path: 'clientes',
        loadChildren: () => import('./pages/pantalla-inicial/pantalla-inicial-module').then(m => m.PantallaInicialRoutingModule)
      },
      {
        path: 'administrador',
        loadChildren: () => import('./pages/administrador/administrador-module').then(m => m.AdministradorModule)
      },
    ]
  },
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: '**', redirectTo: 'auth/login' }
];
