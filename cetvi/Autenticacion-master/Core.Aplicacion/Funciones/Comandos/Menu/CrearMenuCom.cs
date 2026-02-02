using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearMenuCom : IRequest<int>
    {

        public int IdMenu { get; set; }
        public int IdPadre { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string URL { get; set; }
        public string Transaccion { get; set; }
        public int Orden { get; set; }
        public bool Visible { get; set; }
        public bool Estado { get; set; }
        public string Icono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }


    }
}
