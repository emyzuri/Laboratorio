import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-global-layout',
  standalone: false,
  templateUrl: './global-layout.html',
  styleUrls: ['./global-layout.scss']
})
export class GlobalLayoutComponent implements OnInit {
  private router = inject(Router);
  private authService = inject(AuthService);

  listaMenu: any[] = [];
  menusAbiertos: { [key: number]: boolean } = {};

  ngOnInit() {
    this.cargarMenu();
  }

  async cargarMenu() {
    try {
      const respuesta: any = await this.authService.getMenu();

      if (respuesta && respuesta.esExitoso === true) {
        this.listaMenu = respuesta.datos || [];
      } else {
        console.error('Error en carga de menú:', respuesta?.mensaje);
      }
    } catch (error) {
      console.error('Error de comunicación con el servidor:', error);
    }
  }

  toggleMenu(idMenu: number) {
    this.menusAbiertos[idMenu] = !this.menusAbiertos[idMenu];
  }

  cerrarSesion() {
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }
}
