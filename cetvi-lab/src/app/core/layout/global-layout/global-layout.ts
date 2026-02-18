// import { Component, OnInit, inject } from '@angular/core';
// import { Router } from '@angular/router';
// import { AuthService } from '../../../auth/services/auth.service';

// @Component({
//   selector: 'app-global-layout',
//   standalone: false,
//   templateUrl: './global-layout.html',
//   styleUrls: ['./global-layout.scss']
// })
// export class GlobalLayoutComponent implements OnInit {
//   private router = inject(Router);
//   private authService = inject(AuthService);

//   listaMenu: any[] = [];
//   menusAbiertos: { [key: number]: boolean } = {};
  
//   isMenuExpanded: boolean = true; 

//   ngOnInit() {
//     this.cargarMenu();
//   }

//   async cargarMenu() {
//     try {
//       const respuesta: any = await this.authService.getMenu();
//       if (respuesta && respuesta.esExitoso === true) {
//         this.listaMenu = respuesta.datos || [];
//       }
//     } catch (error) {
//       console.error('Error de comunicación:', error);
//     }
//   }

//   toggleMenu(idMenu: number) {
//     this.menusAbiertos[idMenu] = !this.menusAbiertos[idMenu];
//   }

//   // Nueva función para colapsar/expandir el sidebar
//   toggleSidebar() {
//     this.isMenuExpanded = !this.isMenuExpanded;
//   }

//   cerrarSesion() {
//     localStorage.clear();
//     this.router.navigateByUrl('/auth/login');
//   }
// }
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
  private eRef = inject(ElementRef); // Inyectamos ElementRef para detectar clics fuera

  listaMenu: any[] = [];
  menusAbiertos: { [key: number]: boolean } = {};
  
  isMenuExpanded: boolean = true; 
  // Nueva propiedad para el estado del menú de perfil
  isProfileMenuOpen: boolean = false;

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

  toggleSidebar() {
    this.isMenuExpanded = !this.isMenuExpanded;
  }

  // --- Lógica para el User Profile Dropdown ---

  toggleProfileMenu() {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
  }

  // Cierra el menú de perfil si se hace clic fuera del componente
  @HostListener('document:click', ['$event'])
  clickOut(event: Event) {
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
    // Lógica para cambiar de cuenta si la tienes implementada
    this.cerrarSesion();
  }

  cerrarSesion() {
    this.isProfileMenuOpen = false;
    localStorage.clear();
    this.router.navigateByUrl('/auth/login');
  }
}