using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    ///  Genera un reporte en formato PDF con los ensayos realizados por cada cliente. Este comando permite a los usuarios obtener un informe detallado de los ensayos asociados a cada cliente, facilitando el análisis y seguimiento de las actividades de ensayo por cliente.
    /// </summary>
    public record ReporteEnsayoClienteCom() : IRequest<byte[]>;
}
