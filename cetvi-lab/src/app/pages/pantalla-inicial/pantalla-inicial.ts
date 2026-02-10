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
  filtroBusqueda: string = '';
  selectedClient: any = null;

  paginaActual: number = 1;
  itemsPorPagina: number = 10;

  showFormModal = false;
  showSuccessModal = false;
  showDeleteConfirmModal = false;

  isEditing = false;
  successMessage = '';

  clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' };

  ngOnInit() {
    this.cargarClientes();
  }

  get clientesPaginados() {
    const filtrados = this.clientes.filter(c => {
      const cedula = (c.cl_cedula || c.cedula || '').toLowerCase();
      const nombre = (c.cl_nombre || c.nombre || '').toLowerCase();
      const apellido = (c.cl_apellido || c.apellido || '').toLowerCase();
      const busqueda = this.filtroBusqueda.toLowerCase();
      return cedula.includes(busqueda) || nombre.includes(busqueda) || apellido.includes(busqueda);
    }).sort((a, b) => {
      const nombreA = (a.cl_nombre || a.nombre || '').toLowerCase();
      const nombreB = (b.cl_nombre || b.nombre || '').toLowerCase();
      return nombreA.localeCompare(nombreB);
    });

    const inicio = (this.paginaActual - 1) * this.itemsPorPagina;
    return filtrados.slice(inicio, inicio + this.itemsPorPagina);
  }

  get totalPaginas(): number {
    const filtrados = this.clientes.filter(c => {
      const busqueda = this.filtroBusqueda.toLowerCase();
      return (c.cl_cedula || c.cedula || '').includes(busqueda) ||
             (c.cl_nombre || c.nombre || '').toLowerCase().includes(busqueda);
    });
    return Math.ceil(filtrados.length / this.itemsPorPagina);
  }
  validarSoloNumeros(event: any) {
    const pattern = /[0-9]/;
    if (!pattern.test(String.fromCharCode(event.charCode))) {
      event.preventDefault();
    }
  }

  validarFormulario(): boolean {
    const f = this.clientForm;
    if (!f.cedula || !f.nombre || !f.apellido || !f.telefono) {
      alert('⚠️ Complete los campos obligatorios (Cédula, Nombre, Apellido, Teléfono)');
      return false;
    }
    if (!this.isEditing) {
      const existe = this.clientes.some(c => (c.cl_cedula || c.cedula) === f.cedula);
      if (existe) {
        alert('🚫 Error: Esta cédula ya está registrada.');
        return false;
      }
    }
    return true;
  }

  async cargarClientes() {
    try {
      const respuesta = await this.authService.getClientes();
      if (respuesta?.esExitoso) {
        this.clientes = (respuesta.datos || []).filter((c: any) => {
          const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
          return Number(valorEstado) !== 0;
        });
      }
    } catch (error) { console.error('Error al cargar clientes:', error); }
  }

  async saveClient() {
    if (!this.validarFormulario()) return;
    try {
      const respuesta = this.isEditing
        ? await this.authService.actualizarCliente(this.clientForm)
        : await this.authService.insertarCliente(this.clientForm);

      if (respuesta?.esExitoso) {
        this.successMessage = this.isEditing ? 'Actualizado Exitosamente' : 'Registrado Exitosamente';
        this.showFormModal = false;
        this.showSuccessModal = true;
        this.cargarClientes();
      }
    } catch (error) { alert('🚫 Error en el servidor'); }
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

  async deleteClient() {
    if (!this.selectedClient) return;
    try {
      const id = this.selectedClient.cl_id || this.selectedClient.idCliente;
      const respuesta = await this.authService.eliminarCliente(id);
      if (respuesta?.esExitoso) {
        this.successMessage = 'Eliminado Exitosamente';
        this.showDeleteConfirmModal = false;
        this.showSuccessModal = true;
        this.cargarClientes();
      }
    } catch (error) { alert('Error al eliminar'); }
  }

  cambiarPagina(p: number) { this.paginaActual = p; }
  closeSuccessModal() { this.showSuccessModal = false; }
  closeModals() { this.showFormModal = this.showDeleteConfirmModal = false; }
  openNewClientModal() { this.isEditing = false; this.resetForm(); this.showFormModal = true; }
  resetForm() {
    this.clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' };
    this.filtroBusqueda = '';
  }
}
