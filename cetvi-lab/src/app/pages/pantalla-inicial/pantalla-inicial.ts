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
  showDeleteConfirmModal = false;
  isEditing = false;

  // Estructura limpia del formulario
  clientForm = {
    idCliente: 0,
    cedula: '',
    nombre: '',
    apellido: '',
    telefono: '',
    direccion: '',
    ciudad: '',
    titulo: '',
    correo: ''
  };

  ngOnInit() {
    this.cargarClientes();
  }

  async cargarClientes() {
    try {
      const resp = await this.authService.getClientes();
      if (resp?.esExitoso) this.clientes = resp.datos || [];
    } catch (e) { console.error(e); }
  }

  get clientesPaginados() {
    const bus = this.filtroBusqueda.toLowerCase().trim();
    const filtrados = this.clientes.filter(c => {
      const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
      const estaActivo = valorEstado != 0;
      const nombreFull = `${c.cl_nombre || c.nombre || ''} ${c.cl_apellido || c.apellido || ''}`.toLowerCase();
      const coincide = nombreFull.includes(bus) || (c.cl_cedula || c.cedula || '').includes(bus);
      return estaActivo && coincide;
    });

    const inicio = (this.paginaActual - 1) * this.itemsPorPagina;
    return filtrados.slice(inicio, inicio + this.itemsPorPagina);
  }

  get totalPaginas(): number {
    const bus = this.filtroBusqueda.toLowerCase().trim();
    const totalActivos = this.clientes.filter(c => {
      const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
      return valorEstado != 0 &&
            (`${c.cl_nombre || c.nombre || ''} ${c.cl_apellido || c.apellido || ''}`.toLowerCase().includes(bus) ||
             (c.cl_cedula || c.cedula || '').includes(bus));
    }).length;
    return Math.ceil(totalActivos / this.itemsPorPagina) || 1;
  }

  openNewClientModal() {
    this.isEditing = false;
    this.resetForm(); // Garantiza que los campos estén vacíos
    this.showFormModal = true;
  }

  resetForm() {
    this.clientForm = {
      idCliente: 0, cedula: '', nombre: '', apellido: '',
      telefono: '', direccion: '', ciudad: '', titulo: '', correo: ''
    };
  }

  handleAction(action: string, client: any) {
    this.selectedClient = { ...client };
    if (action === 'editar') {
      this.isEditing = true;
      this.clientForm = {
        idCliente: client.cl_id ?? client.idCliente ?? 0,
        cedula:    client.cl_cedula ?? client.cedula ?? '',
        nombre:    (client.cl_nombre ?? client.nombre ?? '').toUpperCase(),
        apellido:  (client.cl_apellido ?? client.apellido ?? '').toUpperCase(),
        telefono:  client.cl_telefono ?? client.telefono ?? '',
        direccion: (client.cl_direccion ?? client.direccion ?? '').toUpperCase(),
        ciudad:    (client.cl_ciudad ?? client.ciudad ?? '').toUpperCase(),
        titulo:    (client.cl_titulo ?? client.titulo ?? '').toUpperCase(),
        correo:    (client.cl_correo ?? client.correo ?? '').toLowerCase()
      };
      this.showFormModal = true;
    } else if (action === 'eliminar') {
      this.showDeleteConfirmModal = true;
    }
  }

  async saveClient() {
    if (!this.formularioValido) return;

    // Payload formateado para el SP
    const payload = {
      idCliente: Number(this.clientForm.idCliente),
      cedula:    this.clientForm.cedula,
      nombre:    this.clientForm.nombre.toUpperCase().trim(),
      apellido:  this.clientForm.apellido.toUpperCase().trim(),
      telefono:  this.clientForm.telefono,
      direccion: this.clientForm.direccion.toUpperCase().trim(),
      ciudad:    this.clientForm.ciudad.toUpperCase().trim(),
      titulo:    this.clientForm.titulo.toUpperCase().trim(),
      correo:    this.clientForm.correo.toLowerCase().trim()
    };

    try {
      const resp = this.isEditing
        ? await this.authService.actualizarCliente(payload)
        : await this.authService.insertarCliente(payload);

      if (resp?.esExitoso) {
        this.closeModals();
        await this.cargarClientes();
      }
    } catch (error) { console.error(error); }
  }

  async confirmDelete() {
    if (!this.selectedClient) return;
    try {
      const id = this.selectedClient.cl_id ?? this.selectedClient.idCliente;
      const resp = await this.authService.eliminarCliente(id);
      if (resp?.esExitoso) {
        this.showDeleteConfirmModal = false;
        await this.cargarClientes();
      }
    } catch (e) { console.error(e); }
  }

  get formularioValido(): boolean {
    return this.clientForm.nombre.trim().length > 2 &&
           this.clientForm.cedula.length === 10;
  }

  validarSoloNumeros(e: any) {
    if (!/[0-9]/.test(String.fromCharCode(e.charCode))) e.preventDefault();
  }

  cambiarPagina(p: number) { this.paginaActual = p; }

  closeModals() {
    this.showFormModal = false;
    this.showDeleteConfirmModal = false;
    this.resetForm(); // Limpiar al cerrar para seguridad
  }
}
