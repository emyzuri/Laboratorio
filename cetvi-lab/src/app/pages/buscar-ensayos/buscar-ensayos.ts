import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-buscar-ensayos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './buscar-ensayos.html',
  styleUrls: ['./buscar-ensayos.scss']
})
export class BuscarEnsayosComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly sanitizer = inject(DomSanitizer);

  // Filtros de búsqueda
  fechaInicio: string = '';
  fechaFin: string = '';
  filtroNombre: string = '';

  // Datos para la tabla
  // Cambiamos el nombre o agregamos la referencia para que coincida con tu HTML
  originalResultados: any[] = [];
  busquedaRealizada: boolean = false;

  // Manejo del Modal y PDF
  showPdfModal: boolean = false;
  pdfUrl: SafeResourceUrl | null = null;
  pdfBlob: Blob | null = null;

  ngOnInit(): void {
    // Inicialización opcional
  }

  // Getter para el filtrado en tiempo real por nombre/cédula
  get listaResultadosFiltrados() {
    const bus = this.filtroNombre.toLowerCase().trim();
    if (!bus) return this.originalResultados;

    return this.originalResultados.filter(item =>
      (item.nombreCompleto || '').toLowerCase().includes(bus) ||
      (item.cedula || '').includes(bus)
    );
  }

  /**
   * Método unificado para buscar.
   * Usa originalResultados para que el getter 'listaResultadosFiltrados' funcione.
   */
  async buscar() {
    if (!this.fechaInicio || !this.fechaFin) return;

    try {
      this.busquedaRealizada = true;
      // Llamada al servicio con el nuevo endpoint por rango
      const resp = await this.authService.getEnsayosPorRangoFechas(this.fechaInicio, this.fechaFin);

      if (resp?.esExitoso && Array.isArray(resp.datos)) {
        // Mapeamos y normalizamos los datos
        this.originalResultados = resp.datos.map((e: any) => ({
          ...e,
          nombreCompleto: (e.nombreCompleto || '').toUpperCase(),
          totalAbonado: Number(e.totalAbonado ?? 0),
          totalAPagar: Number(e.totalAPagar ?? 0),
          saldoPendiente: Number(e.saldoPendiente ?? 0),
          fechaRegistro: e.fechaRegistro
        }));
      } else {
        this.originalResultados = [];
      }
    } catch (error) {
      console.error('Error en búsqueda:', error);
      this.originalResultados = [];
    }
  }

  // --- LÓGICA DEL MODAL DE REPORTE ---

  async verReporte() {
    if (!this.fechaInicio || !this.fechaFin) return;

    try {
      this.pdfBlob = await this.authService.generarReporteEnsayos(this.fechaInicio, this.fechaFin);

      if (this.pdfBlob) {
        const unsafeUrl = URL.createObjectURL(this.pdfBlob);
        this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(unsafeUrl);
        this.showPdfModal = true;
      }
    } catch (error) {
      console.error('Error al generar el informe PDF:', error);
    }
  }

  cerrarModal() {
    this.showPdfModal = false;
    this.pdfUrl = null;
  }

  // Función para validar entrada de solo números (Cédula)
  soloNumeros(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }
}
