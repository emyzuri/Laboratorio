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

  minFecha: string = '';
  fechaEntrega: string = '';

  listaEnsayosTmp: any[] = [];
  nuevoEnsayoTmp = {
    nombre: '',
    monto: 0,
    numero: '',
    idCatalogo: 0
  };

  ensayoForm = {
    ensayo: {
      idCliente: 0,
      descripcion: '',
      ensayos: [] as any[]
    },
    abono: 0
  };

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
        this.catalogoEnsayos = resp.datos || [];
      }
    } catch (error) {
      console.error('Error al cargar catálogo:', error);
    }
  }
  async cargarEnsayos() {
    try {
      const resp = await this.authService.getEnsayos();
      if (resp?.esExitoso) {
        this.ensayosLista = resp.datos || [];
        const nombresBrutos = this.ensayosLista.map(item => item.ensayo);
        this.catalogoEnsayos = [...new Set(nombresBrutos)].filter(n => n && n.trim() !== '');
      }
    } catch (error) {
      console.error('Error al mapear catálogo:', error);
    }
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
    } catch (error) {
      console.error('Error al cargar clientes:', error);
    }
  }

  openEnsayoSelection() {
    this.nuevoEnsayoTmp = { nombre: '', monto: 0, numero: '', idCatalogo: 0 };
    this.showSelectionModal = true;
  }
  onEnsayoChange() {
  const seleccionado = this.catalogoEnsayos.find(e => e.nombre === this.nuevoEnsayoTmp.nombre);
  if (seleccionado) {
    this.nuevoEnsayoTmp.idCatalogo = seleccionado.idCatalogo;
  }
}

  confirmarAgregarEnsayo() {
    const numEnsayo = Number(this.nuevoEnsayoTmp.numero);
    if (this.nuevoEnsayoTmp.nombre && this.nuevoEnsayoTmp.monto > 0 && !isNaN(numEnsayo) && numEnsayo > 0) {
      this.listaEnsayosTmp.push({ ...this.nuevoEnsayoTmp });
      this.showSelectionModal = false;
    } else {
      alert('Por favor, ingrese un número de ensayo válido y un costo mayor a cero.');
    }
  }

  eliminarFilaEnsayo(index: number) {
    this.listaEnsayosTmp.splice(index, 1);
  }

  calcularTotalPedido(): number {
    return this.listaEnsayosTmp.reduce((acc, curr) => acc + (curr.monto || 0), 0);
  }

  formularioValido(): boolean {
    return this.ensayoForm.ensayo.idCliente > 0 && this.fechaEntrega !== '' && this.listaEnsayosTmp.length > 0;
  }

  soloNumeros(event: any) {
    const pattern = /[0-9.]/;
    const inputChar = String.fromCharCode(event.charCode);
    if (inputChar === '.' && event.target.value.includes('.')) {
      event.preventDefault();
      return;
    }
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    }
  }

  async guardarEnsayo() {
    const payload = {
      idCliente: this.ensayoForm.ensayo.idCliente,
      descripcion: this.ensayoForm.ensayo.descripcion,
      abono: this.ensayoForm.abono,
      fechaEntrega: new Date(this.fechaEntrega).toISOString(),
      ensayos: this.listaEnsayosTmp.map(e => ({
        idCatalogo: e.idCatalogo || 1,
        monto: e.monto,
        numeroEnsayo: parseInt(e.numero.toString())
      }))
    };

    try {
      const resp = await this.authService.insertarEnsayo(payload);

      if (resp?.esExitoso) {
        this.closeModals();
        this.cargarEnsayos();
        alert('✅ Ensayo Guardado Correctamente');
      } else {
        alert('Error: ' + (resp?.mensaje || 'No se pudo completar el registro'));
      }
    } catch (error) {
      console.error('Error al guardar:', error);
      alert('⚠️ Error de comunicación. Verifique que el servidor esté activo.');
    }
  }

  openModal() {
    this.resetForm();
    this.showFormModal = true;
  }

  closeModals() {
    this.showFormModal = false;
    this.showSelectionModal = false;
  }

  resetForm() {
    this.listaEnsayosTmp = [];
    this.fechaEntrega = '';
    this.ensayoForm = {
      ensayo: { idCliente: 0, descripcion: '', ensayos: [] },
      abono: 0
    };
  }
  getEstadoDeuda(item: any): string {
    if (item.saldoPendiente <= 0) return 'Pagado';
    if (item.totalAbonado > 0) return 'Abonado';
    return 'Pendiente';
  }
}
