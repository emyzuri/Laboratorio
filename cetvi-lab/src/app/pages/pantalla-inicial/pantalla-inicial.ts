import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-pantalla-inicial',
  standalone: false,
  templateUrl: './pantalla-inicial.html'
})
export class PantallaInicial implements OnInit {
  private authService = inject(AuthService);

  clientes: any[] = [];
  mostrarHijos: boolean = false;

  nuevoCliente = {
    nombre: '',
    apellido: '',
    telefono: '',
    direccion: '',
    ciudad: '',
    titulo: ''
  };

  ngOnInit() {
    this.cargarDatos();
  }

  async cargarDatos() {
    try {
      const res: any = await this.authService.getClientes();
      console.log('Clientes cargados:', res);

      this.clientes = res.dato || res;
    } catch (error) {
      console.error('Error al cargar clientes:', error);
      this.clientes = [{ nombre: 'Constructora Alfa', apellido: 'Ecuador', titulo: 'Ingeniería' }];
    }
  }

  guardarCliente() {
    if (!this.nuevoCliente.nombre.trim()) {
      alert('Por favor, ingresa el nombre del cliente');
      return;
    }

    this.authService.insertarCliente(this.nuevoCliente).subscribe({
      next: (res: any) => {
        console.log('Respuesta del servidor:', res);

        if (res.esExitoso || res.status === 'success') {
          alert('Cliente guardado con éxito');

          this.cargarDatos();
          this.limpiarFormulario();
        } else {
          alert('El servidor respondió pero no se pudo guardar.');
        }
      },
      error: (err) => {
        console.error('Error al insertar:', err);
        alert('Error de conexión: No se pudo guardar el cliente');
      }
    });
  }

  limpiarFormulario() {
    this.nuevoCliente = {
      nombre: '',
      apellido: '',
      telefono: '',
      direccion: '',
      ciudad: '',
      titulo: ''
    };
  }
}
