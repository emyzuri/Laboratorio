using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Genera pdf con el reporte de ensayos realizados en un rango de fechas específico. Permite a los usuarios obtener un informe detallado de los ensayos realizados dentro del período seleccionado, facilitando el análisis y seguimiento de las actividades de ensayo.
    /// </summary>
    public record ReporteEnsayoPorFechaCom() : IRequest<byte[]>;
}
