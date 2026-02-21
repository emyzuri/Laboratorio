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

  ensayosDeudores: any[] = [];
  clientes: any[] = [];
  catalogoEnsayos: any[] = [];

  filtroNombre: string = '';
  filtroClienteBusqueda: string = '';
  paginaActual: number = 1;

  showFormModal: boolean = false;
  showSelectionModal: boolean = false;
  showAbonoModal: boolean = false;

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
  showResults = false;

  seleccionarCliente(cliente: any) {
    this.filtroClienteBusqueda = `${cliente.cedula} - ${cliente.nombre} ${cliente.apellido}`;
    this.ensayoForm.ensayo.idCliente = cliente.idCliente;
    this.showResults = false;
  }

  ngOnInit() {
    this.cargarDatosIniciales();
    this.establecerFechaMinima();
  }

  async cargarDatosIniciales() {
    await this.cargarEnsayos();
    await this.cargarClientes();
    await this.cargarCatalogo();
  }

  get ensayosFiltradosPorBusqueda() {
    const query = this.filtroNombre.toLowerCase().trim();
    if (!query) return this.ensayosDeudores;

    return this.ensayosDeudores.filter(e =>
      (e.nombreCompleto || '').toLowerCase().includes(query) ||
      (e.cedula || '').includes(query) ||
      (e.subensayos || []).some((sub: any) =>
        (sub.nombreSubEnsayo || '').toLowerCase().includes(query)
      )
    );
  }

  async cargarEnsayos() {
    try {
      const resp = await this.authService.getEnsayosDeudores();
      if (resp?.esExitoso) {
        this.ensayosDeudores = resp.datos.map((e: any) => ({
          cedula: e.cedula || '',
          nombreCompleto: e.nombreCompleto || 'Sin nombre',
          totalAbonado: e.totalAbonado ?? 0,
          totalAPagar: e.totalAPagar ?? 0,
          saldoPendiente: e.saldoPendiente ?? 0,
          idPrueba: e.idEnsayo || 0,
          expandido: false,
          subensayos: (e.ensayos || []).map((s: any) => ({
            nombreSubEnsayo: s.nombreCatalogo || 'No especificado',
            saldoSub: e.saldoPendiente
          }))
        }));
      }
    } catch (error) {
      console.error('Error al cargar ensayos:', error);
    }
  }

  async cargarClientes() {
    try {
      const resp = await this.authService.getClientes();
      if (resp?.esExitoso) {
        this.clientes = resp.datos.filter(
          (c: any) => c.cl_estado == 1 || c.estado == 1
        );
      }
    } catch (error) {
      console.error('Error al cargar clientes:', error);
    }
  }

  async cargarCatalogo() {
    try {
      const resp = await this.authService.getCatalogoEnsayos();
      if (resp?.esExitoso) {
        this.catalogoEnsayos = resp.datos.map((cat: any) => ({
          ...cat,
          nombre: cat.ct_nombre || cat.nombre,
          idCatalogo: cat.ct_id || cat.idCatalogo
        }));
      }
    } catch (error) {
      console.error('Error al cargar catálogo:', error);
    }
  }

  openAbonoModal(ensayo: any) {
    this.selectedEnsayoForAbono = ensayo;
    this.nuevoAbonoMonto = null;
    this.showAbonoModal = true;
  }

  async guardarNuevoAbono() {
    if (!this.selectedEnsayoForAbono || !this.nuevoAbonoMonto) return;

    const payload = {
      idEnsayo: this.selectedEnsayoForAbono.idPrueba,
      monto: this.nuevoAbonoMonto,
      usuario: JSON.parse(localStorage.getItem('usuarioLogueado') || '{}').usuario || 'admin'
    };

    try {
      const resp = await this.authService.insertarAbono(payload);
      if (resp?.esExitoso) {
        this.closeModals();
        await this.cargarEnsayos();
      }
    } catch (error) {
      console.error('Error al registrar abono:', error);
    }
  }

  onClienteSearchChange() {
    const encontrado = this.clientes.find(c => {
      const etiqueta = `${c.cedula || c.cl_cedula} - ${c.nombre || c.cl_nombre} ${c.apellido || c.cl_apellido}`;
      return etiqueta.trim() === this.filtroClienteBusqueda.trim();
    });

    this.ensayoForm.ensayo.idCliente = encontrado ? (encontrado.cl_id || encontrado.idCliente) : 0;
  }

  async guardarEnsayo() {
    const payload = {
      idCliente: this.ensayoForm.ensayo.idCliente,
      descripcion: this.ensayoForm.ensayo.descripcion,
      abono: this.ensayoForm.abono || 0,
      fechaEntrega: this.fechaEntrega ? new Date(this.fechaEntrega).toISOString() : null,
      ensayos: this.listaEnsayosTmp.map(e => ({
        idCatalogo: e.idCatalogo,
        monto: e.monto,
        numeroEnsayo: parseInt(e.numero?.toString() || '0')
      }))
    };

    try {
      const resp = await this.authService.insertarEnsayo(payload);
      if (resp?.esExitoso) {
        this.closeModals();
        await this.cargarEnsayos();
      }
    } catch (error) {
      console.error('Error al guardar ensayo:', error);
    }
  }

  openModal() { this.resetForm(); this.showFormModal = true; }
  closeModals() { this.showFormModal = false; this.showSelectionModal = false; this.showAbonoModal = false; }
  toggleEnsayo(ensayo: any) { ensayo.expandido = !ensayo.expandido; }

  onEnsayoChange() {
    const sel = this.catalogoEnsayos.find(c => c.nombre === this.nuevoEnsayoTmp.nombre);
    if (sel) this.nuevoEnsayoTmp.idCatalogo = sel.idCatalogo;
  }

  confirmarAgregarEnsayo() {
    if (this.nuevoEnsayoTmp.nombre && this.nuevoEnsayoTmp.monto > 0) {
      this.listaEnsayosTmp.push({ ...this.nuevoEnsayoTmp });
      this.showSelectionModal = false;
    }
  }

  resetForm() {
    this.listaEnsayosTmp = [];
    this.fechaEntrega = '';
    this.filtroClienteBusqueda = '';
    this.ensayoForm = { ensayo: { idCliente: 0, descripcion: '' }, abono: null };
  }

  establecerFechaMinima() {
    this.minFecha = new Date().toISOString().split('T')[0];
  }

  calcularTotalPedido() {
    return this.listaEnsayosTmp.reduce((acc, curr) => acc + (curr.monto || 0), 0);
  }

  formularioValido() {
    return this.ensayoForm.ensayo.idCliente > 0 && this.fechaEntrega !== '' && this.listaEnsayosTmp.length > 0;
  }

  // ==========================================
  // BLOQUEO DE NUMEROS NEGATIVOS
  // ==========================================
  soloNumeros(event: KeyboardEvent) {
    const char = String.fromCharCode(event.charCode);
    if (!/[0-9.]/.test(char)) event.preventDefault();
  }

  evitarPegadoNegativo(event: ClipboardEvent) {
    const clipboardData = event.clipboardData?.getData('text');
    if (!clipboardData) return;
    if (/[^0-9.]/.test(clipboardData)) event.preventDefault();
  }
}
