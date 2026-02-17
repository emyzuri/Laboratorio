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

  /**
   * Algoritmo de validación de cédula ecuatoriana
   */
  validarCedulaEcuatoriana(cedula: string): boolean {
    if (!cedula || cedula.length !== 10) return false;

    // Verificar los dos primeros dígitos (provincia 01-24 o 30)
    const provincia = parseInt(cedula.substring(0, 2), 10);
    if (!((provincia >= 1 && provincia <= 24) || provincia === 30)) return false;

    // Verificar el tercer dígito (debe ser menor a 6)
    const tercerDigito = parseInt(cedula.substring(2, 3), 10);
    if (tercerDigito >= 6) return false;

    // Coeficientes para el algoritmo de módulo 10
    const coeficientes = [2, 1, 2, 1, 2, 1, 2, 1, 2];
    const verificador = parseInt(cedula.substring(9, 10), 10);
    let suma = 0;

    for (let i = 0; i < 9; i++) {
      let valor = parseInt(cedula.substring(i, i + 1), 10) * coeficientes[i];
      if (valor >= 10) valor -= 9;
      suma += valor;
    }

    const totalCalculado = suma % 10 === 0 ? 0 : 10 - (suma % 10);
    return totalCalculado === verificador;
  }

  /**
   * Getter que valida campos llenos y cédula válida de Ecuador
   */
  get formularioValido(): boolean {
    const f = this.clientForm;
    const camposLlenos = !!(f.cedula && f.nombre && f.apellido && f.telefono);
    const cedulaCorrecta = this.validarCedulaEcuatoriana(f.cedula);

    return camposLlenos && cedulaCorrecta;
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
    if (!this.formularioValido) return;

    if (!this.isEditing) {
      const existe = this.clientes.some(c => (c.cl_cedula || c.cedula) === this.clientForm.cedula);
      if (existe) {
        alert('🚫 Error: Esta cédula ya está registrada.');
        return;
      }
    }

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
  }
}
