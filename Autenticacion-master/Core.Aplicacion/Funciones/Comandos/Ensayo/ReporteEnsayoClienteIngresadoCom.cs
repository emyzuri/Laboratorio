using MediatR;
using System;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ReporteEnsayoClienteIngresadoCom : IRequest<byte[]>
    {
        public string Cedula { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public ReporteEnsayoClienteIngresadoCom(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            Cedula = cedula;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public ReporteEnsayoClienteIngresadoCom() { }
    }
}