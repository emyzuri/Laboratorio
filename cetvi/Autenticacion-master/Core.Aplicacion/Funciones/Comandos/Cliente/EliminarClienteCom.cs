using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class EliminarClienteCom : IRequest<Unit>
    {
        public int IdCliente { get; set; }

        public EliminarClienteCom(int idCliente)
        {
            IdCliente = idCliente;
        }
    }
}