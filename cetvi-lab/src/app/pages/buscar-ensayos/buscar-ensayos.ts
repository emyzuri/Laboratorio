import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-buscar-ensayos',
  standalone: false,
  templateUrl: './buscar-ensayos.html',
  styleUrls: ['./buscar-ensayos.scss']
})
export class BuscarEnsayosComponent {

  private readonly authService = inject(AuthService);

  fechaInicio: string = '';
  fechaFin: string = '';

  listaResultados: any[] = [];
  busquedaRealizada: boolean = false;

  async buscarEnsayos() {

    if (!this.fechaInicio || !this.fechaFin) return;

    try {

      this.busquedaRealizada = true;

      const start = new Date(this.fechaInicio);
      const end = new Date(this.fechaFin);
      end.setHours(23, 59, 59, 999);

      const resp = await this.authService.getEnsayosDeudores();

      if (resp?.esExitoso && resp.datos) {

        this.listaResultados = resp.datos.filter((e: any) => {

          if (!e.fechaRegistro) return false;

          const fechaRegistro = new Date(e.fechaRegistro);

          return fechaRegistro >= start && fechaRegistro <= end;
        });

      } else {
        this.listaResultados = [];
      }

    } catch (error) {

      console.error('Error al filtrar ensayos:', error);
      this.listaResultados = [];
      alert('🚫 No se pudo obtener la información del servidor');
    }
  }
}
