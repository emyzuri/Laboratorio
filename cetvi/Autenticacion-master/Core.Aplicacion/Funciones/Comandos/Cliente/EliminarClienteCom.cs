using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class EliminarClienteCom : IRequest<bool>
    {
        public int IdCliente { get; set; }

        public EliminarClienteCom(int idCliente)
        {
            IdCliente = idCliente;
        }
    }
}