import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-ensayos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ensayos.html',
  styleUrls: ['./ensayos.scss']
})
export class EnsayosComponent implements OnInit {

  private readonly authService = inject(AuthService);

  // =========================
  // LISTAS PRINCIPALES
  // =========================
  ensayosOriginal: any[] = [];
  ensayosFiltrados: any[] = [];

  clientes: any[] = [];
  clientesFiltrados: any[] = [];

  catalogoPadres: any[] = [];
  catalogoHijos: any[] = [];

  idPadreSeleccionado: number = 0;

  // =========================
  // FILTROS Y UI
  // =========================
  filtroNombre: string = '';
  filtroClienteBusqueda: string = '';

  showFormModal = false;
  showSelectionModal = false;
  showAbonoModal = false;
  showResults = false;

  // =========================
  // FORMULARIO
  // =========================
  minFecha: string = '';
  fechaEntrega: string = '';

  listaEnsayosTmp: any[] = [];
  nuevoEnsayoTmp: any = { nombre: '', monto: null, numero: null, idCatalogo: 0 };

  ensayoForm: any = {
    ensayo: { idCliente: 0, descripcion: '' },
    abono: null
  };

  selectedEnsayoForAbono: any = null;
  nuevoAbonoMonto: number | null = null;

  // =========================
  ngOnInit() {
    this.cargarDatosIniciales();
    this.establecerFechaMinima();
  }

  async cargarDatosIniciales() {
    await this.cargarEnsayos();
    await this.cargarClientes();
    await this.cargarCatalogoPadres();
  }

  async cargarEnsayos() {
  try {
    const resp = await this.authService.getEnsayosDeudores();

    if (resp?.esExitoso && Array.isArray(resp.datos)) {

      const soloDeudores = resp.datos.filter((e: any) => Number(e.saldoPendiente ?? 0) > 0);

      this.ensayosOriginal = soloDeudores.map((e: any) => ({
        cedula: e.cedula || '',
        nombreCompleto: (e.nombreCompleto || '').toUpperCase(),
        totalAbonado: Number(e.totalAbonado ?? 0),
        totalAPagar: Number(e.totalAPagar ?? 0),
        saldoPendiente: Number(e.saldoPendiente ?? 0),
        idPrueba: e.idEnsayo ?? 0,
        expandido: false,
        subensayos: Array.isArray(e.ensayos)
          ? e.ensayos.map((s: any) => ({
              nombreSubEnsayo: (s.nombreCatalogo || 'ENSAYO').toUpperCase(),
              saldoSub: 0
            }))
          : []
      }));

      this.ensayosFiltrados = [...this.ensayosOriginal];
    }
  } catch (error) {
    console.error('Error al cargar ensayos:', error);
  }
}

  // =========================
  // FILTRO EN TIEMPO REAL
  // =========================
  filtrarEnsayos() {

    const query = this.filtroNombre?.toLowerCase().trim();

    if (!query) {
      this.ensayosFiltrados = [...this.ensayosOriginal];
      return;
    }

    this.ensayosFiltrados = this.ensayosOriginal.filter(e => {

      const nombre = (e.nombreCompleto || '').toLowerCase();
      const cedula = (e.cedula || '').toLowerCase();

      const coincideSub = (e.subensayos || []).some((sub: any) =>
        (sub.nombreSubEnsayo || '').toLowerCase().includes(query)
      );

      return (
        nombre.includes(query) ||
        cedula.includes(query) ||
        coincideSub
      );
    });
  }

  // =========================
  async cargarClientes() {
    const resp = await this.authService.getClientes();
    if (resp?.esExitoso) {
      this.clientes = resp.datos.filter((c: any) => (c.cl_estado ?? c.estado) != 0);
    }
  }

  onClienteSearchChange() {

    const bus = this.filtroClienteBusqueda.toLowerCase().trim();

    if (!bus) {
      this.clientesFiltrados = [];
      this.showResults = false;
      this.ensayoForm.ensayo.idCliente = 0;
      return;
    }

    this.showResults = true;

    this.clientesFiltrados = this.clientes.filter(c =>
      (c.cl_cedula || c.cedula || '').includes(bus) ||
      (c.cl_nombre || c.nombre || '').toLowerCase().includes(bus) ||
      (c.cl_apellido || c.apellido || '').toLowerCase().includes(bus)
    );
  }

  seleccionarCliente(cliente: any) {

    const nombre = cliente.cl_nombre || cliente.nombre;
    const apellido = cliente.cl_apellido || cliente.apellido;
    const cedula = cliente.cl_cedula || cliente.cedula;

    this.filtroClienteBusqueda = `${cedula} - ${nombre} ${apellido}`.toUpperCase();
    this.ensayoForm.ensayo.idCliente = cliente.cl_id || cliente.idCliente;
    this.showResults = false;
  }

  // =========================
  async cargarCatalogoPadres() {

    const resp = await this.authService.getCatalogoEnsayosPorPadre(0);

    console.log('RESP PADRES:', resp);

    if (resp?.esExitoso && resp?.datos?.datos) {

      this.catalogoPadres = resp.datos.datos;

    } else {
      this.catalogoPadres = [];
    }
  }

  async onPadreChange() {

  this.catalogoHijos = [];
  this.nuevoEnsayoTmp.idCatalogo = 0;

  if (!this.idPadreSeleccionado) return;

  const resp = await this.authService.getCatalogoEnsayosPorPadre(this.idPadreSeleccionado);

  console.log('RESP HIJOS:', resp);

  if (resp?.esExitoso && resp?.datos?.datos) {

    this.catalogoHijos = resp.datos.datos;

  }
}

  onEnsayoHijoChange() {
    const sel = this.catalogoHijos.find(h => (h.id || h.ct_id) == this.nuevoEnsayoTmp.idCatalogo);
    if (sel) {
      this.nuevoEnsayoTmp.nombre = (sel.nombre || sel.ct_nombre).toUpperCase();
    }
  }

  // =========================
  toggleEnsayo(ensayo: any) {
    ensayo.expandido = !ensayo.expandido;
  }

  openModal() {
    this.resetForm();
    this.showFormModal = true;
  }

  openAbonoModal(ensayo: any) {
    this.selectedEnsayoForAbono = ensayo;
    this.nuevoAbonoMonto = null;
    this.showAbonoModal = true;
  }

  closeModals() {
    this.showFormModal = false;
    this.showSelectionModal = false;
    this.showAbonoModal = false;
    this.resetForm();
  }

  resetForm() {
    this.listaEnsayosTmp = [];
    this.fechaEntrega = '';
    this.filtroClienteBusqueda = '';
    this.ensayoForm = { ensayo: { idCliente: 0, descripcion: '' }, abono: null };
    this.nuevoEnsayoTmp = { nombre: '', monto: null, numero: null, idCatalogo: 0 };
    this.idPadreSeleccionado = 0;
    this.catalogoHijos = [];
  }

  confirmarAgregarEnsayo() {

    if (this.nuevoEnsayoTmp.idCatalogo > 0 && this.nuevoEnsayoTmp.monto > 0) {

      this.listaEnsayosTmp.push({ ...this.nuevoEnsayoTmp });

      this.nuevoEnsayoTmp = { nombre: '', monto: null, numero: null, idCatalogo: 0 };
      this.idPadreSeleccionado = 0;
      this.catalogoHijos = [];

      this.showSelectionModal = false;
    }
  }

  async guardarEnsayo() {

    if (!this.formularioValido()) return;

    const payload = {
      idCliente: this.ensayoForm.ensayo.idCliente,
      descripcion: (this.ensayoForm.ensayo.descripcion || '').toUpperCase(),
      abono: this.ensayoForm.abono || 0,
      fechaEntrega: this.fechaEntrega ? new Date(this.fechaEntrega).toISOString() : null,
      ensayos: this.listaEnsayosTmp.map(e => ({
        idCatalogo: e.idCatalogo,
        monto: e.monto,
        numeroEnsayo: parseInt(e.numero?.toString() || '0')
      }))
    };

    const resp = await this.authService.insertarEnsayo(payload);
    if (resp?.esExitoso) {
      this.closeModals();
      await this.cargarEnsayos();
    }
  }

  async guardarNuevoAbono() {

    if (!this.selectedEnsayoForAbono || !this.nuevoAbonoMonto) return;

    const payload = {
      idEnsayo: this.selectedEnsayoForAbono.idPrueba,
      monto: this.nuevoAbonoMonto,
      usuario: JSON.parse(localStorage.getItem('usuarioLogueado') || '{}').usuario || 'SISTEMA'
    };

    const resp = await this.authService.insertarAbono(payload);

    if (resp?.esExitoso) {
      this.closeModals();
      await this.cargarEnsayos();
    }
  }

  establecerFechaMinima() {
    this.minFecha = new Date().toISOString().split('T')[0];
  }

  calcularTotalPedido() {
    return this.listaEnsayosTmp.reduce((acc, curr) => acc + (curr.monto || 0), 0);
  }

  formularioValido() {
    return this.ensayoForm.ensayo.idCliente > 0 &&
           this.fechaEntrega !== '' &&
           this.listaEnsayosTmp.length > 0;
  }

  soloNumeros(event: KeyboardEvent) {
    const char = String.fromCharCode(event.charCode);
    if (!/[0-9.]/.test(char)) event.preventDefault();
  }

  evitarPegadoNegativo(event: ClipboardEvent) {
    const clipboardData = event.clipboardData?.getData('text');
    if (!clipboardData || /[^0-9.]/.test(clipboardData)) event.preventDefault();
  }
}
