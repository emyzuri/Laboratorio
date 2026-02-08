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

  // Listas de datos
  ensayosLista: any[] = [];
  clientes: any[] = [];
  filtroNombre: string = '';

  // Control de Modales
  showFormModal: boolean = false;
  showSuccessModal: boolean = false;

  // Estructura para el POST sincronizada con tu API
  ensayoForm = {
    ensayo: {
      idCliente: 0,
      descripcion: '',
      ensayos: [{ idCatalogo: 1, monto: 0, abono: 0 }]
    },
    abono: 0
  };

  ngOnInit() {
    this.cargarEnsayos();
    this.cargarClientes();
  }

  async cargarEnsayos() {
    try {
      // Consumimos el endpoint de deudores confirmado en Postman
      const resp = await this.authService.getEnsayos();
      if (resp?.esExitoso) {
        this.ensayosLista = resp.datos || [];
      }
    } catch (error) {
      console.error('Error al cargar lista de deudores:', error);
    }
  }

  async cargarClientes() {
    try {
      const resp = await this.authService.getClientes();
      if (resp?.esExitoso) {
        // Filtramos por estado "1" según tu Postman
        this.clientes = (resp.datos || [])
          .filter((c: any) => (c.estado === "1" || c.estado === 1))
          .map((c: any) => ({
            idCliente: c.idCliente,
            nombre: c.nombre,
            apellido: c.apellido
          }));
      }
    } catch (error) {
      console.error('Error al cargar clientes:', error);
    }
  }

  async guardarEnsayo() {
    if (this.ensayoForm.ensayo.idCliente === 0) {
      alert('Por favor, seleccione un cliente activo.');
      return;
    }

    try {
      // Petición POST ahora sin depender obligatoriamente del IdSesion en el header
      const resp = await this.authService.insertarEnsayo(this.ensayoForm);

      if (resp?.esExitoso) {
        this.showFormModal = false;
        this.showSuccessModal = true; // MOSTRAR MODAL DE ÉXITO OBLIGATORIO
        this.cargarEnsayos(); // Recargar tabla de deudores
        this.resetForm();
      } else {
        alert(resp?.mensaje || 'Error al guardar el registro');
      }
    } catch (error) {
      alert('Error de conexión con el servidor de Riobamba.');
    }
  }

  openModal() {
    this.resetForm();
    this.showFormModal = true;
  }

  closeModals() {
    this.showFormModal = false;
    this.showSuccessModal = false;
  }

  resetForm() {
    this.ensayoForm = {
      ensayo: { idCliente: 0, descripcion: '', ensayos: [{ idCatalogo: 1, monto: 0, abono: 0 }] },
      abono: 0
    };
  }

  // Lógica basada en las propiedades del JSON de deudores
  getEstadoDeuda(item: any) {
    const saldo = item.saldoPendiente ?? 0;
    const total = item.totalAPagar ?? 0;

    if (saldo === total && total > 0) return 'Pendiente';
    if (saldo > 0 && saldo < total) return 'Abonado';
    if (saldo === 0 && total > 0) return 'Pagado';
    return 'Pendiente';
  }
}
