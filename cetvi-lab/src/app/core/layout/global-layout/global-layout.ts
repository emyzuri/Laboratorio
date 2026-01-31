import { Component, OnInit, inject } from '@angular/core'; // Agregamos OnInit
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-global-layout',
  standalone: false,
  templateUrl: './global-layout.html',
  styleUrls: ['./global-layout.scss']
})
export class GlobalLayoutComponent implements OnInit { // Implementamos OnInit

  private router = inject(Router);
  private authService = inject(AuthService);

  listaMenu: any[] = []; // Aquí se guardará lo que venga del Back
  menusAbiertos: { [key: number]: boolean } = {}; // Usaremos el ID del menú para controlar el despliegue

  ngOnInit() {
    this.cargarMenu();
  }

  async cargarMenu() {
    try {
      // Consumimos el endpoint de Postman
      this.listaMenu = await this.authService.getMenu();
      console.log('Menú cargado dinámicamente:', this.listaMenu);
    } catch (error) {
      console.error('Error al cargar el menú:', error);
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
