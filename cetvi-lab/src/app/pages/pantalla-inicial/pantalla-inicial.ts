import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-pantalla-inicial',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pantalla-inicial.html',
  styleUrls: ['./pantalla-inicial.scss']
})
export class PantallaInicialComponent implements OnInit {
  private readonly authService = inject(AuthService);

  clientes: any[] = [];
  filtroCedula: string = '';
  selectedClient: any = null;

  paginaActual: number = 1;
  itemsPorPagina: number = 10;

  showFormModal = false;
  showDetailModal = false;
  showSuccessModal = false;
  showDeleteConfirmModal = false;

  isEditing = false;
  successMessage = '';

  clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' };

  nuevoCliente = {
    nombre: '',
    apellido: '',
    telefono: '',
    direccion: '',
    ciudad: '',
    titulo: ''
  };

  ngOnInit() {
    this.cargarClientes();
  }
  get clientesPaginados() {
    const inicio = (this.paginaActual - 1) * this.itemsPorPagina;
    const fin = inicio + this.itemsPorPagina;

    const filtrados = this.clientes.filter(cliente => {
      const cedulaStr = (cliente.cl_cedula || cliente.cedula || '').toString();
      return cedulaStr.includes(this.filtroCedula);
    });

    return filtrados.slice(inicio, fin);
  }

  get totalPaginas(): number {
    const filtrados = this.clientes.filter(cliente => {
      const cedulaStr = (cliente.cl_cedula || cliente.cedula || '').toString();
      return cedulaStr.includes(this.filtroCedula);
    });
    return Math.ceil(filtrados.length / this.itemsPorPagina);
  }

  cambiarPagina(nuevaPagina: number) {
    if (nuevaPagina >= 1 && nuevaPagina <= this.totalPaginas) {
      this.paginaActual = nuevaPagina;
    }
  }
  async cargarClientes() {
    try {
      const respuesta = await this.authService.getClientes();
      if (respuesta?.esExitoso) {
        this.clientes = (respuesta.datos || []).filter((c: any) => {
          const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
          return Number(valorEstado) !== 0;
        });

        this.paginaActual = 1;
      }
    } catch (error) {
      console.error('Error al cargar clientes:', error);
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

  async saveClient() {
    try {
      let respuesta = this.isEditing
        ? await this.authService.actualizarCliente(this.clientForm)
        : await this.authService.insertarCliente(this.clientForm);

      if (respuesta && respuesta.esExitoso) {
        this.successMessage = this.isEditing ? 'Actualizado Exitosamente' : 'Registrado Exitosamente';
        this.showFormModal = false;
        this.showSuccessModal = true;
        this.cargarClientes();
      }
    } catch (error) {
      alert('🚫 Error en la operación');
    }
  }

  async deleteClient() {
    if (!this.selectedClient) return;
    try {
      const id = this.selectedClient.idCliente || this.selectedClient.cl_id;
      const respuesta = await this.authService.eliminarCliente(id);

      if (respuesta?.esExitoso) {
        this.successMessage = 'Eliminado Exitosamente';
        this.showDeleteConfirmModal = false;
        this.showSuccessModal = true;
        this.cargarClientes();
      }
    } catch (error) {
      alert('Error al eliminar');
    }
  }

  handleAction(action: string, client: any) {
    this.selectedClient = { ...client };
    if (action === 'editar') {
      this.isEditing = true;
      this.clientForm = {
        idCliente: client.cl_id || client.idCliente || 0,
        cedula: client.cl_cedula || client.cedula || '',
        nombre: client.cl_nombre || client.nombre || '',
        apellido: client.cl_apellido || client.apellido || '',
        telefono: client.cl_telefono || client.telefono || '',
        direccion: client.cl_direccion || client.direccion || '',
        ciudad: client.cl_ciudad || client.ciudad || '',
        titulo: client.cl_titulo || client.titulo || ''
      };
      this.showFormModal = true;
    } else if (action === 'eliminar') {
      this.showDeleteConfirmModal = true;
    }
  }

  openNewClientModal() {
    this.isEditing = false;
    this.resetForm();
    this.showFormModal = true;
  }

  closeSuccessModal() { this.showSuccessModal = false; }
  closeModals() { this.showFormModal = this.showDetailModal = this.showDeleteConfirmModal = false; }
  resetForm() {
    this.clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' };
    this.filtroCedula = '';
    this.paginaActual = 1;
  }
}
