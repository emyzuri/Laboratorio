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

  // --- LÓGICA DE FILTRADO (ESTADO 0 = OCULTO) ---
  get clientesPaginados() {
    const bus = this.filtroBusqueda.toLowerCase().trim();

    const filtrados = this.clientes.filter(c => {
      // Filtro 1: Solo registros activos (Estado diferente de 0)
      const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
      const estaActivo = valorEstado != 0 && valorEstado != '0';

      // Filtro 2: Coincidencia con búsqueda
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
      const estaActivo = valorEstado != 0 && valorEstado != '0';
      const coincide = (c.cl_nombre || c.nombre || '').toLowerCase().includes(bus) ||
                       (c.cl_cedula || c.cedula || '').includes(bus);
      return estaActivo && coincide;
    }).length;
    return Math.ceil(totalActivos / this.itemsPorPagina) || 1;
  }

  async cargarClientes() {
    try {
      const resp = await this.authService.getClientes();
      if (resp?.esExitoso) this.clientes = resp.datos || [];
    } catch (e) { console.error(e); }
  }

  handleAction(action: string, client: any) {
    this.selectedClient = { ...client };
    if (action === 'editar') {
      this.isEditing = true;
      this.clientForm = {
        idCliente: client.cl_id ?? client.idCliente ?? 0,
        cedula:    client.cl_cedula ?? client.cedula ?? '',
        nombre:    client.cl_nombre ?? client.nombre ?? '',
        apellido:  client.cl_apellido ?? client.apellido ?? '',
        telefono:  client.cl_telefono ?? client.telefono ?? '',
        direccion: client.cl_direccion ?? client.direccion ?? '',
        ciudad:    client.cl_ciudad ?? client.ciudad ?? '',
        titulo:    client.cl_titulo ?? client.titulo ?? ''
      };
      this.showFormModal = true;
    } else if (action === 'eliminar') {
      this.showDeleteConfirmModal = true;
    }
  }

  async saveClient() {
    if (!this.formularioValido) return;
    const payload = {
      idCliente: Number(this.clientForm.idCliente),
      nombre:    this.clientForm.nombre,
      apellido:  this.clientForm.apellido,
      telefono:  this.clientForm.telefono,
      direccion: this.clientForm.direccion,
      ciudad:    this.clientForm.ciudad,
      titulo:    this.clientForm.titulo
    };

    try {
      const resp = this.isEditing
        ? await this.authService.actualizarCliente(payload)
        : await this.authService.insertarCliente({ ...payload, cedula: this.clientForm.cedula });

      if (resp?.esExitoso) {
        this.showFormModal = false;
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
        await this.cargarClientes(); // Recarga y el filtro oculta el estado 0
      }
    } catch (e) { console.error(e); }
  }

  // --- HELPERS ---
  get formularioValido(): boolean {
    return this.clientForm.nombre.length > 2 && this.clientForm.cedula.length === 10;
  }
  validarSoloNumeros(e: any) { if (!/[0-9]/.test(String.fromCharCode(e.charCode))) e.preventDefault(); }
  cambiarPagina(p: number) { this.paginaActual = p; }
  closeModals() { this.showFormModal = this.showDeleteConfirmModal = false; }
  openNewClientModal() { this.isEditing = false; this.resetForm(); this.showFormModal = true; }
  resetForm() { this.clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' }; }
}
