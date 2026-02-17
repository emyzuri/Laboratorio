import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-buscar-ensayos',
  standalone: false, // Vinculado a BuscarEnsayosModule
  templateUrl: './buscar-ensayos.html',
  styleUrls: ['./buscar-ensayos.scss']
})
export class BuscarEnsayosComponent {
  private readonly authService = inject(AuthService);

  fechaBusqueda: string = '';
  listaResultados: any[] = [];
  busquedaRealizada: boolean = false;

  async buscarEnsayos() {
    if (!this.fechaBusqueda) return;

    try {
      // Consumimos el nuevo endpoint detallado
      const resp = await this.authService.getEnsayosDetallados();
      this.busquedaRealizada = true;

      if (resp?.esExitoso) {
        // FILTRADO CLAVE: Usamos 'FechaRegistro' con Mayúscula
        this.listaResultados = (resp.datos || []).filter((e: any) => {
          if (e.FechaRegistro) {
            // Comparamos solo la parte de la fecha YYYY-MM-DD
            const fechaLimpia = e.FechaRegistro.split('T')[0];
            return fechaLimpia === this.fechaBusqueda;
          }
          return false;
        });
      } else {
        this.listaResultados = [];
      }
    } catch (error) {
      console.error('Error al buscar ensayos por fecha:', error);
      this.listaResultados = [];
    }
  }
}
