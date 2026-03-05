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
  originalResultados: any[] = [];
  busquedaRealizada: boolean = false;

  // Manejo del Modal y PDF
  showPdfModal: boolean = false;
  pdfUrl: SafeResourceUrl | null = null;
  pdfBlob: Blob | null = null;

  ngOnInit(): void {
    // Se puede inicializar con la fecha de hoy si se requiere
  }

  /**
   * Getter para filtrar la lista cargada por nombre o cédula en tiempo real
   */
  get listaResultadosFiltrados() {
    const bus = this.filtroNombre.toLowerCase().trim();
    if (!bus) return this.originalResultados;

    return this.originalResultados.filter(item =>
      (item.nombreCompleto || '').toLowerCase().includes(bus) ||
      (item.cedula || '').includes(bus)
    );
  }

  /**
   * Carga los ensayos desde el servicio y aplica el filtro de fechas.
   * IMPORTANTE: No filtra por saldo para que asomen todos los registros (incluso saldo $0).
   */
  async buscarEnsayos() {
  if (!this.fechaInicio || !this.fechaFin) return;

  try {
    this.busquedaRealizada = true;
    const resp = await this.authService.getEnsayosDeudores();

    if (resp?.esExitoso && resp.datos) {
      // Creamos las fechas de rango y normalizamos a las 00:00:00
      const start = new Date(this.fechaInicio + 'T00:00:00');
      const end = new Date(this.fechaFin + 'T23:59:59');

      this.originalResultados = resp.datos.filter((e: any) => {
        if (!e.fechaRegistro) return false;

        // Convertimos la fecha del backend (que viene con T00:00:00) a objeto Date
        const fechaReg = new Date(e.fechaRegistro);

        // Comparamos los tiempos para evitar errores de zona horaria
        return fechaReg.getTime() >= start.getTime() && fechaReg.getTime() <= end.getTime();
      });
    }
  } catch (error) {
    console.error('Error al filtrar:', error);
    this.originalResultados = [];
  }
}

  // --- LÓGICA DEL MODAL DE REPORTE ---

  /**
   * Solicita el archivo PDF al backend y genera la URL segura para el iframe
   */
  async verReporte() {
    if (!this.fechaInicio || !this.fechaFin) return;

    try {
      // Obtiene el Blob del reporte diseñado para LABORATORIO CETVI
      this.pdfBlob = await this.authService.generarReporteEnsayos(this.fechaInicio, this.fechaFin);

      if (this.pdfBlob) {
        const unsafeUrl = URL.createObjectURL(this.pdfBlob);
        this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(unsafeUrl);
        this.showPdfModal = true; // Activa el modal overlay
      }
    } catch (error) {
      console.error('Error al generar el informe PDF:', error);
    }
  }

  /**
   * Permite la descarga directa del archivo PDF generado
   */
  descargarPdf() {
    if (!this.pdfBlob) return;
    const url = window.URL.createObjectURL(this.pdfBlob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `Reporte_Ensayos_CETVI_${this.fechaInicio}_${this.fechaFin}.pdf`;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  /**
   * Cierra el modal y limpia la URL del PDF para liberar recursos
   */
  cerrarModal() {
    this.showPdfModal = false;
    this.pdfUrl = null;
    // Opcionalmente liberar el Blob si no se descargó
  }
}
