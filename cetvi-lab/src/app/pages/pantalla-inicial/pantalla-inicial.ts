import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service'; // Ruta corregida

@Component({
  selector: 'app-pantalla-inicial',
  standalone: false,
  templateUrl: './pantalla-inicial.html'
})
export class PantallaInicial implements OnInit {
  private authService = inject(AuthService);

  clientes: any[] = [];
  mostrarHijos: boolean = false;

  ngOnInit() {
    this.cargarDatos();
  }

  async cargarDatos() {
    try {
      this.clientes = await this.authService.getClientes();
    } catch (error) {
      console.error('Error al cargar clientes:', error);
      this.clientes = [{ nombre: 'Constructora Alfa', ruc: '179000111', estado: 'Activo' }];
    }
  }
}
