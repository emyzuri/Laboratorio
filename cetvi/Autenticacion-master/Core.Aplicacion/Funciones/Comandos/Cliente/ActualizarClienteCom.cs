using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ActualizarClienteCom : IRequest<Respuesta<bool>>
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Titulo { get; set; }
    }
}