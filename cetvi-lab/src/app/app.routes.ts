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
      children: [
        { path: '', redirectTo: 'clientes', pathMatch: 'full' },

        {
          path: 'ensayos',
          loadComponent: () => import('./pages/ensayos/ensayos').then(m => m.EnsayosComponent)
        },
        {
          path: 'buscar-ensayos',
          loadChildren: () => import('./pages/buscar-ensayos/buscar-ensayos-module').then(m => m.BuscarEnsayosModule)
        },
        {
          path: 'clientes',
          loadChildren: () => import('./pages/pantalla-inicial/pantalla-inicial-module').then(m => m.PantallaInicialRoutingModule)
        },
        {
          path: 'usuarios',
          loadComponent: () => import('./pages/administrador/administrador').then(m => m.AdministradorComponent)
        },
      ]
    },
    { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
    { path: '**', redirectTo: 'auth/login' }
  ];
