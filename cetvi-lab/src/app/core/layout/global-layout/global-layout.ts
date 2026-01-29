import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-global-layout',
  standalone: false,
  templateUrl: './global-layout.html'
})
export class GlobalLayoutComponent {
  private router = inject(Router);
  mostrarHijos: boolean = false;

  cerrarSesion() {
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }
}
