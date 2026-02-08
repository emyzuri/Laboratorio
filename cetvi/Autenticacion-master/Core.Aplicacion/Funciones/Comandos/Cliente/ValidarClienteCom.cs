using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ValidarClienteCom : IRequest<ClienteModel>
    {
        public int IdCliente { get; set; }

        /// <param name="idCliente">Clave del usuario</param>
        public ValidarClienteCom(int idCliente)
        {
            this.IdCliente = idCliente;
        }
    }
}
