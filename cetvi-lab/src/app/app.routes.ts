import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
// {
//   path: 'principal',
//   loadChildren: () =>
//     // Fíjate si tu archivo es .module o -module
//     // import('./pages/pantalla-inicial/pantalla-inicial-module').then((m) => m.PantallaInicialModule),
// },
  {
    path: '**',
    redirectTo: 'auth/login'
  }
];
