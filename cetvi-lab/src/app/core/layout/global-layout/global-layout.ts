import { Component, OnInit, inject, HostListener, ElementRef } from '@angular/core';
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
  private eRef = inject(ElementRef);

  listaMenu: any[] = [];
  menusAbiertos: { [key: number]: boolean } = {};

  isMenuExpanded: boolean = true;
  isProfileMenuOpen: boolean = false;

  // Propiedades para mostrar en el HTML
  nombreUsuario: string = '';
  usuario : string = '';
  telefono : string = '';
  cedula : string = '';

  ngOnInit() {
    this.cargarMenu();
    this.cargarDatosUsuario(); // Extraemos los datos del usuario logueado
  }

  cargarDatosUsuario() {
    const usuarioData = localStorage.getItem('usuarioLogueado');

    if (usuarioData) {
      const usuario = JSON.parse(usuarioData);

      // Mapeamos nombre y apellido según la estructura del back
      this.nombreUsuario = `${usuario.nombre} ${usuario.apellido}`;

      // Usamos el campo 'usuario' que contiene el login (ej: bory)
      this.usuario = usuario.usuario;
      this.telefono = usuario.telefono;
      this.cedula = usuario.cedula;
    } else {
      // Valores por defecto si no hay sesión
      this.nombreUsuario = 'Invitado';
      this.usuario = 'Sin sesión activa';
      this.telefono = '';
      this.cedula = '';
    }
  }

  async cargarMenu() {
  try {

    const respuesta: any = await this.authService.getMenu();

    if (respuesta?.esExitoso) {
      this.listaMenu = respuesta.datos || [];
    } else {
      this.listaMenu = [];
    }

  } catch (error) {
    console.error('Error de comunicación:', error);
    this.listaMenu = [];
  }
}


  toggleMenu(idMenu: number) {
    this.menusAbiertos[idMenu] = !this.menusAbiertos[idMenu];
  }

  toggleSidebar() {
    this.isMenuExpanded = !this.isMenuExpanded;
  }

  toggleProfileMenu() {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
  }

  @HostListener('document:click', ['$event'])
  clickOut(event: Event) {
    // Si el clic es fuera del contenedor del perfil, cerramos el dropdown
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isProfileMenuOpen = false;
    }
  }

  irAConfiguracion() {
    this.isProfileMenuOpen = false;
    this.router.navigateByUrl('/configuracion');
  }

  cambiarUsuario() {
    this.isProfileMenuOpen = false;
    this.cerrarSesion();
  }

  cerrarSesion() {
    this.isProfileMenuOpen = false;
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }
}
