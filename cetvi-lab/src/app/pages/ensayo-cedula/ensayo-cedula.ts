import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-ensayo-cedula',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ensayo-cedula.html',
  styleUrls: ['./ensayo-cedula.scss']
})
export class EnsayoCedulaComponent {
  private authService = inject(AuthService);
  private sanitizer = inject(DomSanitizer);

  // Filtros de búsqueda
  cedula: string = '';
  fechaInicio: string = '';
  fechaFin: string = '';

  // Datos y UI
  resultados: any[] = [];
  busquedaRealizada: boolean = false;

  // Manejo de PDF
  pdfUrl: SafeResourceUrl | null = null;
  showPdfModal: boolean = false;

  async buscar() {
    if (!this.cedula || !this.fechaInicio || !this.fechaFin) return;

    try {
      const resp = await this.authService.getEnsayosPorCedula(this.cedula, this.fechaInicio, this.fechaFin);
      if (resp?.esExitoso) {
        this.resultados = resp.datos;
        this.busquedaRealizada = true;
      }
    } catch (error) {
      console.error('Error en búsqueda:', error);
      this.resultados = [];
    }
  }

  async verReporte() {
    if (!this.cedula || !this.fechaInicio || !this.fechaFin) return;

    try {
      // Llamada al endpoint de reporte por cliente
      const blob = await this.authService.generarReportePorCliente(this.cedula, this.fechaInicio, this.fechaFin);
      if (blob) {
        const url = URL.createObjectURL(blob);
        this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        this.showPdfModal = true;
      }
    } catch (error) {
      console.error('Error al generar el reporte:', error);
    }
  }

  cerrarModal() {
    this.showPdfModal = false;
    this.pdfUrl = null;
  }

  // Función corregida para evitar el error en el HTML
  // Dentro de tu clase EnsayoCedulaComponent
soloNumeros(event: KeyboardEvent) {
  const charCode = event.which ? event.which : event.keyCode;
  // Permitir solo números (48-57)
  if (charCode > 31 && (charCode < 48 || charCode > 57)) {
    event.preventDefault();
  }
}
}
