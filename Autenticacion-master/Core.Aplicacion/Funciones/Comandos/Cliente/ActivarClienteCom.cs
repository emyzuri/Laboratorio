
using Core.Dominio.Request.Clientes;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ActivarClienteCom: IRequest<Unit>
    {
        public string Cedula { get; set; }
        public ActivarClienteCom(ActivarClienteRequest cliente)
        {
            this.Cedula = cliente.Cedula;
        }
    }
}
