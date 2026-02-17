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

  ensayosLista: any[] = [];
  clientes: any[] = [];
  catalogoEnsayos: any[] = [];
  filtroNombre: string = '';

  showFormModal: boolean = false;
  showSelectionModal: boolean = false;
  showAbonoModal: boolean = false;

  minFecha: string = '';
  fechaEntrega: string = '';

  listaEnsayosTmp: any[] = [];
  nuevoEnsayoTmp: any = {
    nombre: '',
    monto: null,
    numero: null,
    idCatalogo: 0
  };

  ensayoForm: any = {
    ensayo: { idCliente: 0, descripcion: '', ensayos: [] },
    abono: null
  };

  selectedEnsayoForAbono: any = null;
  nuevoAbonoMonto: number | null = null;

  // Propiedad para la lista agrupada de clientes
  clientesDeudores: any[] = [];

  ngOnInit() {
    this.cargarEnsayos();
    this.cargarClientes();
    this.cargarCatalogoMaster();
    this.establecerFechaMinima();
  }

  establecerFechaMinima() {
    const hoy = new Date();
    this.minFecha = hoy.toISOString().split('T')[0];
  }

  async cargarCatalogoMaster() {
    try {
      const resp = await this.authService.getCatalogoEnsayos();
      if (resp?.esExitoso) {
        this.catalogoEnsayos = (resp.datos || []).map((item: any) => ({
          idCatalogo: item.ct_id || item.idCatalogo,
          nombre: item.ct_nombre || item.nombre
        }));
      }
    } catch (error) { console.error('Error catálogo:', error); }
  }

  async cargarEnsayos() {
    try {
      const resp = await this.authService.getEnsayos();
      if (resp?.esExitoso) {
        this.ensayosLista = resp.datos || [];

        // Lógica de agrupación por cédula
        const grupos = this.ensayosLista.reduce((acc: any, curr: any) => {
          const key = curr.cedula;
          if (!acc[key]) {
            acc[key] = {
              cedula: curr.cedula,
              nombreCompleto: curr.nombreCompleto,
              totalAbonado: 0,
              totalAPagar: 0,
              saldoPendiente: 0,
              expandido: false,
              detalles: []
            };
          }
          acc[key].totalAbonado += curr.totalAbonado;
          acc[key].totalAPagar += curr.totalAPagar;
          acc[key].saldoPendiente += curr.saldoPendiente;
          acc[key].detalles.push(curr);
          return acc;
        }, {});

        this.clientesDeudores = Object.values(grupos);
      }
    } catch (error) { console.error('Error lista ensayos:', error); }
  }

  toggleCliente(cliente: any) {
    cliente.expandido = !cliente.expandido;
  }

  async cargarClientes() {
    try {
      const resp = await this.authService.getClientes();
      if (resp?.esExitoso) {
        this.clientes = (resp.datos || [])
          .filter((c: any) => c.estado === "1")
          .map((c: any) => ({
            idCliente: c.idCliente,
            nombre: c.nombre,
            apellido: c.apellido,
            cedula: c.cedula
          }));
      }
    } catch (error) { console.error('Error clientes:', error); }
  }

  openEnsayoSelection() {
    this.nuevoEnsayoTmp = { nombre: '', monto: null, numero: null, idCatalogo: 0 };
    this.showSelectionModal = true;
  }

  onEnsayoChange() {
    const seleccionado = this.catalogoEnsayos.find(e => e.nombre === this.nuevoEnsayoTmp.nombre);
    if (seleccionado) this.nuevoEnsayoTmp.idCatalogo = seleccionado.idCatalogo;
  }

  confirmarAgregarEnsayo() {
    if (
      this.nuevoEnsayoTmp.nombre &&
      this.nuevoEnsayoTmp.monto !== null && this.nuevoEnsayoTmp.monto > 0 &&
      this.nuevoEnsayoTmp.numero !== null && this.nuevoEnsayoTmp.numero > 0
    ) {
      this.listaEnsayosTmp.push({ ...this.nuevoEnsayoTmp });
      this.showSelectionModal = false;
    } else {
      alert('⚠️ Error: El número y el costo deben ser mayores a cero.');
    }
  }

  eliminarFilaEnsayo(index: number) { this.listaEnsayosTmp.splice(index, 1); }

  openAbonoModal(item: any) {
    this.selectedEnsayoForAbono = item;
    this.nuevoAbonoMonto = null;
    this.showAbonoModal = true;
  }

  async guardarNuevoAbono() {
    // Si se abona desde la fila principal (objeto agrupado), buscamos el primer ensayo con saldo
    let idEnsayoParaPago = this.selectedEnsayoForAbono.idPrueba;

    if (!idEnsayoParaPago && this.selectedEnsayoForAbono.detalles) {
        const ensayoConSaldo = this.selectedEnsayoForAbono.detalles.find((d: any) => d.saldoPendiente > 0);
        idEnsayoParaPago = ensayoConSaldo ? ensayoConSaldo.idPrueba : this.selectedEnsayoForAbono.detalles[0].idPrueba;
    }

    const payload = {
      idEnsayo: idEnsayoParaPago,
      monto: this.nuevoAbonoMonto,
      usuario: localStorage.getItem('usuario') || 'emy'
    };

    try {
      const resp = await this.authService.insertarAbono(payload);
      if (resp?.esExitoso) {
        alert('✅ Abono registrado correctamente.');
        this.showAbonoModal = false;
        await this.cargarEnsayos();
      }
    } catch (error) {
      alert('⚠️ Error de comunicación.');
    }
  }

  calcularTotalPedido(): number {
    return this.listaEnsayosTmp.reduce((acc, curr) => acc + (curr.monto || 0), 0);
  }

  formularioValido(): boolean {
    return this.ensayoForm.ensayo.idCliente > 0 && this.fechaEntrega !== '' && this.listaEnsayosTmp.length > 0;
  }

  /**
   * Bloquea caracteres no numéricos y el signo menos (-) físicamente
   */
  soloNumeros(event: any) {
    const pattern = /[0-9.]/;
    const inputChar = String.fromCharCode(event.charCode);

    // Bloquear si no es número o punto decimal
    if (!pattern.test(inputChar)) {
      event.preventDefault();
      return;
    }

    // Evitar múltiples puntos decimales
    if (inputChar === '.' && event.target.value.includes('.')) {
      event.preventDefault();
    }
  }

  /**
   * Valida que no se peguen valores negativos o inválidos
   */
  validarPegado(event: ClipboardEvent) {
    const pastedText = event.clipboardData?.getData('text');
    if (pastedText && (pastedText.includes('-') || isNaN(Number(pastedText)))) {
      event.preventDefault();
      alert('⚠️ No se permiten valores negativos o caracteres inválidos.');
    }
  }

  async guardarEnsayo() {
    const payload = {
      idCliente: this.ensayoForm.ensayo.idCliente,
      descripcion: this.ensayoForm.ensayo.descripcion,
      abono: this.ensayoForm.abono || 0,
      fechaEntrega: new Date(this.fechaEntrega).toISOString(),
      ensayos: this.listaEnsayosTmp.map(e => ({
        idCatalogo: e.idCatalogo,
        monto: e.monto,
        numeroEnsayo: parseInt(e.numero.toString())
      }))
    };

    try {
      const resp = await this.authService.insertarEnsayo(payload);
      if (resp?.esExitoso) {
        this.closeModals();
        this.cargarEnsayos();
        alert('✅ Pedido guardado correctamente.');
      }
    } catch (error) { alert('⚠️ Error al guardar pedido.'); }
  }

  openModal() { this.resetForm(); this.showFormModal = true; }

  closeModals() {
    this.showFormModal = false;
    this.showSelectionModal = false;
    this.showAbonoModal = false;
  }

  resetForm() {
    this.listaEnsayosTmp = [];
    this.fechaEntrega = '';
    this.ensayoForm = {
      ensayo: { idCliente: 0, descripcion: '', ensayos: [] },
      abono: null
    };
  }
}
