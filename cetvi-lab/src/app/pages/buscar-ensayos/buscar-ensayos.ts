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

  // Variable que coincide con el [(ngModel)] del HTML
  fechaBusqueda: string = '';

  listaResultados: any[] = [];
  busquedaRealizada: boolean = false;

  async buscarEnsayos() {
    // Si no hay fecha seleccionada, no hacemos la búsqueda
    if (!this.fechaBusqueda) return;

    try {
      this.busquedaRealizada = true;

      // Creamos el objeto fecha para la comparación
      // Usamos toDateString() para comparar solo año, mes y día (sin horas)
      const fechaFiltro = new Date(this.fechaBusqueda).toDateString();

      const resp = await this.authService.getEnsayosDeudores();

      if (resp?.esExitoso && resp.datos) {

        this.listaResultados = resp.datos.filter((e: any) => {
          if (!e.fechaRegistro) return false;

          const fechaReg = new Date(e.fechaRegistro).toDateString();

          // Retorna verdadero si el registro coincide con la fecha seleccionada
          return fechaReg === fechaFiltro;
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
