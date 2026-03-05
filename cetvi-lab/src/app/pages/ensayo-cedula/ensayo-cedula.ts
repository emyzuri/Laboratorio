import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-ensayo-cedula',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ensayo-cedula.html',
  styleUrls: ['./ensayo-cedula.scss']
})
export class EnsayoCedulaComponent {
  private authService = inject(AuthService);

  cedula: string = '';
  fechaInicio: string = '';
  fechaFin: string = '';
  resultados: any[] = [];
  busquedaRealizada: boolean = false;

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
    }
  }
}
