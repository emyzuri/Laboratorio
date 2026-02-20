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

  // --- ESTADO DE DATOS ---
  clientes: any[] = [];
  filtroBusqueda: string = '';
  selectedClient: any = null;
  paginaActual: number = 1;
  itemsPorPagina: number = 10;

  // --- ESTADO DE MODALES ---
  showFormModal = false;
  showSuccessModal = false;
  showDeleteConfirmModal = false;
  isEditing = false;
  successMessage = '';

  // --- FORMULARIO ---
  clientForm = {
    idCliente: 0,
    cedula: '',
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

  // --- VALIDACIONES PERSONALIZADAS ---

  validarSoloNumeros(event: any) {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }

  // --- DENTRO DE TU CLASE ---

/**
 * Valida si una cédula es ecuatoriana real (Algoritmo Módulo 10)
 */
get esCedulaEcuatorianaValida(): boolean {
  const cedula = this.clientForm.cedula;

  // 1. Debe tener 10 dígitos exactos y ser solo números
  // Corregido: Quitamos el .slice() innecesario
  if (!cedula || cedula.length !== 10 || !/^\d+$/.test(cedula)) {
    return false;
  }

  // 2. Verificar código de provincia (01 a 24) o 30 (Extranjeros)
  const provincia = parseInt(cedula.substring(0, 2), 10);
  if (!((provincia >= 1 && provincia <= 24) || provincia === 30)) {
    return false;
  }

  // 3. Verificar el tercer dígito (debe ser menor a 6 para personas naturales)
  const tercerDigito = parseInt(cedula.substring(2, 3), 10);
  if (tercerDigito >= 6) {
    return false;
  }

  // 4. Algoritmo de Módulo 10
  const coeficientes = [2, 1, 2, 1, 2, 1, 2, 1, 2];
  const verificador = parseInt(cedula.substring(9, 10), 10);
  let suma = 0;

  for (let i = 0; i < coeficientes.length; i++) {
    let valor = parseInt(cedula.substring(i, i + 1), 10) * coeficientes[i];
    if (valor >= 10) {
      valor -= 9;
    }
    suma += valor;
  }

  const residuo = suma % 10;
  const resultadoEsperado = residuo === 0 ? 0 : 10 - residuo;

  return resultadoEsperado === verificador;
}
/**
 * Control central del botón GUARDAR
 */
get formularioValido(): boolean {
  const f = this.clientForm;

  // Verificamos que todos los campos tengan contenido (sin espacios vacíos)
  const camposLlenos =
    f.cedula.trim() !== '' &&
    f.nombre.trim() !== '' &&
    f.apellido.trim() !== '' &&
    f.telefono.trim() !== '' &&
    f.direccion.trim() !== '' &&
    f.ciudad.trim() !== '' &&
    f.titulo.trim() !== '';

  // El teléfono debe tener 10 dígitos
  const telefonoOk = f.telefono.length === 10;

  // La cédula debe ser válida bajo el algoritmo ecuatoriano
  // Si estamos editando, usualmente la cédula viene bloqueada o ya validada
  const cedulaOk = this.isEditing ? true : this.esCedulaEcuatorianaValida;

  return camposLlenos && telefonoOk && cedulaOk;
}

  // --- CARGA DE DATOS ---
  async cargarClientes() {
    try {
      const respuesta = await this.authService.getClientes();
      if (respuesta?.esExitoso) {
        this.clientes = respuesta.datos || [];
        this.ajustarPaginaDespuesDeCarga();
      }
    } catch (error) {
      console.error("Error al cargar lista:", error);
    }
  }

  // --- LÓGICA DE TABLA ---
  get clientesPaginados() {
    const busqueda = this.filtroBusqueda.toLowerCase();
    const filtrados = this.clientes.filter(c => {
      const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
      const estaActivo = valorEstado === undefined || valorEstado === null || (valorEstado != 0 && valorEstado != '0');
      const coincideBusqueda =
        (c.cl_cedula || c.cedula || '').toLowerCase().includes(busqueda) ||
        (c.cl_nombre || c.nombre || '').toLowerCase().includes(busqueda) ||
        (c.cl_apellido || c.apellido || '').toLowerCase().includes(busqueda);
      return estaActivo && coincideBusqueda;
    });

    const ordenados = filtrados.sort((a, b) => {
      const nombreA = (a.cl_nombre || a.nombre || '').toLowerCase();
      const nombreB = (b.cl_nombre || b.nombre || '').toLowerCase();
      return nombreA.localeCompare(nombreB);
    });

    const inicio = (this.paginaActual - 1) * this.itemsPorPagina;
    return ordenados.slice(inicio, inicio + this.itemsPorPagina);
  }

  get totalPaginas(): number {
    const busqueda = this.filtroBusqueda.toLowerCase();
    const filtrados = this.clientes.filter(c => {
      const valorEstado = c.cl_estado !== undefined ? c.cl_estado : c.estado;
      const estaActivo = valorEstado === undefined || valorEstado === null || (valorEstado != 0 && valorEstado != '0');
      return estaActivo && (
        (c.cl_cedula || c.cedula || '').toLowerCase().includes(busqueda) ||
        (c.cl_nombre || c.nombre || '').toLowerCase().includes(busqueda)
      );
    });
    return Math.ceil(filtrados.length / this.itemsPorPagina) || 1;
  }

  cambiarPagina(p: number) { this.paginaActual = p; }

  private ajustarPaginaDespuesDeCarga() {
    if (this.clientesPaginados.length === 0 && this.paginaActual > 1) this.paginaActual--;
  }

  // --- GESTIÓN DE MODALES ---
  openNewClientModal() {
    this.isEditing = false;
    this.resetForm();
    this.showFormModal = true;
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

  closeModals() {
    this.showFormModal = false;
    this.showDeleteConfirmModal = false;
  }

  // --- OPERACIONES ---
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
      let respuesta;
      if (this.isEditing) {
        respuesta = await this.authService.actualizarCliente(payload);
      } else {
        respuesta = await this.authService.insertarCliente({ ...payload, cedula: this.clientForm.cedula });
      }

      if (respuesta?.esExitoso) {
        this.successMessage = this.isEditing ? 'Actualizado Exitosamente' : 'Registrado Exitosamente';
        this.showFormModal = false;
        this.showSuccessModal = true;
        await this.cargarClientes();
      }
    } catch (error) {
      console.error(error);
    }
  }

  async confirmDelete() {
    if (!this.selectedClient) return;
    try {
      const id = this.selectedClient.cl_id ?? this.selectedClient.idCliente;
      const respuesta = await this.authService.eliminarCliente(id);
      if (respuesta?.esExitoso) {
        this.showDeleteConfirmModal = false;
        await this.cargarClientes();
      }
    } catch (error) { console.error(error); }
  }

  resetForm() {
    this.clientForm = { idCliente: 0, cedula: '', nombre: '', apellido: '', telefono: '', direccion: '', ciudad: '', titulo: '' };
    this.selectedClient = null;
  }
}
