using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using Pipelines.Sockets.Unofficial.Arenas;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ValidarClienteHandler : IRequestHandler<ValidarClienteCom, ClienteModel>
    {
        /// <summary>
        /// Servicio de cliente
        /// </summary>
        readonly ICliente iCLiente;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="iCLiente">Servicio de cliente</param>
        /// <exception cref="ArgumentException">Control de errores</exception>
        public ValidarClienteHandler(ICliente iCLiente)
        {
            this.iCLiente = iCLiente ?? throw new ArgumentException(nameof(iCLiente));
        }

        /// <summary>
        /// Logica de consula del cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Cliente</returns>
        /// <exception cref="NotImplementedException">Control de errores</exception>
        public async Task<ClienteModel> Handle(ValidarClienteCom request, CancellationToken cancellationToken)
        {
            ClienteModel cliente = await iCLiente.ObtenerCliente(request.Usuario, "prueba");
            if (cliente != null)
            {
                if (cliente.Clave.Equals(request.Clave, StringComparison.InvariantCultureIgnoreCase))
                {
                    return cliente;
                }
                else
                {
                    await iCLiente.ActualizarNumeroLogguin(cliente.IdCliente);
                    throw new Exception("message");
                }
            }
            else
            {
                throw new Exception("message");
            }
           
        }
    }
}
