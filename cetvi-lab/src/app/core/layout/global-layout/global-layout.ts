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
  
  isMenuExpanded: boolean = true; 

  ngOnInit() {
    this.cargarMenu();
  }

  async cargarMenu() {
    try {
      const respuesta: any = await this.authService.getMenu();
      if (respuesta && respuesta.esExitoso === true) {
        this.listaMenu = respuesta.datos || [];
      }
    } catch (error) {
      console.error('Error de comunicación:', error);
    }
  }

  toggleMenu(idMenu: number) {
    this.menusAbiertos[idMenu] = !this.menusAbiertos[idMenu];
  }

  // Nueva función para colapsar/expandir el sidebar
  toggleSidebar() {
    this.isMenuExpanded = !this.isMenuExpanded;
  }

  cerrarSesion() {
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }
}
